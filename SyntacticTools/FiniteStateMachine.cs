using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LexicalTools;

namespace SyntacticTools
{
    public class FiniteStateMachine
    {
        // read for ll1 from file 
        public StreamReader StreamReader { get; set; }
        public string StartNotTerminal { get; set; }

        // states of machine
        public List<RecordTableLL1> TableParseLL1 { get; private set; } = new List<RecordTableLL1>();

        // grammar 
        public List<Rule> Grammar { get; private set; } = new List<Rule>();

        // current state of machine
        private int CurrentStatePosition = 0;

        // stack for save states
        private Stack<int?> StackSavedStates { get; set; } = new Stack<int?>();

        // error description
        public Error Error { get; private set; }

        //expected symbols for alternative rules
        public List<string> ExpectedSymbolsForRules { get; private set; } = new List<string>();

        // code of fours
        public List<Four> CodeOfFours { get; private set; } = new List<Four>();

        // storage for identifier
        private List<Identifier> LocalIdentifiers { get; set; } = new List<Identifier>();
        // storage for funcs 
        private List<Function> Functions { get; set; } = new List<Function>()
        {
            new Function(){Name = "ReadFile"},
            new Function(){Name = "IsFileExist"},
            new Function(){Name = "PrintFile"},
            new Function(){Name = "CountColumns"},
            new Function(){Name = "PrintMatrix"},
            new Function(){Name = "ReadMatrix"},
            new Function(){Name = "PrintLine"},
            new Function(){Name = "ReadLine"},
            new Function(){Name = "CountRows"}
        };




        //----------------------------------------------------------------------------------

        public FiniteStateMachine(StreamReader streamReader, string startNotTerminal)
        {
            StreamReader = streamReader;
            StartNotTerminal = startNotTerminal;
        }

        public StateMachine Handle(Lexem lexem)
        {
            eps: RecordTableLL1 currentRecordTable = TableParseLL1[CurrentStatePosition];
            bool isCorrectLexem;

            #region I) go along while not accept rule            
            while (!TableParseLL1[CurrentStatePosition].Accept)
            {

                // 1 correct lexem
                isCorrectLexem = CheckLexem(lexem, CurrentStatePosition);

                // 2 admit error
                if (!isCorrectLexem && currentRecordTable.Error) // not correct and error
                {
                    // add error
                    Error = new Error("Syntax error - Expected lexem(s): " + ExceptedLexems(currentRecordTable.ExpectedLexems));
                    Error.PositionInMultiStr = lexem.PositionInMultiStr;

                    CurrentStatePosition = 0;
                    return StateMachine.ErrorSyntax;
                }
                if (!isCorrectLexem && !currentRecordTable.Error) // not correct and not error go in alterantive left rule
                {
                    CurrentStatePosition++;
                    currentRecordTable = TableParseLL1[CurrentStatePosition];
                    continue;
                }

                //  3 need to stack               
                if (currentRecordTable.ToStack != null)
                    StackSavedStates.Push(currentRecordTable.ToStack);

                // 4 need from stack
                if (currentRecordTable.FromStack)
                {
                    if (StackSavedStates.Count == 0) // end of program
                    {
                        CurrentStatePosition = 0;
                        return StateMachine.EndProgram;
                    }
                    else // need next state from stack
                    {
                        currentRecordTable.NextState = StackSavedStates.Pop().Value;
                    }
                }

                CurrentStatePosition = currentRecordTable.NextState;
                currentRecordTable = TableParseLL1[CurrentStatePosition];
            }
            #endregion

            #region II) accept rule

            // 1 correct lexem
            isCorrectLexem = CheckLexem(lexem, CurrentStatePosition);

            // 2 error lexem
            if (!isCorrectLexem) // not correct and error
            {
                Error = new Error("Syntax error - Expected lexem(s): " + ExceptedLexems(currentRecordTable.ExpectedLexems));
                Error.PositionInMultiStr = lexem.PositionInMultiStr;

                CurrentStatePosition = 0;
                return StateMachine.ErrorSyntax;
            }

            // 3 need from stack
            if (currentRecordTable.FromStack)
            {
                if (StackSavedStates.Count == 0) // end of program
                {
                    CurrentStatePosition = 0;
                    return StateMachine.EndProgram;
                }
                else // need next state from stack
                {
                    currentRecordTable.NextState = StackSavedStates.Pop().Value;
                }
            }

            // expected lexem eps, check again
            if (currentRecordTable.ExpectedLexems.Count == 1 && currentRecordTable.ExpectedLexems[0].Equals("eps"))
            {
                if (TableParseLL1[CurrentStatePosition].NameOfAction != null) // have action in terminal
                {
                    foreach (var nameOfAction in TableParseLL1[CurrentStatePosition].NameOfAction)
                    {
                        StateMachine stateMachine = Action(lexem, nameOfAction);
                        if (stateMachine == StateMachine.ErrorSemantic)
                            return StateMachine.ErrorSemantic; // semantic error
                    }
                }

                CurrentStatePosition = currentRecordTable.NextState;
                currentRecordTable = TableParseLL1[CurrentStatePosition];

                goto eps;
            }

            //---- before next state do action on terminal

            if (TableParseLL1[CurrentStatePosition].NameOfAction != null) // have action in terminal
            {
                foreach (var nameOfAction in TableParseLL1[CurrentStatePosition].NameOfAction)
                {
                    StateMachine stateMachine = Action(lexem, nameOfAction);
                    if (stateMachine == StateMachine.ErrorSemantic)
                        return StateMachine.ErrorSemantic; // semantic error
                }
            }
            //------

            CurrentStatePosition = currentRecordTable.NextState;
            currentRecordTable = TableParseLL1[CurrentStatePosition];

            #endregion



            return StateMachine.Cool;
        }

