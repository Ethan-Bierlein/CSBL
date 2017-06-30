using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Tokenization;
using CSBL.Transformation;
using CSBL.Interpretation;
using CSBL.Interpretation.Functions;
using CSBL.Interpretation.Operators;

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
                '' [print] 
                ':: ADD ::' [print]
                3 4 [+] [print]
                2 2 [+] [print]
                'a' 'b' [+] [print]
                'b' 'c' [+] [print]

                '' [print]
                ':: SUB ::' [print]
                2 2 [-] [print]
                3 4 [-] [print]

                '' [print]
                ':: MUL ::' [print]
                5 3 [*] [print]
                2 2 [*] [print]
                'a' 3 [*] [print]
                'b' 4 [*] [print]

                '' [print]
                ':: DIV ::' [print]
                9 3 [/] [print]
                4 2 [/] [print]

                '' [print]
                ':: LT ::' [print]
                1 1 [<] [print]
                5 9 [<] [print]

                '' [print]
                ':: LTE ::' [print]
                2 2 [<=] [print]
                4 9 [<=] [print]

                '' [print]
                ':: GT ::' [print]
                3 2 [>] [print]
                5 2 [>] [print]

                '' [print]
                ':: GTE ::' [print]
                3 3 [>=] [print]
                1 0 [>=] [print]

                '' [print]
                ':: EQ ::' [print]
                9 9 [==] [print]
                1 1 [==] [print]
                
                '' [print]
                ':: NEQ ::' [print]
                8 7 [!=] [print]
                1 1 [!=] [print]

                '' [print]
                ':: B-AND ::' [print]
                true true [&&] [print]
                false true [&&] [print]

                '' [print]
                ':: B-OR ::' [print]
                true false [||] [print]
                false false [||] [print]
                ",
                new Regex("\\-\\-.*(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"),
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex("\\((?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex("\\)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Type, new Regex("<[a-zA-Z0-9_\\-]+>(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Name, new Regex("@[a-zA-Z0-9_\\-]+(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex("::(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]*\")|('[^']*')")),
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
                Transformer transformer = new Transformer(tokens);
                List<TransformedToken> transformedTokens = transformer.Transform();
                if(transformedTokens != null)
                {
                    Interpreter interpreter = new Interpreter(
                        transformedTokens,
                        new Dictionary<string, FunctionBase>()
                        {
                            { "print", new FunctionPRINT() },
                            { "fn", new FunctionFN() },
                            { "if", new FunctionIF() },
                            { "for", new FunctionFOR() },
                            { "while", new FunctionWHILE() }
                        },
                        new Dictionary<string, OperatorBase>()
                        {
                            { "+", new OperatorADD() },
                            { "-", new OperatorSUB() },
                            { "*", new OperatorMUL() },
                            { "/", new OperatorDIV() },
                            { "<", new OperatorLT() },
                            { "<=", new OperatorLTE() },
                            { ">", new OperatorGT() },
                            { ">=", new OperatorGTE() },
                            { "==", new OperatorEQ() },
                            { "!=", new OperatorNEQ() },
                            { "&&", new OperatorBAND() },
                            { "||", new OperatorBOR() }
                        }
                    );
                    interpreter.PreInterpret();
                    interpreter.Interpret();
                }
            }

            Console.ReadLine();
        }
    }
}
