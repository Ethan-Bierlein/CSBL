using System;
using System.Collections.Generic;
using CSBL.Reporting;
using CSBL.Tokenization;

namespace CSBL.Transformation
{
    /// <summary>
    /// This class is responsible for taking an input list of tokens and transforming them
    /// into a set of proper, interpretable tokens.
    /// </summary>
    public class Transformer
    {
        public List<Token> InputTokens { get; private set; }
        public List<TransformedToken> OutputTokens { get; private set; }

        /// <summary>
        /// Constructor for the Transformer class.
        /// </summary>
        /// <param name="inputTokens">The tokens to be transformed, provided by the tokenizer.</param>
        public Transformer(List<Token> inputTokens)
        {
            this.InputTokens = inputTokens;
            this.OutputTokens = new List<TransformedToken>() { };
        }

        /// <summary>
        /// Add a token to the list of transformed token.
        /// </summary>
        /// <param name="transformedTokens">A reference to the current list of transformed tokens.</param>
        /// <param name="tokenPosition">The position of the token to add.</param>
        /// <param name="tokenType">The type of the token to add.</param>
        /// <param name="tokenData">The data of the token to add.</param>
        private void AddToken(
            ref List<TransformedToken> transformedTokens,
            TokenPosition tokenPosition,
            TransformedTokenType tokenType,
            params dynamic[] tokenData
        )
        {
            transformedTokens.Add(
                new TransformedToken(
                    tokenPosition,
                    tokenType,
                    tokenData
                )
            );
        }

        /// <summary>
        /// Add a token to the list of transformed tokens and increment the 
        /// current token index.
        /// </summary>
        /// <param name="transformedTokens">A reference to the current list of transformed tokens.</param>
        /// <param name="currentTokenIndex">A reference to the current token index.</param>
        /// <param name="tokenPosition">The position of the token to add.</param>
        /// <param name="tokenType">The type of the token to add.</param>
        /// <param name="tokenData">The data of the token to add.</param>
        private void AddTokenAndIncrement(
            ref List<TransformedToken> transformedTokens,
            ref int currentTokenIndex,
            TokenPosition tokenPosition, 
            TransformedTokenType tokenType, 
            params dynamic[] tokenData
        )
        {
            currentTokenIndex++;
            transformedTokens.Add(
                new TransformedToken(
                    tokenPosition,
                    tokenType,
                    tokenData
                )
            );
        }

        /// <summary>
        /// Transform the list of input tokens into a list of transformed tokens.
        /// </summary>
        /// <returns>A list of transformed tokens.</returns>
        public List<TransformedToken> Transform()
        {
            List<TransformedToken> transformedTokens = new List<TransformedToken>() { };
            int currentTokenIndex = 0;
            bool errorEncountered = false;

            while(currentTokenIndex < this.InputTokens.Count)
            {
                switch(this.InputTokens[currentTokenIndex].Type)
                {
                    case TokenType.CodeBlockOpen:
                        this.AddTokenAndIncrement(
                            ref transformedTokens, 
                            ref currentTokenIndex, 
                            this.InputTokens[currentTokenIndex].Position, 
                            TransformedTokenType.CodeBlockOpen
                        );
                        break;

                    case TokenType.CodeBlockClose:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.CodeBlockClose
                        );
                        break;

                    case TokenType.Name:
                        if(
                            this.InputTokens[currentTokenIndex + 1].Type == TokenType.TypeNameSeparator && 
                            this.InputTokens[currentTokenIndex + 2].Type == TokenType.Type &&
                            currentTokenIndex <= this.InputTokens.Count - 3
                        )
                        {
                            this.AddToken(
                                ref transformedTokens,
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.TypedName,
                                this.InputTokens[currentTokenIndex].Value.Trim('@'),
                                this.InputTokens[currentTokenIndex + 2].Value.Trim('<').Trim('>')
                            );
                            currentTokenIndex += 3;
                        }
                        else
                        {
                            this.AddTokenAndIncrement(
                                ref transformedTokens,
                                ref currentTokenIndex,
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.UntypedName,
                                this.InputTokens[currentTokenIndex].Value.Trim('@')
                            );
                        }
                        break;

                    case TokenType.Type:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.Type, 
                            this.InputTokens[currentTokenIndex].Value.Trim('<').Trim('>')
                        );
                        break;

                    case TokenType.TypeNameSeparator:
                        errorEncountered = true;
                        Errors.MisplacedTypeSeparator.Report(
                            this.InputTokens[currentTokenIndex].Position.Line,
                            this.InputTokens[currentTokenIndex].Position.Column
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.BoolLiteral:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.Bool,
                            this.InputTokens[currentTokenIndex].Value == "true" ? true : false
                        );
                        break;

                    case TokenType.NumberLiteral:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.Number, 
                            Convert.ToSingle(this.InputTokens[currentTokenIndex].Value)
                        );
                        break;

                    case TokenType.StringLiteral:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.String,
                            this.InputTokens[currentTokenIndex].Value.Trim('"').Trim('\'')
                        );
                        break;