        public void BuildMachine()
        {
            // read all rules from file and divide right and left part            
            while (!StreamReader.EndOfStream)
            {
                string[] rule = Regex.Split(StreamReader.ReadLine(), "->");
                if (rule.Length != 2) // no valid format rule
                {
                    throw new BuildFSMException("Not valid format rules");
                }
                Grammar.Add(new Rule()
                {
                    LeftPartRule = new Rule.LeftPart() { Name = rule[0] },
                    RightPartRule = new Rule.RightPart() { Terms = rule[1].Split(' ') }
                });
            }

            // empty grammars
            if (Grammar.Count == 0)
                throw new BuildFSMException("Empty grammar!!!");

            // number rule
            NumericRules(Grammar);

            //// fill machine

            int quantCheckAlterRules = 0;
            for (int iterGram = 0; iterGram < Grammar.Count; iterGram++)
            {
                if (quantCheckAlterRules == 0) // that's why not go along alternative rules
                {
                    quantCheckAlterRules = AnalysLeftAlternativeParts(Grammar[iterGram], Grammar);
                }
                AnalysRightPart(Grammar[iterGram], Grammar);
                quantCheckAlterRules -= (quantCheckAlterRules == 0 ? 0 : 1);
            }

            // add actions in table parse
            AddActionsToTableParse();
        }

#warning need realize
        #region Fields for action in table parse

        //stack for sign operations        
        private Stack<Lexem> DownStackLexems { get; set; } = new Stack<Lexem>();
        private Stack<Lexem> DownStackArithmeticExpressResults { get; set; } = new Stack<Lexem>();
        private Stack<Lexem> StackOperations { get; set; } = new Stack<Lexem>();
        private Stack<int> StackOfLabel { get; set; } = new Stack<int>();
        private Stack<int> UpStackIdentOnLongDistance { get; set; } = new Stack<int>();

        private Lexem identifierOnLongDistance = new Lexem();

        //var for work with stack
        private int label = -1, n = 0;

        private string typeOfIdent = ""; // for save type         
        private bool inDeclareFunc = false; // for params identifier position in declare func
        private bool inCallFunc = false; // ident call func

        // identifier for long distance transfer        
        private Lexem currentIdentifier = new Lexem();

        // one time go in arithmetic expression
        private bool firstInArithmeticExpress = true;
        #endregion

