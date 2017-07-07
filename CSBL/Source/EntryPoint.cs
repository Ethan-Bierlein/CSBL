﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CSBL.Reporting;
using CSBL.Tokenization;
using CSBL.Preprocessing;
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
        /// <summary>
        /// This function performs the initialization stage of CSBL.
        /// </summary>
        /// <param name="args">The command line arguments passed to CSBL.</param>
        /// <param name="success">Whether or not the stage succeeded.</param>
        /// <returns>The source string of the input file.</returns>
        public static string StageInitialization(string[] args, ref bool success)
        {
            if(args.Length >= 0)
            {
                string filePath = args[0];
                try
                {
                    string outputString = File.ReadAllText(filePath);
                    success = true;
                    return outputString;
                }
                catch(Exception)
                {
                    Errors.FileOpeningFailed.Report();
                    success = false;
                    return null;
                }
            }
            else
            {
                Errors.NoFileProvided.Report();
                success = false;
                return null;
            }
        }

        /// <summary>
        /// This function performs the preprocessing stage of CSBL.
        /// </summary>
        /// <param name="inputFileDirectory">The directory of the input file.</param>
        /// <param name="inputString">The input string from the provided source file.</param>
        /// <param name="success">Whether or not the stage succeeded.</param>
        /// <returns>The preprocessed source string.</returns>
        public static string StagePreprocessor(string inputFileDirectory, string inputString, ref bool success)
        {
            List<string> definedNames = new List<string>() { };
            List<string> includedFiles = new List<string>() { };
            int outputTokensCount = 1;
            int numberOfInsertedChars = 0;
            string outputString = inputString;

            while(outputTokensCount > 0)
            {
                if(outputString == null)
                {
                    break;
                }

                Preprocessor preprocessor = new Preprocessor(
                    Path.GetDirectoryName(Path.GetFullPath(inputFileDirectory)),
                    outputString,
                    new Regex("//.*"),
                    new PreprocessorTokenDefinition(PreprocessorTokenType.Import, new Regex("#use\\s+(\'|\")[^\n]+(\'|\")"))
                );

                outputString = preprocessor.GenerateOutput(
                    ref definedNames,
                    ref includedFiles,
                    ref outputTokensCount,
                    ref numberOfInsertedChars
                );
            }

            success = outputString == null ? false : true;
            return outputString;
        }

        /// <summary>
        /// This function performs the tokenization stage of CSBL.
        /// </summary>
        /// <param name="inputString">The input string generated by the preprocessor.</param>
        /// <param name="success">Whether or not the stage succeeded.</param>
        /// <returns>A list of tokens.</returns>
        public static List<Token> StageTokenize(string inputString, ref bool success)
        {
            Tokenizer tokenizer = new Tokenizer(
                inputString,
                new TokenDefinition(TokenType.BoolLiteral, new Regex("(true|false)(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]*\")|('[^']*')")),
                new TokenDefinition(TokenType.NumberLiteral, new Regex("((-|\\+?)((\\d+\\.\\d+)|(\\.\\d+)|(\\d+\\.)|(\\d+)))(?=(?:[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\))[^'\"{}\\(\\)]*('|\"|{|}|\\(|\\)))*[^'\"{}\\(\\)]*\\Z)")),
                new TokenDefinition(TokenType.CallFunction, new Regex("\\[[^\\[\\]]+\\](?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.LabelDefinition, new Regex("{[a-zA-Z0-9_\\-]+}(?=(?:[^'\"\\^]*('|\"|\\^)[^'\"\\^]*('|\"|\\^))*[^'\"\\^]*\\Z)")),
                new TokenDefinition(TokenType.LabelUsage, new Regex("\\([a-zA-Z0-9_\\-]+\\)(?=(?:[^'\"\\^]*('|\"|\\^)[^'\"\\^]*('|\"|\\^))*[^'\"\\^]*\\Z)")),
                new TokenDefinition(TokenType.StacklessLabelDefinition, new Regex("\\^{[a-zA-Z0-9_\\-]+}\\^(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.StacklessLabelUsage, new Regex("\\^\\([a-zA-Z0-9_\\-]+\\)\\^(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)")),
                new TokenDefinition(TokenType.Name, new Regex("@<[a-zA-Z0-9_\\-]+>(?=(?:[^'\"]*('|\")[^'\"]*('|\"))*[^'\"]*\\Z)"))
            );

            List<Token> tokens = tokenizer.Tokenize();
            success = tokens == null ? false : true;
            return tokens;
        }

        /// <summary>
        /// This function performs the transformation stage of CSBL.
        /// </summary>
        /// <param name="inputTokens">The input tokens generated by the tokenizer.</param>
        /// <param name="success">Whether or not the stage succeeded.</param>
        /// <returns>A list of transformed tokens.</returns>
        public static List<TransformedToken> StageTransform(List<Token> inputTokens, ref bool success)
        {
            Transformer transformer = new Transformer(inputTokens);
            List<TransformedToken> transformedTokens = transformer.Transform();
            success = transformedTokens == null ? false : true;
            return transformedTokens;
        }

        /// <summary>
        /// This function performs the interpretation stage of CSBL.
        /// </summary>
        /// <param name="inputTokens">The input tokens generated by the transformer.</param>
        public static void StageInterpret(List<TransformedToken> inputTokens)
        {
            Interpreter interpreter = new Interpreter(
                inputTokens,
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

        /// <summary>
        /// Main entry point function..
        /// </summary>
        /// <param name="args">The command line arguments passed.</param>
        [STAThread]
        public static void Main(string[] args)
        {
            bool success = false;
            string inputFileString = StageInitialization(args, ref success);
            if(success)
            {
                string inputPreprocessedString = StagePreprocessor(args[0], inputFileString, ref success);
                if(success)
                {
                    List<Token> inputTokenizedTokens = StageTokenize(inputPreprocessedString, ref success);
                    if(success)
                    {
                        List<TransformedToken> inputTransformedTokens = StageTransform(inputTokenizedTokens, ref success);
                        if(success)
                        {
                            StageInterpret(inputTransformedTokens);
                        }
                    }
                }
            }
        }
    }
}
