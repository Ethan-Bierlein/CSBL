using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Tokenization;
using CSBL.Transformation;

namespace CSBL
{
    /// <summary>
    /// This class is the main entry point for the CSBL language.
    /// </summary>
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer(
                @"
                (
                    'na' @na-count {build-repeated-string} ' ' 'Batman!' {concatenate} [print]
                ) @batman @na-count::<number> [fn]

                (
                    @array ' ' {join}
                ) @join-array @array::<array> [fn]

                [[ 'a' 'b' 'c' 'd' 'e' ]] {join-array}
                ",
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex(@"\(")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex(@"\)")),

                new TokenDefinition(TokenType.Type, new Regex(@"<[a-zA-Z0-9_\-]+>")),
                new TokenDefinition(TokenType.Name, new Regex(@"@[a-zA-Z0-9_\-]+")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex(@"::")),

                new TokenDefinition(TokenType.NumberLiteral, new Regex(@"\b-{0,1}[0-9]+(\.[0-9]*){0,1}\b")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]+\")|('[^']+')")),
                new TokenDefinition(TokenType.ArrayOpenLiteral, new Regex(@"\[\[")),
                new TokenDefinition(TokenType.ArrayCloseLiteral, new Regex(@"\]\]")),

                new TokenDefinition(TokenType.CallOperator, new Regex(@"\[(\<\<|\<\=|\<|\>\>|\>\=|\>|\=\=|\!\=|\&\&|\|\||\||\^|\&|\~|\-\>|\+|\-|\*|\/|)\]")),
                new TokenDefinition(TokenType.CallFunction, new Regex(@"\[[a-zA-Z0-9_\-]+\]")),
                new TokenDefinition(TokenType.CallCustomFunction, new Regex(@"{[a-zA-Z0-9_\-]+}"))
            );

            Console.ReadLine();
            List<Token> tokens = tokenizer.Tokenize();
            if(tokens != null)
            {
                Console.WriteLine(":: TOKENS ::");
                foreach(Token token in tokens)
                {
                    Console.Write(
                        "[TOKEN ({0},{1}) -> {2}] = '{3}'", 
                        token.Position.Line, 
                        token.Position.Column, 
                        token.Type, token.Value
                    );
                    Console.ReadLine();
                }

                Transformer transformer = new Transformer(tokens);
                List<TransformedToken> transformedTokens = transformer.Transform();

                if(transformedTokens != null)
                {
                    Console.WriteLine("\n:: TRANSFORMED TOKENS ::");
                    foreach(TransformedToken transformedToken in transformedTokens)
                    {
                        Console.Write(
                            "[TOKEN ({0},{1}) -> {2}] = ('{3}')",
                            transformedToken.Position.Line,
                            transformedToken.Position.Column,
                            transformedToken.Type,
                            string.Join(
                                "' , '",
                                transformedToken.Data.Length > 0
                                    ? transformedToken.Data[0].GetType() == typeof(List<TransformedToken>) 
                                        ? new string[] { string.Join("' , '", (transformedToken.Data[0] as List<TransformedToken>).Select(t => t.Data[0])) }
                                        : transformedToken.Data
                                    : new string[] { }
                            )
                        );
                        Console.ReadLine();
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