        private StateMachine Action(Lexem lexem, string action)
        {
            switch (action)
            {
                //1) create list identifier
                case "S1":
                    LocalIdentifiers = new List<Identifier>();
                    break;
                // 2) for saved identifiers type
                case "S2":
                    typeOfIdent = lexem.Lex;
                    break;
                // 3) for saved name identifiers and check on repeated names
                case "S3":
                    if (inDeclareFunc)
                    {
                        Functions[Functions.Count - 1].ParamsList.Add(
                            new Function.Param() { Type = typeOfIdent, Name = lexem.Lex });
                    }
                    else
                    {
                        if (LocalIdentifiers.Find(match => match.Name == lexem.Lex) != null)
                        {
                            Error = new Error("Semantic error - such identifier  <" + lexem.Lex + "\">  already exist");
                            Error.PositionInMultiStr = lexem.PositionInMultiStr;
                            return StateMachine.ErrorSemantic;
                        }
                        else
                        {
                            LocalIdentifiers.Add(new Identifier()
                            { Name = lexem.Lex, Type = typeOfIdent });

                        }

                    }

                    break;
                // 4) add name function and get return type from var typeOfIdent
                case "S4":
                    // such func already exist
                    if (Functions.Find(match => match.Name == lexem.Lex) != null)
                    {
                        Error = new Error("Semantic error - such name func  <" + lexem.Lex + ">  already exist");
                        Error.PositionInMultiStr = lexem.PositionInMultiStr;
                        return StateMachine.ErrorSemantic;
                    }
                    else
                    {
                        Functions.Add(new Function()
                        {
                            Name = lexem.Lex,
                            ReturnType = typeOfIdent,
                            ParamsList = new List<Function.Param>()
                        });
                    }
                    break;
                // 5) we are in declare func,  add flag
                case "S5":
                    inDeclareFunc = true;
                    break;
                // 6) we leave declare func,  remove flag
                case "S6":
                    inDeclareFunc = false;
                    break;
                // 7) usage identifier
                case "S7":
                    currentIdentifier = lexem;
                    break;
                // 8) check identifier on declaration in table ident
                case "S8":
                    if (LocalIdentifiers.Find(match => match.Name == currentIdentifier.Lex) == null)
                    {
                        Error = new Error("Semantic error - such identifier  <" + currentIdentifier.Lex + ">  not declare");
                        Error.PositionInMultiStr = currentIdentifier.PositionInMultiStr;
                        return StateMachine.ErrorSemantic;
                    }
                    break;
                // 9) check identifier on declaration in table func
                case "S9":
                    if (Functions.Find(match => match.Name == currentIdentifier.Lex) == null)
                    {
                        Error = new Error("Semantic error - such func  <" + currentIdentifier.Lex + ">  not declare");
                        Error.PositionInMultiStr = currentIdentifier.PositionInMultiStr;
                        return StateMachine.ErrorSemantic;
                    }
                    break;

                // add first operand to stack lexem
                case "A1":
                    DownStackLexems.Push(lexem);

                    if (!firstInArithmeticExpress)
                    {
                        CodeOfFours.Add(new Four()
                        {
                            SecondOperand = DownStackLexems.Pop().Lex,
                            FirstOperand = DownStackLexems.Pop().Lex,
                            Number = n.ToString(),
                            Operation = StackOperations.Pop().Lex
                        });
                        DownStackLexems.Push(new Lexem() { Lex = n.ToString() });
                        n++; // next fours
                    }
                    firstInArithmeticExpress = false;
                    break;
                //// save sign operation
                case "A2":
                    StackOperations.Push(lexem);
                    break;
                // first ident
                case "A3":
                    identifierOnLongDistance = lexem;
                    break;
                // end of expression      
                case "A4":
                    CodeOfFours.Add(new Four()
                    {
                        FirstOperand = identifierOnLongDistance.Lex,
                        Operation = "=",
                        SecondOperand = DownStackLexems.Pop().Lex,
                        Number = n.ToString()
                    });
                    firstInArithmeticExpress = true;
                    identifierOnLongDistance.Lex = n.ToString();
                    n++;
                    break;
                // first time in arithmetic expression
                case "A5":
                    firstInArithmeticExpress = true;
                    break;
                // add result from arithmetic express to operand
                case "A6":
                    // not last expression
                    if (DownStackLexems.Count > 1)
                    {
                        CodeOfFours.Add(new Four()
                        {
                            SecondOperand = DownStackLexems.Pop().Lex,
                            FirstOperand = DownStackLexems.Pop().Lex,
                            Number = n.ToString(),
                            Operation = StackOperations.Pop().Lex
                        });
                        DownStackLexems.Push(new Lexem() { Lex = n.ToString() });
                        n++; // next fours                       
                    }
                    break;
                // save logical sign
                case "A7":
                    StackOperations.Push(lexem);
                    // get res of first arithm express
                    DownStackArithmeticExpressResults.Push(DownStackLexems.Pop());
                    // save arithmetic express
                    firstInArithmeticExpress = true;
                    break;
                // build four for logical expression
                case "A8":
                    // get res of second arithm express
                    DownStackArithmeticExpressResults.Push(DownStackLexems.Pop());
                    CodeOfFours.Add(new Four()
                    {
                        SecondOperand = DownStackArithmeticExpressResults.Pop().Lex,
                        FirstOperand = DownStackArithmeticExpressResults.Pop().Lex,
                        Number = n.ToString(),
                        Operation = StackOperations.Pop().Lex
                    });
                    //end of arithm express
                    firstInArithmeticExpress = true;

                    // save result of logical express
                    identifierOnLongDistance.Lex = n.ToString();

                    n++; // next fours


                    break;
                #region constructions "if else"
                // after logical express in "if"
                case "B1":
                    CodeOfFours.Add(new Four()
                    {
                        FirstOperand = identifierOnLongDistance.Lex,
                        Operation = "cmp",
                        SecondOperand = "true",
                        Number = n.ToString()
                    });
                    n++;// next number of four

                    CodeOfFours.Add(new Four()
                    {
                        Operation = "JNE",
                        FirstOperand = label.ToString(),
                        SecondOperand = "",
                        Number = n.ToString()
                    });
                    n++; // next number of four

                    StackOfLabel.Push(label); // save label for goto: else or end code

                    label--; // next label
                    break;
                // entry in "else"
                case "B2":
                    // jump to end of constuction if else
                    CodeOfFours.Add(new Four()
                    {
                        Operation = "JMP",
                        FirstOperand = label.ToString(),
                        SecondOperand = "",
                        Number = n.ToString()
                    });

                    n++;

                    // label for entry in else
                    CodeOfFours.Add(new Four()
                    {
                        Operation = "LABEL",
                        FirstOperand = StackOfLabel.Pop().ToString(),
                        SecondOperand = "",
                        Number = n.ToString()
                    });

                    // save label for end construction if else
                    StackOfLabel.Push(label);


                    n++; // next fours
                    label--; // next label


                    break;

                // end of construction "if else"
                case "B3":
                    CodeOfFours.Add(new Four()
                    {
                        Operation = "LABEL",
                        SecondOperand = "",
                        FirstOperand = StackOfLabel.Pop().ToString(),
                        Number = n.ToString()
                    });

                    n++; // next four
                    break;
                #endregion

                #region Actions "for"
                case "C1":
                    UpStackIdentOnLongDistance.Push(int.Parse(identifierOnLongDistance.Lex));
                    break;
                    #endregion
            }

            return StateMachine.Cool;
        }

#warning Realize
        private void AddActionsToTableParse()
        {
            #region Semantic actions
            // 1) create list identifier
            TableParseLL1[47].NameOfAction.Add("S1"); // entry in main
            TableParseLL1[54].NameOfAction.Add("S1"); // antry in function
            TableParseLL1[63].NameOfAction.Add("S1"); // antry in function

            // 2) saved  type       
            TableParseLL1[13].NameOfAction.Add("S2");
            TableParseLL1[20].NameOfAction.Add("S2");
            TableParseLL1[39].NameOfAction.Add("S2");
            TableParseLL1[40].NameOfAction.Add("S2");
            TableParseLL1[41].NameOfAction.Add("S2");
            TableParseLL1[42].NameOfAction.Add("S2");
            TableParseLL1[79].NameOfAction.Add("S2");
            TableParseLL1[348].NameOfAction.Add("S2");

            // 3) saved name identifiers and identifiers and check on repeated names
            TableParseLL1[350].NameOfAction.Add("S3"); // ident matrix var
            TableParseLL1[354].NameOfAction.Add("S3"); // ident any type var
            TableParseLL1[73].NameOfAction.Add("S3"); // ident in params func
            TableParseLL1[84].NameOfAction.Add("S3"); // ident in params func

            // 4) add function type and func
            TableParseLL1[7].NameOfAction.Add("S4");
            TableParseLL1[14].NameOfAction.Add("S4");
            TableParseLL1[24].NameOfAction.Add("S4");
            TableParseLL1[44].NameOfAction.Add("S4");

            // 5) we are in declare func,  add flag
            TableParseLL1[8].NameOfAction.Add("S5");
            TableParseLL1[15].NameOfAction.Add("S5");
            TableParseLL1[25].NameOfAction.Add("S5");

            // 6) we leave declare func,  remove flag
            TableParseLL1[10].NameOfAction.Add("S6");
            TableParseLL1[17].NameOfAction.Add("S6");
            TableParseLL1[27].NameOfAction.Add("S6");

            // 7) usage identifier
            TableParseLL1[93].NameOfAction.Add("S7");
            TableParseLL1[186].NameOfAction.Add("S7");
            TableParseLL1[229].NameOfAction.Add("S7");
            TableParseLL1[312].NameOfAction.Add("S7");
            TableParseLL1[319].NameOfAction.Add("S7");
            TableParseLL1[336].NameOfAction.Add("S7");
            TableParseLL1[151].NameOfAction.Add("S7");
            // 8) check identifier on declaration in table ident
            TableParseLL1[125].NameOfAction.Add("S8");
            TableParseLL1[161].NameOfAction.Add("S8");
            TableParseLL1[159].NameOfAction.Add("S8");
            TableParseLL1[167].NameOfAction.Add("S8");
            TableParseLL1[196].NameOfAction.Add("S8");
            // 9) check identifier on declaration in table func
            TableParseLL1[135].NameOfAction.Add("S9");
            #endregion

            #region Fours actions

            #region //1)--- actions for arithmetic expression            

            // add first operand to stack lexem
            TableParseLL1[186].NameOfAction.Add("A1");
            TableParseLL1[188].NameOfAction.Add("A1");
            TableParseLL1[189].NameOfAction.Add("A1");
            TableParseLL1[190].NameOfAction.Add("A1");

            //// save sign operation
            TableParseLL1[208].NameOfAction.Add("A2");
            TableParseLL1[209].NameOfAction.Add("A2");
            TableParseLL1[210].NameOfAction.Add("A2");
            TableParseLL1[211].NameOfAction.Add("A2");
            TableParseLL1[212].NameOfAction.Add("A2");

            // first time in arithmetic expression
            TableParseLL1[183].NameOfAction.Add("A5");

            // add result from arithmetic express to operand
            TableParseLL1[185].NameOfAction.Add("A6");

            //----
            #endregion

            #region//2) ---operation assign

            // first ident
            TableParseLL1[350].NameOfAction.Add("A3");
            TableParseLL1[354].NameOfAction.Add("A3");
            TableParseLL1[93].NameOfAction.Add("A3");


            // end of expression            
            TableParseLL1[102].NameOfAction.Add("A4");
            TableParseLL1[318].NameOfAction.Add("A4");
            TableParseLL1[95].NameOfAction.Add("A4");
            #endregion

            #region//3) -- logical expression

            // save logical sign
            TableParseLL1[281].NameOfAction.Add("A7");
            TableParseLL1[282].NameOfAction.Add("A7");
            TableParseLL1[283].NameOfAction.Add("A7");
            TableParseLL1[284].NameOfAction.Add("A7");
            TableParseLL1[285].NameOfAction.Add("A7");
            TableParseLL1[286].NameOfAction.Add("A7");

            // build four result for logical express
            TableParseLL1[271].NameOfAction.Add("A8");
            // --
            #endregion

            #region //4) actions for "if else"

            TableParseLL1[253].NameOfAction.Add("B1"); // after logical express in "if"
            TableParseLL1[293].NameOfAction.Add("B2"); // entry in "else"
            TableParseLL1[296].NameOfAction.Add("B3"); // end of construction
            TableParseLL1[297].NameOfAction.Add("B3");

            #endregion

            #region//5) actions for "for"

            // save variables of loop
            TableParseLL1[318].NameOfAction.Add("C1");
            TableParseLL1[322].NameOfAction.Add("C1");

            // label for repeat


            #endregion

            #endregion
        }

