using System;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This class represents the position of the token within the provided
    /// source string.
    /// </summary>
    public class TokenPosition
    {
        public int Line { get; private set; }
        public int Column { get; private set; }

        /// <summary>
        /// Constructor for the TokenPosition class.
        /// </summary>
        /// <param name="line">The line of the token.</param>
        /// <param name="column">The column of the token.</param>
        public TokenPosition(int line, int column)
        {
            this.Line = line;
            this.Column = column;
        }
    }
}
