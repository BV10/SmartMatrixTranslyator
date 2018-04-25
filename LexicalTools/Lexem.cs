using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalTools
{
    public class Lexem
    {
        public string Lex { get; set; }
        public LexemClass LexClass { get; set; }
        public int NumberInClass { get; set; }
        public PositionInMultiStr PositionInMultiStr { get; set; }

        public override string ToString()
        {
            string className = null;
            switch (LexClass)
            {
                case LexemClass.KeyWord:
                    className = "Key Word";
                    break;
                case LexemClass.Identifier:
                    className = "Indentifier";
                    break;
                case LexemClass.Operations:
                    className = "Operation";
                    break;
                case LexemClass.Separator:
                    className = "Separator";
                    break;
                case LexemClass.Literal:
                    className = "Literal";
                    break;
            }

            return Lex + " --- " + className + "(" + NumberInClass + "). " + PositionInMultiStr;
        }
    }
}
