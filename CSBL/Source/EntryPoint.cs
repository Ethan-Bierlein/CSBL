using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Tokenization;
using CSBL.Transformation;
using CSBL.Interpretation;
using CSBL.Interpretation.Functions;
using CSBL.Interpretation.Functions.FunctionTypes.IO;
using CSBL.Interpretation.Functions.FunctionTypes.Flow;
using CSBL.Interpretation.Functions.FunctionTypes.Math;
using CSBL.Interpretation.Functions.FunctionTypes.Memory;
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
                1 (increment) [call] [print]
                2 (increment) [call] [print]
                16 (generate-batman) [call] [print]

                [exit]

                {increment} 
                    @<value-to-increment> [set]
                    @<value-to-increment> [get] 1 [+]
                    @<value-to-increment> [del]
                    [ret]

                {decrement}
                    @<value-to-decrement> [set]
                    @<value-to-decrement> [get] 1 [-]
                    @<value-to-decrement> [del]
                    [ret]

                {generate-batman}
                    @<na-count> [set]
                    'Na' @<na-count> [get] [*] ' Batman!' [+]
                    @<na-count> [del]
                    [ret]
                ",
                new Regex("(\\-\\-(.|\n)*\\-\\-)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"),
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]*\")|('[^']*')")),
                new TokenDefinition(TokenType.NumberLiteral, new Regex("((-|\\+?)((\\d+\\.\\d+)|(\\.\\d+)|(\\d+\\.)|(\\d+)))(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.CallFunction, new Regex("\\[[^\\[\\]]+\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.LabelDefinition, new Regex("{[a-zA-Z0-9_\\-]+}(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.LabelUsage, new Regex("\\([a-zA-Z0-9_\\-]+\\)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Name, new Regex("@<[a-zA-Z0-9_\\-]+>(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"))
            );

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
                            { "||", new FunctionOR() },

                            { "in", new FunctionIN() },
                            { "print", new FunctionPRINT() },

                            { "set", new FunctionSET() },
                            { "get", new FunctionGET() },
                            { "del", new FunctionDEL() },
                            { "pop-val", new FunctionPOPVAL() },
                            { "pop-lbl", new FunctionPOPLBL() },
                            { "pop-name", new FunctionPOPNAME() },
                            { "pop-call", new FunctionPOPCALL() },

                            { "call-if", new FunctionCALLIF() },
                            { "call", new FunctionCALL() },
                            { "ret", new FunctionRET() },
                            { "exit", new FunctionEXIT() }
                        }
                    );

                    if(interpreter.PreInterpret())
                    {
                        interpreter.Interpret();
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
