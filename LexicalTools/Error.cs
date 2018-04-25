namespace LexicalTools
{
    public class Error
    {
        string description;
        int numberLine;
        int startPos;
        int endPos;

        public Error(string description)
        {
            this.Description = description;
        }

        public Error(string description, int numberLine, int startPos, int endPos) : this(description)
        {
            NumberLine = numberLine;
            StartPos = startPos;
            EndPos = endPos;
        }

        public string Description { get => description; set => description = value; }
        public int NumberLine { get => numberLine; set => numberLine = value; }        
        public int StartPos { get => startPos; set => startPos = value; }
        public int EndPos { get => endPos; set => endPos = value; }

        public override string ToString()
        {
            return "Decription: " + Description + ". Line: " + NumberLine + ". " + 
                "Position in line: [" + startPos + ", " + endPos + "].";
        }
    }
}