                    case TokenType.ArrayOpenLiteral:
                        List<TransformedToken> arrayLiteralContents = new List<TransformedToken>() { };
                        TokenPosition arrayStartPosition = this.InputTokens[currentTokenIndex].Position;
                        currentTokenIndex++;

                        while(this.InputTokens[currentTokenIndex].Type != TokenType.ArrayCloseLiteral)
                        {
                            if(
                                currentTokenIndex == this.InputTokens.Count - 1 && 
                                this.InputTokens[currentTokenIndex].Type != TokenType.ArrayCloseLiteral
                            )
                            {
                                errorEncountered = true;
                                Errors.MissingArrayEnd.Report(
                                    arrayStartPosition.Line,
                                    arrayStartPosition.Column
                                );
                                break;
                            }

                            if(
                                this.InputTokens[currentTokenIndex].Type == TokenType.Type ||
                                this.InputTokens[currentTokenIndex].Type == TokenType.Name ||
                                this.InputTokens[currentTokenIndex].Type == TokenType.BoolLiteral ||
                                this.InputTokens[currentTokenIndex].Type == TokenType.NumberLiteral ||
                                this.InputTokens[currentTokenIndex].Type == TokenType.StringLiteral
                            )
                            {
                                switch(this.InputTokens[currentTokenIndex].Type)
                                {
                                    case TokenType.Type:
                                        arrayLiteralContents.Add(
                                            new TransformedToken(
                                                this.InputTokens[currentTokenIndex].Position,
                                                TransformedTokenType.Type,
                                                this.InputTokens[currentTokenIndex].Value.Trim('<').Trim('>')
                                            )
                                        );
                                        currentTokenIndex++;
                                        break;

                                    case TokenType.Name:
                                        arrayLiteralContents.Add(
                                            new TransformedToken(
                                                this.InputTokens[currentTokenIndex].Position,
                                                TransformedTokenType.UntypedName,
                                                this.InputTokens[currentTokenIndex].Value.Trim('@')
                                            )
                                        );
                                        currentTokenIndex++;
                                        break;

                                    case TokenType.BoolLiteral:
                                        arrayLiteralContents.Add(
                                            new TransformedToken(
                                                this.InputTokens[currentTokenIndex].Position,
                                                TransformedTokenType.Bool,
                                                this.InputTokens[currentTokenIndex].Value == "true" ? true : false
                                            )
                                        );
                                        currentTokenIndex++;
                                        break;

                                    case TokenType.NumberLiteral:
                                        arrayLiteralContents.Add(
                                            new TransformedToken(
                                                this.InputTokens[currentTokenIndex].Position,
                                                TransformedTokenType.Number,
                                                Convert.ToSingle(this.InputTokens[currentTokenIndex].Value)
                                            )
                                        );
                                        currentTokenIndex++;
                                        break;

                                    case TokenType.StringLiteral:
                                        arrayLiteralContents.Add(
                                            new TransformedToken(
                                                this.InputTokens[currentTokenIndex].Position,
                                                TransformedTokenType.String,
                                                this.InputTokens[currentTokenIndex].Value.Trim('"').Trim('\'')
                                            )
                                        );
                                        currentTokenIndex++;
                                        break;
                                }
                            }
                            else
                            {
                                errorEncountered = true;
                                Errors.UnknownToken.Report(
                                    this.InputTokens[currentTokenIndex].Value,
                                    this.InputTokens[currentTokenIndex].Position.Line,
                                    this.InputTokens[currentTokenIndex].Position.Column
                                );
                            }
                        }

                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            arrayStartPosition,
                            TransformedTokenType.Array,
                            arrayLiteralContents
                        );
                        break;

                    case TokenType.ArrayCloseLiteral:
                        errorEncountered = true;
                        Errors.MisplacedArrayEnd.Report(
                            this.InputTokens[currentTokenIndex].Position.Line,
                            this.InputTokens[currentTokenIndex].Position.Column
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.CallOperator:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.CallOperator, 
                            this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                        );
                        break;

                    case TokenType.CallFunction:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.CallFunction,
                            this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                        );
                        break;

                    case TokenType.CallCustomFunction:
                        this.AddTokenAndIncrement(
                            ref transformedTokens,
                            ref currentTokenIndex,
                            this.InputTokens[currentTokenIndex].Position,
                            TransformedTokenType.CallCustomFunction,
                            this.InputTokens[currentTokenIndex].Value.Trim('{').Trim('}')
                        );
                        break;

                    default:
                        errorEncountered = true;
                        Errors.UnknownToken.Report(
                            this.InputTokens[currentTokenIndex].Value,
                            this.InputTokens[currentTokenIndex].Position.Line,
                            this.InputTokens[currentTokenIndex].Position.Column
                        );
                        currentTokenIndex++;
                        break;
                }
            }

            if(errorEncountered)
            {
                return null;
            }

            this.OutputTokens = transformedTokens;
            return transformedTokens;
        }
    }
}
