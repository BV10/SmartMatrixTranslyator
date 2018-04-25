using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicalTools
{
    public struct PositionInMultiStr
    {
        public int numberLine;
        public int startPos;
        public int endPos;

        public override string ToString()
        {
            return "Line: " + numberLine + ". Position in line: [" + startPos + "," + endPos + ").";
        }
    }
}
