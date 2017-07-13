using System;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This class represents the position of the token within the provided
    /// source string.
    /// </summary>
    public class TokenPosition
    {
        public string File { get; set; }
        public int Line { get; set; }
        public int RawLine { get; set; }
        public int Column { get; set; }

        /// <summary>
        /// Constructor for the TokenPosition class.
        /// </summary>
        /// <param name="file">The file that the token is contained within.</param>
        /// <param name="line">The line of the token.</param>
        /// <param name="rawLine">The 'raw' line number of the token in the preprocessed source string.</param>
        /// <param name="column">The column of the token.</param>
        public TokenPosition(string file, int line, int rawLine, int column)
        {
            this.File = file;
            this.Line = line;
            this.RawLine = rawLine;
            this.Column = column;
        }
    }
}
