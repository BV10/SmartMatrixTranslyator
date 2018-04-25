using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LexicalTools
{
    public class LexemAnalyzator
    {        

        private string code;        
        private FSM fsm;
        private StorageLexem storageLexem;
        private List<Error> listError = new List<Error>();

        public LexemAnalyzator(StorageLexem storageLexem, FSM fsm)
        {                      
            this.Fsm = fsm;
            this.StorageLexem = storageLexem;
        }
 
        public List<Error> ListError { get => listError; set => listError = value; }
        internal StorageLexem StorageLexem { get => StorageLexem1; set => StorageLexem1 = value; }
        public FSM Fsm { get => fsm; set => fsm = value; }
        internal StorageLexem StorageLexem1 { get => storageLexem; set => storageLexem = value; }
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                //reset fsm and lexem storage
                fsm.Reset();
                storageLexem.Reset();
                // reset errors
                ListError.Clear();
                code = value += "\n";
            }
        }       

        public List<Lexem> GetLexems()
        {
            int startPos = 0;
            int endPos = 0;
            char currentSymbol;
            for (int indexCh = 0; indexCh < Code.Length; indexCh++)
            {
                currentSymbol = Code[indexCh];

                int descrip = Fsm.Handle(currentSymbol);
                switch (descrip)
                {
                    case FSM.Analys_Lexem_State:
                        endPos++;// go along lexem
                        break;
                    case FSM.Error_State:
                        PositionInMultiStr errorPosition = GetStringPositionInMultiStr(Code, startPos, endPos);
                        ListError.Add(new Error("Error", errorPosition.numberLine, errorPosition.startPos, errorPosition.endPos));
                        //indexCh--; // rollback symbol                        
                        endPos++;
                        startPos = endPos;
                        break;
                    case FSM.Skip_Chars_State:
                        startPos++;
                        endPos++;
                        break;
                    default:// class of lexem
                        PositionInMultiStr lexemPosition = GetStringPositionInMultiStr(Code, startPos, endPos);
                        StorageLexem.Add(Code.Substring(startPos, endPos-startPos), (LexemClass)descrip, lexemPosition);
                        indexCh--;
                        startPos = endPos;
                        break;
                }                
            }
            fsm.Reset();

            return StorageLexem.ListLexem;
        }    
        
      
        private string AddSymbolForEndProgram(string code)
        {
            if (code[code.Length - 1] == '\n')
                return code;
            return code += "\n";
        }
        private PositionInMultiStr GetStringPositionInMultiStr(string code, int startPos, int endPos)
        {
            PositionInMultiStr stringPosition = new PositionInMultiStr();

            Regex reg = new Regex("\n");

            List<Match> matchesNewLine = new List<Match>(); // save all matches
            int countNewLine = 0;
            for (Match match = reg.Match(code, 0, endPos); match.Success; match = match.NextMatch())
            {
                countNewLine++;
                matchesNewLine.Add(match);
            }

            stringPosition.numberLine = countNewLine + 1; // add current line
            if (stringPosition.numberLine == 1)
            {
                stringPosition.startPos = startPos;
                stringPosition.endPos = endPos;
            }
            else
            {
                stringPosition.startPos = startPos - (matchesNewLine[matchesNewLine.Count - 1].Index + 2) + 1; // 
                stringPosition.endPos = endPos - (matchesNewLine[matchesNewLine.Count - 1].Index + 2) + 1;
            }
            return stringPosition;
        }
    }
}
