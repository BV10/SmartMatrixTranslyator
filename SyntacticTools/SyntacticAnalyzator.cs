using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LexicalTools;
using System.Text.RegularExpressions;

namespace SyntacticTools
{
    public class SyntacticAnalyzator
    {
        private List<Lexem> lexems; // list lexems 
        private FiniteStateMachine finiteStateMachin; // finite state machine     

        public List<Lexem> Lexems { get => lexems; set => lexems = value; }
        public FiniteStateMachine FiniteStateMachin { get => finiteStateMachin; private set => finiteStateMachin = value; }
        public Error ErrorSyntax { get; private set; }

        public SyntacticAnalyzator(List<Lexem> lexems, FiniteStateMachine finiteStateMachin)
        {
            Lexems = lexems;
            FiniteStateMachin = finiteStateMachin;
            FiniteStateMachin.BuildMachine();           
        }

        public bool SyntaxAnalyzeAndBuildFours()
        {
            StateMachine stateMachine;
            foreach (var lexem in Lexems)
            {
                stateMachine = FiniteStateMachin.Handle(lexem);
                if (stateMachine == StateMachine.ErrorSyntax || stateMachine == StateMachine.ErrorSemantic)
                {
                    ErrorSyntax = FiniteStateMachin.ErrorSyntax;
                    return false;
                }
                else if(stateMachine == StateMachine.EndProgram)
                {
                    return true;
                }            
            }
            return true;
        }
    }
}
