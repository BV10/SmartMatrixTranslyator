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
        private Stack<int> StackSavedStates { get; set; } = new Stack<int>();

        // error description
        public Error ErrorSyntax { get; private set; }

        //----------------------------------------------------------------------------------

        public FiniteStateMachine(StreamReader streamReader, string startNotTerminal)
        {
            StreamReader = streamReader;
            StartNotTerminal = startNotTerminal;
        }

        public StateMachine Handle(Lexem lexem)
        {
            RecordTableLL1 currentRecordTable = TableParseLL1[CurrentStatePosition];
            // 1 correct lexem
            bool isCorrectLexem = CheckLexem(lexem, CurrentStatePosition);

            // 2 admit error
            if (!isCorrectLexem && currentRecordTable.Error)
            {
                ErrorSyntax = new Error(ExceptedLexems(currentRecordTable.ExpectedLexems));
                CurrentStatePosition = 0;
                return StateMachine.Error;
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
                    currentRecordTable.NextState = StackSavedStates.Pop();
                }                   
            }

            CurrentStatePosition = currentRecordTable.NextState;

            return 0;
        }

        private string ExceptedLexems(List<string> expectedLexems)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Error syntax: expected: ");

            expectedLexems.ForEach((lexem) =>
            {
                stringBuilder.Append(lexem + " ");
            });
            
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

            //Stack<int> stackStates = new Stack<int>();
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

            TableParseLL1.ForEach((RecordTableLL1 rec) => Console.WriteLine(rec));
        }

        public void SaveNumericGrammar(string pathFile)
        {
            StreamWriter streamWriter = new StreamWriter(File.Create(pathFile));
            Grammar.ForEach((rule) =>
            {
                streamWriter.WriteLine(rule);
            });
        }

        public void SaveTableParse(string pathFile)
        {
            StreamWriter streamWriter = new StreamWriter(File.Create(pathFile));
            TableParseLL1.ForEach((rule) =>
            {
                streamWriter.WriteLine(rule);
            });
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
                    recInTable.ToStack = rightPart.Numbers[iterRightTerms];
                }
                else
                {
                    recInTable.ToStack = null;
                }

                /*При останньому терміналі або при пустому правилі здійснюється вилу-чення
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

