using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Tokenization;
using CSBL.Transformation;
using CSBL.Interpretation;
using CSBL.Interpretation.Functions;

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
                -- This function is used to generate the most famous line from the
                -- introduction song of the 1966 Batman TV show.
                -- @na-count - The number of 'na's to generate.
                -- (
                --    '[na]' @na-count {build-repeated-string} ' ' 'Batman!' {concatenate}
                -- ) @batman @na-count::<number> [fn]


                -- This function is used to join a provided array of values that may
                -- or may not be strings and join them together with a delimeter.
                -- @array     - The array to join together.
                -- @delimeter - The delimeter to join on.
                -- (
                --    @array {to-string} {map} @delimeter {join-to-string}
                -- ) @join-array @array::<array> @delimeter::<string> [fn]


                -- Call both the {batman} and the {join-array} function and print the
                -- results of both.
                -- 16 {batman} [print]
                -- [[ 'a' 'b' 'c' 'd' 'e' ]] {join-array} [print]

                1 [print]
                '2' [print]
                ",
                new Regex(@"\-\-.*"),
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex("\\((?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex("\\)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Type, new Regex("<[a-zA-Z0-9_\\-]+>(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Name, new Regex("@[a-zA-Z0-9_\\-]+(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex("::(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]+\")|('[^']+')")),
                new TokenDefinition(TokenType.NumberLiteral, new Regex("((-|\\+?)((\\d+\\.\\d+)|(\\.\\d+)|(\\d+\\.)|(\\d+)))(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.ArrayOpenLiteral, new Regex("\\[\\[(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.ArrayCloseLiteral, new Regex("\\]\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CallOperator, new Regex("\\[(\\<\\<|\\<\\=|\\<|\\>\\>|\\>\\=|\\>|\\=\\=|\\!\\=|\\&\\&|\\|\\||\\||\\^|\\&|\\~|\\-\\>|\\+|\\-|\\*|\\/|)\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CallFunction, new Regex("\\[[a-zA-Z0-9_\\-]+\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CallCustomFunction, new Regex("{[a-zA-Z0-9_\\-]+}(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"))
            );

            Console.ReadLine();
            List<Token> tokens = tokenizer.Tokenize();
            if(tokens != null)
            {
                /*
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
                */

                Transformer transformer = new Transformer(tokens);
                List<TransformedToken> transformedTokens = transformer.Transform();
                if(transformedTokens != null)
                {
                    /*
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
                    */

                    Interpreter interpreter = new Interpreter(
                        transformedTokens,
                        new Dictionary<string, FunctionBase>()
                        {
                            { "print", new FunctionPRINT() }
                        }
                    );
                    interpreter.Interpret();
                }
            }

            Console.ReadLine();
        }
    }
}
