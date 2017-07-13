using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Flow
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [ret] function.
    /// </summary>
    public class FunctionRET : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionRET class.
        /// </summary>
        public FunctionRET()
            : base("ret")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            if(interpreterEnvironment.CallStack.Count > 0)
            {
                int returnTo = interpreterEnvironment.CallStack.Pop();
                interpreterEnvironment.CurrentTokenIndex = returnTo;
                return true;
            }
            else
            {
                Errors.EmptyCallStack.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
                return false;
            }
        }
    }
}
