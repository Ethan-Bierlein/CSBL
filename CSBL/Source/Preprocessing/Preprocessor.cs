using System;
using System.IO;
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
        public string BaseIncludePath { get; private set; }
        public string InputString { get; private set; }
        public string OutputString { get; private set; }
        public Regex CommentRegex { get; private set; }
        public List<string> Options { get; private set; }
        public PreprocessorTokenDefinition[] InputTokens { get; private set; }
        public List<PreprocessorToken> OutputTokens { get; private set; }
        public List<string> OutputOptions { get; private set; }

        /// <summary>
        /// Constructor for the Preprocessor class.
        /// </summary>
        /// <param name="baseIncludePath">The base include path for all #use tokens.</param>
        /// <param name="inputString">The input string provided.</param>
        /// <param name="commentRegex">The regex for matching comments.</param>
        /// <param name="inputTokens">The list of preprocessor token definitions.</param>
        public Preprocessor(string baseIncludePath, string inputString, Regex commentRegex, List<string> options, params PreprocessorTokenDefinition[] inputTokens)
        {
            this.BaseIncludePath = baseIncludePath;
            this.InputString = inputString + " \n";
            this.OutputString = "";
            this.CommentRegex = commentRegex;
            this.Options = options;
            this.InputTokens = inputTokens;
            this.OutputTokens = new List<PreprocessorToken>() { };
            this.OutputOptions = new List<string>() { };
        }

        /// <summary>
        /// Replace all the comments within the input string with newline characters.
        /// </summary>
        private void ReplaceComments()
        {
            this.InputString = this.CommentRegex.Replace(this.InputString, "\n");
        }

        /// <summary>
        /// Generate a list of preprocessor tokens.
        /// </summary>
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

        /// <summary>
        /// Generate an output string for the current set of preprocessor tokens.
        /// </summary>
        /// <param name="definedNames">The </param>
        /// <param name="includedFiles"></param>
        /// <param name="numberOfOutputTokens"></param>
        /// <param name="numberOfInsertedChars"></param>
        /// <returns></returns>
        public string GenerateOutput(ref List<string> definedNames, ref List<string> includedFiles, ref int numberOfOutputTokens, ref int numberOfInsertedChars)
        {
            this.ReplaceComments();
            this.GenerateTokens();
            Regex splitRegex = new Regex("\\s+");
            string outputString = this.InputString;
            bool errorEncountered = false;

            foreach(PreprocessorToken token in this.OutputTokens)
            {
                outputString = outputString
                    .Remove(token.CharacterPosition, token.Data[0].Length)
                    .Insert(token.CharacterPosition, new string(' ', token.Data[0].Length));
            }

            foreach(PreprocessorToken token in this.OutputTokens)
            {
                if(token.Type == PreprocessorTokenType.Option)
                {
                    List<string> splitOptionToken = splitRegex.Split(token.Data[0], 2).ToList();
                    splitOptionToken.RemoveAll(str => str == string.Empty);

                    if(this.Options.Contains(splitOptionToken[1]))
                    {
                        this.OutputOptions.Add(splitOptionToken[1]);
                        outputString = outputString
                            .Remove(token.CharacterPosition, token.Data[0].Length)
                            .Insert(token.CharacterPosition, new string(' ', token.Data[0].Length));
                    }
                    else
                    {
                        Errors.InvalidPreprocessorOption.Report(splitOptionToken[1]);
                        errorEncountered = true;
                    }
                }
            }

            foreach(PreprocessorToken token in this.OutputTokens)
            {
                switch(token.Type)
                {
                    case PreprocessorTokenType.Import:
                        List<string> splitImportToken = splitRegex.Split(token.Data[0], 2).ToList();
                        string path = string.Format("{0}/{1}", this.BaseIncludePath, splitImportToken[1].Trim('"'));
                        splitImportToken.RemoveAll(str => str == string.Empty);
                        if(!includedFiles.Contains(path))
                        {
                            try
                            {
                                string text = File.ReadAllText(path);
                                outputString = outputString.Insert(token.CharacterPosition + numberOfInsertedChars, text);
                                includedFiles.Add(path);
                                numberOfInsertedChars += text.Length;
                            }
                            catch(Exception)
                            {
                                Errors.ErrorOpeningFile.Report(path);
                                errorEncountered = true;
                            }
                        }
                        else
                        {
                            if(this.OutputOptions.Contains("ENABLE_REIMPORT_ERROR"))
                            {
                                Errors.RedefinedPreprocessorImport.Report(splitImportToken[1]);
                                errorEncountered = true;
                            }
                        }
                        break;

                    case PreprocessorTokenType.Option:
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
