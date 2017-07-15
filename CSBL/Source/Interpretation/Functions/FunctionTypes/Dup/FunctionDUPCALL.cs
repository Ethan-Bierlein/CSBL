using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-call] function.
    /// </summary>
    public class FunctionDUPCALL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPCALL class.
        /// </summary>
        public FunctionDUPCALL()
            : base("dup-call")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            int callToDuplicate;

            if(interpreterEnvironment.CallStack.Count > 0)
            {
                callToDuplicate = interpreterEnvironment.CallStack.Peek();
                interpreterEnvironment.CallStack.Push(callToDuplicate);
                return true;
            }
            else
            {
                Errors.EmptyCallStack.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Data[0]
                );
                return false;
            }
        }
    }
}