        private string ExceptedLexems(List<string> expectedLexems)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[");

            for (int iterLexem = 0; iterLexem < expectedLexems.Count; iterLexem++)
            {
                if (iterLexem == expectedLexems.Count - 1)
                {
                    stringBuilder.Append(expectedLexems[iterLexem]);
                    break;
                }
                stringBuilder.Append(expectedLexems[iterLexem] + ", ");
            }


            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        private bool CheckLexem(Lexem lexem, int currentStatePosition)
        {
            List<string> expectedLexems = TableParseLL1[currentStatePosition].ExpectedLexems;

            if (lexem.LexClass == LexemClass.Identifier)
            {
                if (expectedLexems.Find((match) => match.Equals("identifier")) != null) // identifier
                    return true;
                else
                    return expectedLexems.Find((match) => match.Equals("eps")) != null;
            }
            else if (lexem.LexClass == LexemClass.Literal)
            {
                if (expectedLexems.Find((match) => match.Equals("literal")) != null) // literal
                    return true;
                else
                    return expectedLexems.Find((match) => match.Equals("eps")) != null;
            }
            else
            {
                if (expectedLexems.Find((match) => match.Equals(lexem.Lex, StringComparison.CurrentCultureIgnoreCase)) != null) // key words or other
                    return true;
                else
                    return expectedLexems.Find((match) => match.Equals("eps")) != null;
            }

        }

        public void SaveNumericGrammar(string pathFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(File.Create(pathFile)))
            {
                Grammar.ForEach((rule) =>
               {
                   streamWriter.WriteLine(rule);
               });
            }

        }

