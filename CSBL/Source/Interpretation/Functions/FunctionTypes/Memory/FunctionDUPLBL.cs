using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Memory
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-lbl] function.
    /// </summary>
    public class FunctionDUPLBL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPLBL class.
        /// </summary>
        public FunctionDUPLBL()
            : base("dup-lbl")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken labelToDuplicate;

            if(interpreterEnvironment.LabelStack.Count > 0)
            {
                labelToDuplicate = interpreterEnvironment.NameStack.Peek();
                interpreterEnvironment.LabelStack.Push(labelToDuplicate);
                return true;
            }
            else
            {
                Errors.EmptyStack.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Data[0],
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
                return false;
            }
        }
    }
}
