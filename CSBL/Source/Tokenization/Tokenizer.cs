using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Reporting;

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
        /// tokens. The tokenizer works in three stages, matching, generating
        /// and validating.
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
                        int tokenLine = 1;
                        int tokenColumn = 1;

                        for(int i = 0; i < this.InputString.Length; i++)
                        {
                            if(i == match.Index)
                            {
                                break;
                            }

                            if(this.InputString[i] == '\n')
                            {
                                tokenLine++;
                                tokenColumn = 1;
                            }
                            else
                            {
                                tokenColumn++;
                            }
                        }

                        indexedTokens.Add(
                            match.Index, 
                            new Token(
                                new TokenPosition("", 0, tokenLine, tokenColumn), 
                                tokenDefinition.Type, 
                                match.Value
                            )
                        );
                    }
                }
            }

            indexedTokens.OrderBy(value => value.Key);
            Stack<int> currentLineCountStack = new Stack<int>() { };
            Stack<int> totalLineDriftStack = new Stack<int>() { };
            int currentLineValue = 1;
            int previousLineValue = 1;
            foreach(KeyValuePair<int, Token> keyValuePair in indexedTokens)
            {
                Token token = keyValuePair.Value;
                previousLineValue = currentLineValue;
                currentLineValue = indexedTokens[keyValuePair.Key].Position.RawLine;

                if(currentLineCountStack.Count > 0 && totalLineDriftStack.Count > 0)
                {
                    if(currentLineValue != previousLineValue)
                    {
                        int newTopLineCount = currentLineCountStack.Pop() + 1;
                        currentLineCountStack.Push(newTopLineCount);
                        int newTopTotalLineDrift = totalLineDriftStack.Pop() + (currentLineValue - previousLineValue - 1);
                        totalLineDriftStack.Push(newTopTotalLineDrift);
                    }

                    indexedTokens[keyValuePair.Key].Position.Line = currentLineCountStack.Peek() + totalLineDriftStack.Peek();
                }

                if(token.Type == TokenType.IncludedFileStartMarker)
                {
                    currentLineCountStack.Push(1);
                    totalLineDriftStack.Push(0);
                }
                else if(token.Type == TokenType.IncludedFileEndMarker)
                {
                    currentLineCountStack.Pop();
                    totalLineDriftStack.Pop();
                }
            }

            Stack<string> currentFileStack = new Stack<string>() { };
            foreach(KeyValuePair<int, Token> keyValuePair in indexedTokens)
            {
                if(keyValuePair.Value.Type == TokenType.IncludedFileStartMarker)
                {
                    currentFileStack.Push(keyValuePair.Value.Value.Trim('=').Trim('='));
                    outputTokens.Add(keyValuePair.Value);
                }
                else if(keyValuePair.Value.Type == TokenType.IncludedFileEndMarker)
                {
                    currentFileStack.Pop();
                    outputTokens.Add(keyValuePair.Value);
                }
                else
                {
                    Token tokenToAdd = keyValuePair.Value;
                    tokenToAdd.Position.File = currentFileStack.Peek();
                    outputTokens.Add(tokenToAdd);
                }
            }

            bool invalidTokenEncountered = false;
            Stack<string> currentLineFileStack = new Stack<string>() { };
            int currentNonRawLine = 1;
            List<string> lineSplitInputString = this.InputString.Split('\n').ToList();
            for(int lineIndex = 0; lineIndex < lineSplitInputString.Count; lineIndex++)
            {
                string lineCopy = lineSplitInputString[lineIndex] + "  ";
                foreach(Token outputToken in outputTokens)
                {
                    if(outputToken.Position.RawLine - 1 == lineIndex)
                    {
                        currentNonRawLine = outputToken.Position.Line;
                        lineCopy = lineCopy
                            .Remove(outputToken.Position.Column - 1, outputToken.Value.Length)
                            .Insert(outputToken.Position.Column - 1, new string(' ', outputToken.Value.Length));

                        if(outputToken.Type == TokenType.IncludedFileStartMarker)
                        {
                            currentLineFileStack.Push(outputToken.Value.Trim('=').Trim('='));
                        }
                        else if(outputToken.Type == TokenType.IncludedFileEndMarker)
                        {
                            currentLineFileStack.Pop();
                        }
                    }
                }

                int lineCopyIndex = 0;
                while(lineCopyIndex < lineCopy.Length - 1)
                {
                    if(!Regex.IsMatch(lineCopy[lineCopyIndex].ToString(), "\\s"))
                    {
                        string invalidToken = "";
                        int lineCopyIndexIncrements = 0;
                        invalidTokenEncountered = true;

                        while(!Regex.IsMatch(lineCopy[lineCopyIndex].ToString(), "\\s"))
                        {
                            invalidToken += lineCopy[lineCopyIndex];
                            lineCopyIndex++;
                            lineCopyIndexIncrements++;
                        }

                        Errors.InvalidToken.Report(currentLineFileStack.Peek(), currentNonRawLine, lineCopyIndex - lineCopyIndexIncrements, invalidToken);
                        continue;
                    }

                    lineCopyIndex++;
                }
            }

            if(invalidTokenEncountered)
            {
                return null;
            }

            this.OutputTokens = outputTokens; 
            return this.OutputTokens;
        }
    }
}
