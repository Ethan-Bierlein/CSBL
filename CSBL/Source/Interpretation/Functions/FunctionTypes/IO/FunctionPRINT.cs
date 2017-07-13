using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.IO
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [print] function.
    /// </summary>
    public class FunctionPRINT : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionPRINT class.
        /// </summary>
        public FunctionPRINT()
            : base("print")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken tokenToWrite;

            if(interpreterEnvironment.ValueStack.Count > 0)
            {
                tokenToWrite = interpreterEnvironment.ValueStack.Pop();
            }
            else
            {
                Errors.EmptyStack.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Data[0]
                );
                return false;
            }

            Console.WriteLine(string.Join("", tokenToWrite.Data));
            return true;
        }
    }
}
