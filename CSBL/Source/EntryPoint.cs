using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Tokenization;
using CSBL.Transformation;
using CSBL.Interpretation;
using CSBL.Interpretation.Functions;
using CSBL.Interpretation.Functions.FunctionTypes.Math;
using CSBL.Interpretation.Functions.FunctionTypes.Boolean;
using CSBL.Interpretation.Functions.FunctionTypes.Comparison;

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
                -- This is a test comment. --
                ",
                new Regex("(\\-\\-(.|\n)*\\-\\-)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"),
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]*\")|('[^']*')")),
                new TokenDefinition(TokenType.NumberLiteral, new Regex("((-|\\+?)((\\d+\\.\\d+)|(\\.\\d+)|(\\d+\\.)|(\\d+)))(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CallFunction, new Regex("\\[[^\\[\\]]+\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"))
            );

            Console.ReadLine();
            List<Token> tokens = tokenizer.Tokenize();
            if(tokens != null)
            {
                Transformer transformer = new Transformer(tokens);
                List<TransformedToken> transformedTokens = transformer.Transform();
                if(transformedTokens != null)
                {
                    Interpreter interpreter = new Interpreter(
                        transformedTokens,
                        new Dictionary<string, FunctionBase>()
                        {
                            { "+", new FunctionADD() },
                            { "-", new FunctionSUB() },
                            { "*", new FunctionMUL() },
                            { "/", new FunctionDIV() },
                            { "**", new FunctionPOW() },

                            { "<", new FunctionLT() },
                            { "<=", new FunctionLTE() },
                            { ">", new FunctionGT() },
                            { ">=", new FunctionGTE() },
                            { "==", new FunctionEQ() },
                            { "!=", new FunctionNEQ() },

                            { "&&", new FunctionAND() },
                            { "||", new FunctionOR() }
                        }
                    );
                    interpreter.PreInterpret();
                    interpreter.Interpret();

                    Console.ReadLine();
                }
            }
        }
    }
}
