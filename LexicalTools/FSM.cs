using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LexicalTools
{  
    public class FSM
    {
        public const int Skip_Chars_State = -555;
        public const int Error_State = -666;
        public const int Analys_Lexem_State = -777;       
        public struct State
        {
            public string[] InChars { get; set; }
            public int[] NextStates { get; set; }
            public int DescriptState { get; set; }
        }

        private List<State> listStates = new List<State>
        {
            new State
            {
                DescriptState=0,
                InChars = new string[]{"a", "b", "c", "d", "e", "f", "g", "h", "i", "j-l",
                                        "m", "n-q", "r", "s", "t", "u", "v", "w-z", "A-Z",
                                        "{", "}", "(", ")", "\"", "[", "]", ";", ",",
                                        ">", "<", "=", "!", "+", "-", "*", "/", "%", "&", "|",
                                        "0-9", " ", "\n", "\r", "\t", ""
                                       },
                NextStates = new int[] {1, 2, 1, 6, 14, 18, 1, 1, 25, 1,
                                        33, 1, 41, 47, 53, 1, 61, 1, 1,
                                        73, 73, 73, 73, 67, 73, 73, 73, 73,
                                        68, 68, 68, 68, 68, 68, 68, 65, 68, 69, 70,
                                        71, 0, 0, 0, 0, Error_State
}
            },

            new State
            {
                DescriptState=1,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=2,
                InChars=new string[]{"o", "a-n", "p-z", "A-Z", "0-9", ""},

                NextStates = new int[]{3, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=3,
                  InChars=new string[]{"o", "a-n", "p-z", "A-Z", "0-9", ""},

                NextStates = new int[]{4, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=4,
                  InChars=new string[]{"l", "a-k", "m-z", "A-Z", "0-9", ""},

                NextStates = new int[]{5, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=5,
                  InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=6,
                  InChars=new string[]{"o", "e", "a-d", "f-n", "p-z", "A-Z", "0-9", ""},

                NextStates = new int[]{7, 12, 1, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=7,
                  InChars=new string[]{"u", "a-t", "v-z", "A-Z", "0-9", ""},

                NextStates = new int[]{8, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=8,
                InChars=new string[]{"b", "a", "c-z", "A-Z", "0-9", ""},

                NextStates = new int[]{9, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=9,
                InChars=new string[]{"l", "a-k", "m-z", "A-Z", "0-9", ""},

                NextStates = new int[]{10, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
             new State
            {
                DescriptState=10,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{11, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=11,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=12,
                  InChars=new string[]{"t", "a-s", "u-z", "A-Z", "0-9", ""},

                NextStates = new int[]{13, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=13,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},
NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=14,
                InChars=new string[]{"l", "a-k", "m-z", "A-Z", "0-9", ""},

                NextStates = new int[]{15, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=15,
                InChars=new string[]{"s", "a-r", "t-z", "A-Z", "0-9", ""},

                NextStates = new int[]{16, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=16,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{17, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=17,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=18,
                InChars=new string[]{"o", "a", "b-n", "p-z", "A-Z", "0-9", ""},

                NextStates = new int[]{19, 21, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=19,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{20, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=20,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=21,
                InChars=new string[]{"l", "a-k", "m-z", "A-Z", "0-9", ""},

                NextStates = new int[]{22, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=22,
                InChars=new string[]{"s", "a-r", "t-z", "A-Z", "0-9",""},

                NextStates = new int[]{23, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=23,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{24, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=24,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=25,
                InChars=new string[]{"f", "n", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{26, 27, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=26,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=27,
                InChars=new string[]{"t", "a-s", "u-z", "A-Z", "0-9", ""},

                NextStates = new int[]{28, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=28,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{29, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=29,
                InChars=new string[]{"g", "a-f", "h-z", "A-Z", "0-9", ""},

                NextStates = new int[]{30, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=30,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},
NextStates = new int[]{31, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=31,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{32, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=32,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=33,
                InChars=new string[]{"a", "b-z", "A-Z", "0-9", ""},

                NextStates = new int[]{34, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=34,
                InChars=new string[]{"t", "i", "a-h", "g-s", "u-z", "A-Z", "0-9", ""},

                NextStates = new int[]{35, 39, 1, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=35,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{36, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=36,
                InChars=new string[]{"i", "a-h", "j-z", "A-Z", "0-9", ""},

                NextStates = new int[]{37, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=37,
                InChars=new string[]{"x", "a-w", "y-z", "A-Z", "0-9", ""},

                NextStates = new int[]{38, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=38,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
             new State
            {
                DescriptState=39,
                InChars=new string[]{"n", "a-m", "o-z", "A-Z", "0-9", ""},

                NextStates = new int[]{40, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
             new State
            {
                DescriptState=40,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=41,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{42, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },


            new State
            {
                DescriptState=42,
                InChars=new string[]{"t", "a-s", "u-z", "A-Z", "0-9", ""},

                NextStates = new int[]{43, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=43,
                InChars=new string[]{"u", "a-t", "v-z", "A-Z", "0-9", ""},

                NextStates = new int[]{44, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=44,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{45, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=45,
                InChars=new string[]{"n", "a-m", "o-z", "A-Z", "0-9", ""},

                NextStates = new int[]{46, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=46,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=47,
                InChars=new string[]{"t", "a-s", "u-z", "A-Z", "0-9", ""},
NextStates = new int[]{48, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=48,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{49, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=49,
                InChars=new string[]{"i", "a-h", "j-z", "A-Z", "0-9", ""},

                NextStates = new int[]{50, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=50,
                InChars=new string[]{"n", "a-m", "o-z", "A-Z", "0-9", ""},

                NextStates = new int[]{51, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=51,
                InChars=new string[]{"g", "a-f", "h-z", "A-Z", "0-9", ""},

                NextStates = new int[]{52, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=52,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },

            new State
            {
                DescriptState=53,
                InChars=new string[]{"r", "a-p", "s-z", "A-Z", "0-9", ""},

                NextStates = new int[]{54, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=54,
                InChars=new string[]{"u", "a", "a-t", "v-z", "A-Z", "0-9", ""},

                NextStates = new int[]{55, 57, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=55,
                InChars=new string[]{"e", "a-d", "f-z", "A-Z", "0-9", ""},

                NextStates = new int[]{56, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=56,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{ 1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=57,
                InChars=new string[]{"n", "a-m", "o-z", "A-Z", "0-9", ""},

                NextStates = new int[]{58, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=58,
                InChars=new string[]{"s", "a-r", "t-z", "A-Z", "0-9", ""},

                NextStates = new int[]{59, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=59,
                InChars=new string[]{"p", "a-o", "q-z", "A-Z", "0-9", ""},

                NextStates = new int[]{60, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },
            new State
            {
                DescriptState=60,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},

                NextStates = new int[]{ 1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=61,
                InChars=new string[]{"o", "a-n", "p-z", "A-Z", "0-9", ""},

                NextStates = new int[]{62, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=62,
                InChars=new string[]{"i", "a-h", "j-z", "A-Z", "0-9", ""},

                NextStates = new int[]{63, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=63,
                InChars=new string[]{"d", "a-c", "e-z", "A-Z", "0-9", ""},

                NextStates = new int[]{64, 1, 1, 1, 1, (int) LexemClass.Identifier}
            },

            new State
            {
                DescriptState=64,
                InChars=new string[]{"a-z", "A-Z", "0-9", ""},
NextStates = new int[]{1, 1, 1, (int) LexemClass.KeyWord}
            },
            new State
            {
                DescriptState=65,
                InChars=new string[]{"/", "=", ""},

                NextStates = new int[]{66, 72, (int) LexemClass.Operations}
            },

            new State
            {
                DescriptState=66,
                InChars=new string[]{"\r", "\n", ""},

                NextStates = new int[]{66, 0, 66}
            },


            new State
            {
                DescriptState=67,
                InChars=new string[]{"\"", "\n", ""},

                NextStates = new int[]{72, Error_State, 67}
            },
            new State
            {
                DescriptState=68,

                InChars=new string[]{"=", ""},

                NextStates = new int[]{73, (int) LexemClass.Operations}
            },
            new State
            {
                DescriptState=69,
                InChars=new string[]{"&", ""},

                NextStates = new int[]{(int) LexemClass.Operations, Error_State}
            },
            new State
            {
                DescriptState=70,
                InChars=new string[]{"|", ""},

                NextStates = new int[]{(int) LexemClass.Operations, Error_State}
            },
            new State
            {
                DescriptState=71,
                InChars=new string[]{"0-9", ".", ""},

                NextStates = new int[]{71, 72, (int) LexemClass.Literal}
            },

             new State
            {
                DescriptState=72,
                InChars=new string[]{"0-9", ""},

                NextStates = new int[]{72, (int) LexemClass.Literal}
            },
              new State
            {
                DescriptState=73,
                InChars=new string[]{""},

                NextStates = new int[]{(int) LexemClass.Separator}
            },

              new State
            {
                DescriptState=74,
                InChars=new string[]{""},

                NextStates = new int[]{(int) LexemClass.Literal}
            },
              new State
            {
                DescriptState=75,
                InChars=new string[]{""},

                NextStates = new int[]{(int) LexemClass.Operations}
            },
        };
       
        public int CurrentState { get; private set; }

        public FSM()
        {
            CurrentState = 0; // 0 state
        }

        // return current state - Error, InProcess or Class Of Lexem
        public int Handle(char symbol)
        {
            //check symbols inchars and do jump to next state
            string[] inChars = listStates[CurrentState].InChars;
            int[] nextStates = listStates[CurrentState].NextStates;
            for(int i=0; i<inChars.Length; i++)
            {
                // str contains 1 symbol
                if(inChars[i].Length == 1)
                {
                    // change current State
                    if (inChars[i].Equals(symbol.ToString()))
                    {
                        CurrentState = nextStates[i];
                        break;
                    }                        
                }
                else if(inChars[i].Length > 1) // diapazon
                {
                    if(Regex.IsMatch(symbol.ToString(), "[" + inChars[i] + "]"))
                    {
                        {
                            CurrentState = nextStates[i];
                            break;
                        }
                    }
                } 
                else if(inChars[i].Equals("")) // other symbols
                {
                    CurrentState = nextStates[i];
                    break;
                }
            }

            int resState = 0;
            // check state on Error and Lexem
            if (IsErrorOrFoundLexemState(CurrentState))
            {
                resState = CurrentState;
                Reset();
            }
            else if(CurrentState == 0)
            {
                resState = Skip_Chars_State;
            }            
            else resState = Analys_Lexem_State;

            return resState;
        }

        public void Reset()
        {
            CurrentState = 0;
        }

        private bool IsErrorOrFoundLexemState(int currentState)
        {
            if (currentState == Error_State || currentState == (int)LexemClass.Identifier)
                return true;
            if (currentState == (int)LexemClass.KeyWord || currentState == (int)LexemClass.Literal)
                return true;
            if (currentState == (int)LexemClass.Operations || currentState == (int)LexemClass.Separator)
                return true;
            return false;
        }       
    }
}
