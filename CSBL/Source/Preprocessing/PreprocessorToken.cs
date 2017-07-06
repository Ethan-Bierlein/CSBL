using System;
using System.Text.RegularExpressions;
using CSBL.Reporting;

namespace CSBL.Preprocessing
{
    /// <summary>
    /// This class represents a preprocessor token.
    /// </summary>
    public class PreprocessorToken
    {
        public PreprocessorTokenType Type { get; set; }
        public int CharacterPosition { get; set; }
        public string[] Data { get; set; }

        /// <summary>
        /// Constructor for the PreprocessorToken class.
        /// </summary>
        /// <param name="type">The type of the preprocessor token.</param>
        /// <param name="characterPosition">The position of the preprocessor token.</param>
        /// <param name="data">The data contained within the preprocessor token.</param>
        public PreprocessorToken(PreprocessorTokenType type, int characterPosition, params string[] data)
        {
            this.Type = type;
            this.CharacterPosition = characterPosition;
            this.Data = data;
        }
    }
}
