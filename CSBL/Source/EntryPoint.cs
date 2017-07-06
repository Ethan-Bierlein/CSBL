using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        [STAThread]
        public static void Main(string[] args)
        {
            string inputString = @"
                #def VAL-TO-INC-1 8
                #def VAL-TO-INC-1 9
                #def BATMAN-NA-COUNT 16
                #def BATMAN-NA-COUNT 17
                #def EXIT-STRING 'Exiting the program!'
                #def EXIT-STRING 'Exiting the program?'

                VAL-TO-INC-1 (increment) [call] [print]
                VAL-TO-INC-2 (increment) [call] [print]
                BATMAN-NA-COUNT (generate-batman) [call] [print]
                EXIT-STRING [print]

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
                ";

            List<string> definedNames = new List<string>() { };
            List<string> includedFiles = new List<string>() { };
            int outputTokensCount = 1;
            string outputString = inputString;

            while(outputTokensCount > 0)
            {
                if(outputString == null)
                {
                    break;
                }

                Preprocessor preprocessor = new Preprocessor(
                    outputString,
                    new PreprocessorTokenDefinition(PreprocessorTokenType.Define, new Regex("#def\\s+[a-zA-Z0-9_\\-]+\\s+[^\n]+\\s*\n")),
                    new PreprocessorTokenDefinition(PreprocessorTokenType.Import, new Regex("#use\\s+\"[^\n]+\"\\s*\n"))
                );
                outputString = preprocessor.GenerateOutput(ref definedNames, ref includedFiles, ref outputTokensCount);
            }

            if(outputString != null)
            {
                Tokenizer tokenizer = new Tokenizer(
                    outputString,
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
            }

            Console.ReadLine();
        }
    }
}
