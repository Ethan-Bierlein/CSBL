using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Reporting;

namespace CSBL.Preprocessing
{
    /// <summary>
    /// This class is responsible for detectign preprocessor tokens and regenerating
    /// a source file based off the given preprocessor tokens.
    /// </summary>
    public class Preprocessor
    {
        public string InputString { get; set; }
        public string OutputString { get; set; }
        public PreprocessorTokenDefinition[] InputTokens { get; set; }
        public List<PreprocessorToken> OutputTokens { get; set; }

        /// <summary>
        /// Constructor for the Preprocessor class.
        /// </summary>
        /// <param name="inputString">The input string provided.</param>
        /// <param name="inputTokens">The list of preprocessor token definitions.</param>
        public Preprocessor(string inputString, params PreprocessorTokenDefinition[] inputTokens)
        {
            this.InputString = inputString;
            this.OutputString = "";
            this.InputTokens = inputTokens;
            this.OutputTokens = new List<PreprocessorToken>() { };
        }

        public void GenerateTokens()
        {
            foreach(PreprocessorTokenDefinition tokenRegex in this.InputTokens)
            {
                MatchCollection tokenMatchCollection = tokenRegex.Matches(this.InputString);
                List<PreprocessorToken> generatedTokens = tokenMatchCollection
                    .Cast<Match>()
                    .Select(match => new PreprocessorToken(tokenRegex.Type, match.Index, match.Value))
                    .ToList();
                
                foreach(PreprocessorToken token in generatedTokens)
                {
                    this.OutputTokens.Add(token);
                }
            }
        }

        public string GenerateOutput()
        {

        }
    }
}
