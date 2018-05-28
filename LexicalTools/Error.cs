namespace LexicalTools
{
    public class Error
    {
        string description;
        PositionInMultiStr positionInMultiStr;

        public Error(string description)
        {
            this.Description = description;
        }

        public Error(string description, PositionInMultiStr positionInMulti) : this(description)
        {
            this.PositionInMultiStr = positionInMulti;
        }

        public string Description { get => description; set => description = value; }
        public PositionInMultiStr PositionInMultiStr { get => positionInMultiStr; set => positionInMultiStr = value; }

        public override string ToString()
        {
            return "Decription: " + Description + ". Line: " + PositionInMultiStr.numberLine + ". " + 
                "Position in line: [" + PositionInMultiStr.startPos + ", " + PositionInMultiStr.endPos + "].";
        }
    }
}