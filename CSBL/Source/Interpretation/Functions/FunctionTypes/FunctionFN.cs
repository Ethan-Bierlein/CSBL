using System;
using System.Collections.Generic;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [fn] function.
    /// </summary>
    public class FunctionFN : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionFN class.
        /// </summary>
        public FunctionFN()
            : base("fn")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            int currentTokenIndexTemp1 = interpreterEnvironment.CurrentTokenIndex - 1;
            List<TransformedToken> functionParameters = new List<TransformedToken>() { };
            TransformedToken functionName = null;

            while(currentTokenIndexTemp1 > 0)
            {
                TransformedToken currentToken = interpreter.InputTokens[currentTokenIndexTemp1];
                if(currentToken.Type == TransformedTokenType.TypedName)
                {
                    functionParameters.Add(currentToken);
                    currentTokenIndexTemp1--;
                }
                else if(currentToken.Type == TransformedTokenType.UntypedName)
                {
                    functionName = currentToken;
                    currentTokenIndexTemp1--;
                    break;
                }
                else
                {
                    Errors.UnexpectedToken.Report(
                        string.Join(" ", currentToken.Data),
                        currentToken.Position.Line,
                        currentToken.Position.Column
                    );
                    return false;
                }
            }

            int currentTokenIndexTemp2 = currentTokenIndexTemp1;
            if(interpreter.InputTokens[currentTokenIndexTemp2].Type == TransformedTokenType.CodeBlockClose)
            {
                Stack<TransformedToken> tempCloseParenthesesStack = new Stack<TransformedToken>() { };
                while(currentTokenIndexTemp2 > 0)
                {
                    TransformedToken currentToken = interpreter.InputTokens[currentTokenIndexTemp2];
                    if(currentToken.Type == TransformedTokenType.CodeBlockClose)
                    {
                        tempCloseParenthesesStack.Push(currentToken);
                        currentTokenIndexTemp2--;
                    }
                    else if(currentToken.Type == TransformedTokenType.CodeBlockOpen)
                    {
                        tempCloseParenthesesStack.Pop();
                        currentTokenIndexTemp2--;
                    }
                }

                if(tempCloseParenthesesStack.Count == 0)
                {
                    CustomFunction customFunction = new CustomFunction(functionName.Data[0], currentTokenIndexTemp2 + 1, functionParameters.ToArray());
                    interpreterEnvironment.FunctionDefinitions.Add(functionName.Data[0], customFunction);
                    return true;
                }
                else
                {
                    foreach(TransformedToken transformedToken in tempCloseParenthesesStack)
                    {
                        Errors.RuntimeUnbalancedParentheses.Report(
                            transformedToken.Position.Line,
                            transformedToken.Position.Column
                        );
                    }
                    return true;
                }
            }
            else
            {
                Errors.UnexpectedToken.Report(
                    string.Join(" ", interpreter.InputTokens[currentTokenIndexTemp2].Data),
                    interpreter.InputTokens[currentTokenIndexTemp2].Position.Line,
                    interpreter.InputTokens[currentTokenIndexTemp2].Position.Column
                );
                return false;
            }
        }
    }
}
