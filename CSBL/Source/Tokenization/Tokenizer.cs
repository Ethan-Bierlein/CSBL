﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This class is responsible for taking an input string and transforming it
    /// into a set of output tokens.
    /// </summary>
    public class Tokenizer
    {
        public string InputString { get; private set; }
        public List<Token> OutputTokens { get; private set; }
        public List<TokenDefinition> TokenDefinitions { get; private set; }

        /// <summary>
        /// Constructor for the Tokenizer class.
        /// </summary>
        /// <param name="inputString">The input string to tokenize.</param>
        /// <param name="tokenDefinitions">The array of all token definitions.</param>
        public Tokenizer(string inputString, params TokenDefinition[] tokenDefinitions)
        {
            this.InputString = inputString;
            this.OutputTokens = new List<Token>() { };
            this.TokenDefinitions = tokenDefinitions.ToList();
        }

        /// <summary>
        /// Tokenize the provided input string into a new list of output
        /// tokens. The tokenizer works in three stages. 
        /// 
        /// 1. In stage one, the list of token definitions is iterated over, 
        ///    and a sorted dictionary of regex matches and match indices is 
        ///    built. 
        /// 
        /// 2. In the second stage, the sorted dictionary is sorted based on 
        ///    its keys, which represent match indices, and a list of tokens 
        ///    is built.  
        /// 
        /// 3. Finally, in stage three any invalid tokens within the string 
        ///    are detected and reported to the user.
        /// </summary>
        /// <returns>A list of output tokens.</returns>
        public List<Token> Tokenize()
        {
            SortedDictionary<int, Token> indexedTokens = new SortedDictionary<int, Token>() { };
            List<Token> outputTokens = new List<Token>() { };

            foreach(TokenDefinition tokenDefinition in this.TokenDefinitions)
            {
                MatchCollection matchCollection = tokenDefinition.Matches(this.InputString);
                List<Match> matchList = matchCollection.Cast<Match>().Select(match => match).ToList();

                foreach(Match match in matchList)
                {
                    if(!indexedTokens.ContainsKey(match.Index))
                    {
                        indexedTokens.Add(match.Index, new Token(tokenDefinition.Type, match.Value));
                    }
                }
            }

            indexedTokens.OrderBy(value => value.Key);
            foreach(KeyValuePair<int, Token> keyValuePair in indexedTokens)
            {
                outputTokens.Add(keyValuePair.Value);
            }

            List<string> lineSplitInputString = this.InputString.Split('\n').ToList();
            for(int lineIndex = 0; lineIndex < lineSplitInputString.Count; lineIndex++)
            {
                string lineCopy = lineSplitInputString[lineIndex] + "  ";
                foreach(Token outputToken in outputTokens)
                {
                    lineCopy = lineCopy.Replace(outputToken.Value, new string(' ', outputToken.Value.Length));
                }

                int lineCopyIndex = 0;
                while(lineCopyIndex < lineCopy.Length - 1)
                {
                    if(!Regex.IsMatch(lineCopy[lineCopyIndex].ToString(), "\\s"))
                    {
                        string invalidToken = "";
                        int lineCopyIndexIncrements = 0;
                        while(!Regex.IsMatch(lineCopy[lineCopyIndex].ToString(), "\\s"))
                        {
                            invalidToken += lineCopy[lineCopyIndex];
                            lineCopyIndex++;
                            lineCopyIndexIncrements++;
                        }

                        Console.WriteLine("Unknown token '{0}' at line {1}, at column {2}.", invalidToken, lineIndex, lineCopyIndex - lineCopyIndexIncrements);
                        continue;
                    }

                    lineCopyIndex++;
                }
            }

            this.OutputTokens = outputTokens; 
            return this.OutputTokens;
        }
    }
}