        public void SaveRuleAndExpectedSymbols(string pathFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(File.Create(pathFile)))
            {
                ExpectedSymbolsForRules.ForEach((str) =>
                {
                    streamWriter.WriteLine(str);
                });
            }
        }

        public void SaveTableParse(string pathFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(File.Create(pathFile)))
            {
                TableParseLL1.ForEach((rule) =>
                {
                    streamWriter.WriteLine(rule);
                });
            }
        }

        private void AnalysRightPart(Rule rule, List<Rule> grammars)
        {
            Rule.RightPart rightPart = rule.RightPartRule;
            for (int iterRightTerms = 0; iterRightTerms < rightPart.Terms.Length; iterRightTerms++)
            {
                string currentTerm = rightPart.Terms[iterRightTerms];
                // fill record in table 
                RecordTableLL1 recInTable = new RecordTableLL1();
                bool isTerminal = IsTerminal(currentTerm);

                // знаходиться в стані терміналу, то +, інакше -.
                recInTable.Accept = isTerminal;

                /*Завжди +, за виключенням станів відповідних нетерміналам в лівій частині 
                 * альтернативних правил*/
                recInTable.Error = true;

                /*При переході на розбір нетерміналу, якщо він не останній в правій
                 * частині правила*/
                if (!isTerminal && iterRightTerms != rightPart.Terms.Length - 1)
                {
                    recInTable.ToStack = rightPart.Numbers[iterRightTerms + 1];
                }
                else
                {
                    recInTable.ToStack = null;
                }

                /*При останньому терміналі або при пустому правилі здійснюється вилучення
                 * із стеку номера стану*/
                recInTable.FromStack = isTerminal && iterRightTerms == rightPart.Terms.Length - 1;

                /*Якщо стан відповідає терміналу, то він так і ставиться. Якщо нетерміналу - 
                 * то направляючі символи нетермінала, а для альтернативних 
                 * правих частин – направляючі символи цих альтернативних частин.*/
                recInTable.ExpectedLexems = (isTerminal ?
                    new List<string>() { currentTerm } :
                    ExpectedLexemForNotTerminal(grammars, currentTerm, new List<string>()));

                // define next state
                if (isTerminal && iterRightTerms == rightPart.Terms.Length - 1) // last terminal  
                {
                    // from stack next state that!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1
                    recInTable.NextState = 0; // change during analys
                }
                else if (isTerminal)// terminal no in end       
                {
                    recInTable.NextState = rightPart.Numbers[iterRightTerms + 1];
                }
                else // not terminal
                {
                    // number not terminal in left part
                    recInTable.NextState = grammars.Find((Rule rule1) =>
                    { return rule1.LeftPartRule.Equals(currentTerm); }).LeftPartRule.Number;
                }

                TableParseLL1.Add(recInTable);
            }
        }

        public List<Rule> NumericRules(List<Rule> rules)
        {
            int startAlternRule = 0;
            int endAlternRule = 0;
            int numberOfTerm = 0;
            for (int i = 0; i < rules.Count; i += endAlternRule - i)
            {
                // numeric start  left part rule
                startAlternRule = i;
                string startLeftRule = rules[i].LeftPartRule.Name;
                rules[i].LeftPartRule.Number = numberOfTerm;

                // numeric next left partaltern rules
                endAlternRule = i + 1;
                numberOfTerm++;
                while (endAlternRule < rules.Count)
                {
                    if (rules[endAlternRule].LeftPartRule.Equals(startLeftRule))
                        rules[endAlternRule].LeftPartRule.Number = numberOfTerm;
                    else
                        break;

                    endAlternRule++;
                    numberOfTerm++;
                }

                // numeric right part               

                int stAlternRule = 0;
                for (stAlternRule = startAlternRule; stAlternRule < endAlternRule; stAlternRule++)
                {
                    string[] splitRightRules = rules[stAlternRule].RightPartRule.Terms;
                    int rigthRule;
                    rules[stAlternRule].RightPartRule.Numbers = new int[rules[stAlternRule].RightPartRule.Terms.Length];
                    for (rigthRule = 0; rigthRule < splitRightRules.Length; rigthRule++)
                    {
                        rules[stAlternRule].RightPartRule.Numbers[rigthRule] = numberOfTerm;
                        numberOfTerm++;
                    }
                }
            }

            return rules;
        }

        // return quantity left altern rules
        private int AnalysLeftAlternativeParts(Rule rule, List<Rule> grammars)
        {

            // seek alternative rules
            List<Rule> alternativeRules = SeekAlterantiveRules(rule.LeftPartRule.Name, grammars);

            // list string for alternative rules
            List<string> expectedSymbols = new List<string>();

            // add rules in machine
            for (int i = 0; i < alternativeRules.Count; i++)
            {
                string expectedSymbol = alternativeRules[i].RightPartRule.Terms[0];

                TableParseLL1.Add(new RecordTableLL1()
                {
                    Accept = false,
                    Error = false,
                    FromStack = false,
                    ToStack = null,

                    ExpectedLexems = (IsTerminal(expectedSymbol)
                    ? new List<string>() { expectedSymbol }
                    : ExpectedLexemForNotTerminal(grammars, expectedSymbol, new List<string>())),

                    NextState = alternativeRules[i].RightPartRule.Numbers[0]
                });
                if (i == alternativeRules.Count - 1) // last alternative rule error in true
                    TableParseLL1[TableParseLL1.Count - 1].Error = true;

                // save expected symbols for rules
                ExpectedSymbolsForRules.Add(alternativeRules[0].LeftPartRule.Name + " --- " +
                    string.Join(", ", TableParseLL1[TableParseLL1.Count - 1].ExpectedLexems));

                // check expected lexem
                foreach (var lexem in TableParseLL1[TableParseLL1.Count - 1].ExpectedLexems)
                {
                    // have similar lexem
                    if (expectedSymbols.Find((match) => match.Equals(lexem)) != null)
                        throw new BuildFSMException("Not LL1. " + "In rule - " + alternativeRules[0].LeftPartRule.Name);
                    expectedSymbols.Add(lexem);
                }

            }

            return alternativeRules.Count;
        }

        private List<string> ExpectedLexemForNotTerminal(List<Rule> grammars, string notTerminal, List<string> lexems)
        {
            // seek all alternative notTerminals
            List<Rule> allNotTerminals = grammars.FindAll((Rule rule) =>
            {
                return rule.LeftPartRule.Name == notTerminal;
            });

            // seek direct symbol
            List<string> directSymbols = new List<string>();
            allNotTerminals.ForEach((Rule rule) =>
            {
                directSymbols.Add(rule.RightPartRule.Terms[0]);
            }
            );

            // check direct symbol
            foreach (var directSymbol in directSymbols)
            {
                if (IsTerminal(directSymbol))
                    lexems.Add(directSymbol);
                else
                    ExpectedLexemForNotTerminal(grammars, directSymbol, lexems);
            }

            return lexems;
        }

        private bool IsTerminal(string directSymbol)
        {
            if (char.IsUpper(directSymbol[0])) // in terminal all lower character 
                return false;
            return true;
        }

        private List<Rule> SeekAlterantiveRules(string notTerminal, List<Rule> grammars)
        {
            return grammars.FindAll((Rule match) => match.LeftPartRule.Equals(notTerminal));
        }
    }

}

