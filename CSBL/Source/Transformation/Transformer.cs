using System;
using System.Collections.Generic;
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

            while(currentTokenIndex < this.InputTokens.Count)
            {
                switch(this.InputTokens[currentTokenIndex].Type)
                {
                    case TokenType.CodeBlockOpen:
                        transformedTokens.Add(new TransformedToken(TransformedTokenType.CodeBlockOpen));
                        break;

                    case TokenType.CodeBlockClose:
                        transformedTokens.Add(new TransformedToken(TransformedTokenType.CodeBlockClose));
                        break;

                    case TokenType.Name:
                        // TODO: Implement.
                        break;

                    case TokenType.Type:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.Type, 
                                this.InputTokens[currentTokenIndex].Value.Trim('<').Trim('>')
                            )
                        );
                        break;

                    case TokenType.TypeNameSeparator:
                        // TODO: Implement
                        break;

                    case TokenType.NumberLiteral:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.Number, 
                                Convert.ToSingle(this.InputTokens[currentTokenIndex].Value)
                            )
                        );
                        break;

                    case TokenType.StringLiteral:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.String, 
                                this.InputTokens[currentTokenIndex].Value.Trim('"')
                            )
                        );
                        break;

                    case TokenType.ArrayOpenLiteral:
                        // TODO: Implement.
                        break;

                    case TokenType.ArrayCloseLiteral:
                        // TODO: Implement.
                        break;

                    case TokenType.CallOperator:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.CallOperator, 
                                this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                            )
                        );
                        break;

                    case TokenType.CallFunction:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.CallFunction,
                                this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                            )
                        );
                        break;

                    case TokenType.CallCustomFunction:
                        transformedTokens.Add(
                            new TransformedToken(
                                TransformedTokenType.CallCustomFunction,
                                this.InputTokens[currentTokenIndex].Value.Trim('[').Trim(']')
                            )
                        );
                        break;

                    default:
                        // TODO: Throw error.
                        break;
                }
            }

            this.OutputTokens = transformedTokens;
            return transformedTokens;
        }
    }
}
