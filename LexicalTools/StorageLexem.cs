using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalTools
{
    public class StorageLexem
    {
        
        private List<string> listKeyWords;
        private List<string> listOperations;
        private List<string> listSeparators;
        private List<string> listIdentifiers;
        private int countOfSystemFunc;
        private List<string> listLiterals = new List<string>();

        private List<Lexem> listLexem = new List<Lexem>(); // saved number class, and number in class

        public StorageLexem()
        {
            ListKeyWords = new List<string>
            {
                "Det", "Transp", "boolean", "double", "else", "false", "for", "if", "integer", 
                "matrix", "string", "true", "return", "void", 
            };            

            ListOperations = new List<string>
            {
                "=", "!",  "!=", "%", "&&", "*", "+", "-", "/", "<", "<=", "==", ">", ">=", "||",
                "+=","-=","/=","*=","%="
            };            

            ListSeparators = new List<string>
            {
               "{", "}", "(", ")", "[", "]", ";", ","
            };

            ListIdentifiers = new List<string>
            {
                "ReadFile", "IsFileExist", "PrintFile", "CountColumns", "CountRows", "PrintMatrix",
                "ReadMatrix", "PrintLine", "ReadLine"
            };
            CountOfSystemFunc = ListIdentifiers.Count;
        }

        public List<string> ListSeparators { get => listSeparators; set => listSeparators = value; }
        public List<string> ListKeyWords { get => listKeyWords; set => listKeyWords = value; }
        public List<string> ListOperations { get => listOperations; set => listOperations = value; }
        public List<string> ListIdentifiers { get => listIdentifiers; set => listIdentifiers = value; }
        public int CountOfSystemFunc { get => countOfSystemFunc; private set => countOfSystemFunc = value; }
        public List<string> ListLiterals { get => listLiterals; set => listLiterals = value; }
        internal List<Lexem> ListLexem { get => listLexem; private set => listLexem = value; }      

        public void Add(string lexem, LexemClass lexemClass, PositionInMultiStr positionInMultiStr)
        {
            switch(lexemClass)
            {
                case LexemClass.Identifier:
                    {
                        if (!IsSystemFunc(lexem))
                        {
                            ListIdentifiers.Add(lexem);
                            ListLexem.Add(new Lexem
                            {
                                NumberInClass = ListIdentifiers.Count - 1,
                                LexClass = lexemClass,
                                Lex = lexem,
                                PositionInMultiStr = positionInMultiStr
                            });
                        }
                        else
                        {
                            ListLexem.Add(new Lexem
                            {
                                Lex = lexem,
                                LexClass = lexemClass,
                                NumberInClass = ListIdentifiers.IndexOf(lexem),
                                PositionInMultiStr = positionInMultiStr
                            });
                        }
                    }
                    break;                
                case LexemClass.Literal:
                    {
                        ListLiterals.Add(lexem);
                        ListLexem.Add(new Lexem
                        {
                            NumberInClass = ListLiterals.Count - 1,
                            LexClass = lexemClass,
                            Lex = lexem,
                            PositionInMultiStr = positionInMultiStr
                        });
                    }
                    break;
                case LexemClass.KeyWord:
                    {
                        ListLexem.Add(new Lexem
                        {
                            NumberInClass = ListKeyWords.IndexOf(lexem),
                            LexClass = lexemClass,
                            Lex = lexem,
                            PositionInMultiStr = positionInMultiStr
                        });
                    }
                    break;
                case LexemClass.Operations:
                    {
                        ListLexem.Add(new Lexem
                        {
                            NumberInClass = ListOperations.IndexOf(lexem),
                            LexClass = lexemClass,
                            Lex = lexem,
                            PositionInMultiStr = positionInMultiStr
                        });
                    }
                    break;
                case LexemClass.Separator:
                    {
                        ListLexem.Add(new Lexem
                        {
                            NumberInClass = ListSeparators.IndexOf(lexem),
                            LexClass = lexemClass,
                            Lex = lexem,
                            PositionInMultiStr = positionInMultiStr
                        });
                    }
                    break;
            }
        }

        public void Reset()
        {
            ListLiterals.Clear();
            ListLexem.Clear();
            ListIdentifiers.RemoveRange(CountOfSystemFunc, ListIdentifiers.Count - CountOfSystemFunc);
            
        }
       
        private bool IsSystemFunc(string identif)
        {
            for(int i=0; i<CountOfSystemFunc; i++)
            {
                if (ListIdentifiers[i].Equals(identif))
                    return true;
            }

            return false;
        }
    }
}
