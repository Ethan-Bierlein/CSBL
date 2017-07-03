﻿using System;
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
                (test-fn-1) [call]
                (test-fn-2) [call]
                (test-fn-3) [call]
                
                [exit]

                {test-fn-1} 'test-fn-1 called' [print] [ret]
                {test-fn-2} 'test-fn-2 called' [print] [ret]
                {test-fn-3} 'test-fn-3 called' [print] [ret]
                ",
                new Regex("(\\-\\-(.|\n)*\\-\\-)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"),
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]*\")|('[^']*')")),
                new TokenDefinition(TokenType.NumberLiteral, new Regex("((-|\\+?)((\\d+\\.\\d+)|(\\.\\d+)|(\\d+\\.)|(\\d+)))(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.CallFunction, new Regex("\\[[^\\[\\]]+\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.LabelDefinition, new Regex("{[a-zA-Z0-9_\\-]+}(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.LabelUsage, new Regex("\\([a-zA-Z0-9_\\-]+\\)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"))
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
