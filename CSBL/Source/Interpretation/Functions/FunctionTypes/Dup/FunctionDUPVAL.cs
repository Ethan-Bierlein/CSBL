using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-val] function.
    /// </summary>
    public class FunctionDUPVAL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPVAL class.
        /// </summary>
        public FunctionDUPVAL()
            : base("dup-val")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken valueToDuplicate;

            if(interpreterEnvironment.ValueStack.Count > 0)
            {
                valueToDuplicate = interpreterEnvironment.ValueStack.Peek();
                interpreterEnvironment.ValueStack.Push(valueToDuplicate);
                return true;
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
        }
    }
}
