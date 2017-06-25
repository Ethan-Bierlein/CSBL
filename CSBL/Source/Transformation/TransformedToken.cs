using System;
using System.Linq;
using CSBL.Tokenization;

namespace CSBL.Transformation
{
    /// <summary>
    /// This class contains information representing a transformed token.
    /// </summary>
    public class TransformedToken
    {
        public TokenPosition Position { get; set; }
        public TransformedTokenType Type { get; private set; }
        public dynamic[] Data { get; private set; }

        /// <summary>
        /// Constructor for the TransformedToken class.
        /// </summary>
        /// <param name="position">The token's position within the input string.</param>
        /// <param name="type">The type of the token.</param>
        /// <param name="data">The data stored within the token.</param>
        public TransformedToken(TokenPosition position, TransformedTokenType type, params dynamic[] data)
        {
            this.Position = position;
            this.Type = type;
            this.Data = data;
        }
    }
}
