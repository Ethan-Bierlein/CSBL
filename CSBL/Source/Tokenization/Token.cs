using System;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This class represents a token.
    /// </summary>
    public class Token
    {
        public TokenPosition Position { get; set; }
        public TokenType Type { get; set; }
        public string Value { get; set; }

        /// <summary>
        /// Constructor for the Token class.
        /// </summary>
        /// <param name="position">The position of the token.</param>
        /// <param name="type">The type of the token.</param>
        /// <param name="value">The value of the token.</param>
        public Token(TokenPosition position, TokenType type, string value)
        {
            this.Position = position;
            this.Type = type;
            this.Value = value;
        }
    }
}
