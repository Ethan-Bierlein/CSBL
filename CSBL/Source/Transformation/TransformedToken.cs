using System;
using System.Linq;

namespace CSBL.Transformation
{
    /// <summary>
    /// This class contains information representing a transformed token.
    /// </summary>
    public class TransformedToken
    {
        public TransformedTokenType Type { get; private set; }
        public dynamic[] Data { get; private set; }

        /// <summary>
        /// Constructor for the TransformedToken class.
        /// </summary>
        /// <param name="type">The type of the token.</param>
        /// <param name="data">The data stored within the token.</param>
        public TransformedToken(TransformedTokenType type, params dynamic[] data)
        {
            this.Type = type;
            this.Data = data;
        }
    }
}
