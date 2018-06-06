namespace SyntacticTools
{
    public class Four
    {
        public string Operation { get; set; }
        public string FirstOperand { get; set; }
        public string SecondOperand { get; set; }
        public string Number { get; set; }

        public override string ToString()
        {
            return Operation + ",  " + FirstOperand + ",  " + SecondOperand + ",  " + Number + ";";
        }
    }
}