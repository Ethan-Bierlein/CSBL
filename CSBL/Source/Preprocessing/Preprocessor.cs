﻿using System;
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

        private void GenerateTokens()
        {
            foreach(PreprocessorTokenDefinition tokenRegex in this.InputTokens)
            {
                MatchCollection tokenMatchCollection = tokenRegex.Matches(this.InputString);
                List<PreprocessorToken> generatedTokens = tokenMatchCollection
                    .Cast<Match>()
                    .Select(match => new PreprocessorToken(tokenRegex.Type, match.Index, match.Value))
                    .OrderBy(token => token.CharacterPosition)
                    .ToList();
                
                foreach(PreprocessorToken token in generatedTokens)
                {
                    this.OutputTokens.Add(token);
                }
            }
        }

        public string GenerateOutput(ref List<string> definedNames, ref List<string> includedFiles, ref int numberOfOutputTokens)
        {
            Regex splitRegex = new Regex("\\s+");
            string outputString = this.InputString;
            bool errorEncountered = false;
            this.GenerateTokens();

            foreach(PreprocessorToken token in this.OutputTokens)
            {
                outputString = outputString
                    .Remove(token.CharacterPosition, token.Data[0].Length)
                    .Insert(token.CharacterPosition, new string(' ', token.Data[0].Length));
            }

            foreach(PreprocessorToken token in this.OutputTokens)
            {
                switch(token.Type)
                {
                    case PreprocessorTokenType.Define:
                        List<string> splitDefineToken = splitRegex.Split(token.Data[0], 3).ToList();
                        splitDefineToken.RemoveAll(str => str == string.Empty);
                        if(!definedNames.Contains(splitDefineToken[1]))
                        {
                            outputString = outputString.Replace(splitDefineToken[1], splitDefineToken[2]);
                            definedNames.Add(splitDefineToken[1]);
                        }
                        else
                        {
                            Errors.RedefinedPreprocessorName.Report(splitDefineToken[1]);
                            errorEncountered = true;
                        }
                        break;

                    case PreprocessorTokenType.Import:
                        List<string> splitImportToken = splitRegex.Split(token.Data[0], 2).ToList();
                        splitImportToken.RemoveAll(str => str == string.Empty);
                        if(!includedFiles.Contains(splitImportToken[1]))
                        {
                            includedFiles.Add(splitImportToken[1]);
                        }
                        else
                        {
                            Errors.RedefinedPreprocessorImport.Report(splitImportToken[1]);
                            errorEncountered = true;
                        }
                        break;

                    default:
                        Errors.InvalidPreprocessorToken.Report(
                            token.Data[0],
                            token.CharacterPosition
                        );
                        errorEncountered = true;
                        break;
                }
            }

            numberOfOutputTokens = this.OutputTokens.Count;
            this.OutputString = errorEncountered ? null : outputString;
            return this.OutputString;
        }
    }
}