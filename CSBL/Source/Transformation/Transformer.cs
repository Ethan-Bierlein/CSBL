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
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position, 
                                TransformedTokenType.CodeBlockOpen
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.CodeBlockClose:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.CodeBlockClose
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.Name:
                        if(
                            this.InputTokens[currentTokenIndex + 1].Type == TokenType.TypeNameSeparator && 
                            this.InputTokens[currentTokenIndex + 2].Type == TokenType.Type &&
                            currentTokenIndex <= this.InputTokens.Count - 3
                        )
                        {
                            transformedTokens.Add(
                                new TransformedToken(
                                    this.InputTokens[currentTokenIndex].Position,
                                    TransformedTokenType.TypedName,
                                    this.InputTokens[currentTokenIndex].Value.Trim('@'),
                                    this.InputTokens[currentTokenIndex + 2].Value.Trim('<').Trim('>')
                                )
                            );
                            currentTokenIndex += 3;
                        }
                        else
                        {
                            transformedTokens.Add(
                                new TransformedToken(
                                    this.InputTokens[currentTokenIndex].Position,
                                    TransformedTokenType.UntypedName,
                                    this.InputTokens[currentTokenIndex].Value.Trim('@')
                                )
                            );
                            currentTokenIndex++;
                        }
                        break;

                    case TokenType.Type:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.Type, 
                                this.InputTokens[currentTokenIndex].Value.Trim('<').Trim('>')
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.TypeNameSeparator:
                        errorEncountered = true;
                        Errors.MisplacedTypeSeparator.Report(
                            this.InputTokens[currentTokenIndex].Position.Line,
                            this.InputTokens[currentTokenIndex].Position.Column
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.NumberLiteral:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.Number, 
                                Convert.ToSingle(this.InputTokens[currentTokenIndex].Value)
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.StringLiteral:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.String,
                                this.InputTokens[currentTokenIndex].Value.Trim('"').Trim('\'')
                            )
                        );
                        currentTokenIndex++;
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

                        transformedTokens.Add(
                            new TransformedToken(
                                arrayStartPosition,
                                TransformedTokenType.Array,
                                arrayLiteralContents
                            )
                        );

                        currentTokenIndex++;
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
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.CallOperator, 
                                this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.CallFunction:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.CallFunction,
                                this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                            )
                        );
                        currentTokenIndex++;
                        break;

                    case TokenType.CallCustomFunction:
                        transformedTokens.Add(
                            new TransformedToken(
                                this.InputTokens[currentTokenIndex].Position,
                                TransformedTokenType.CallCustomFunction,
                                this.InputTokens[currentTokenIndex].Value.Trim('{').Trim('}')
                            )
                        );
                        currentTokenIndex++;
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
