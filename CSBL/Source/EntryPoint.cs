﻿using System;
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
                ",
                new Regex(@"\-\-.*"),
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex("\\((?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex("\\)(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Type, new Regex("<[a-zA-Z0-9_\\-]+>(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Name, new Regex("@[a-zA-Z0-9_\\-]+(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex("::(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)")),
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
