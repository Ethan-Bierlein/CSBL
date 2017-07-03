using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Memory
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [popname] function.
    /// </summary>
    public class FunctionPOPNAME : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionPOPNAME class.
        /// </summary>
        public FunctionPOPNAME()
            : base("popname")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            if(interpreterEnvironment.NameStack.Count > 0)
            {
                interpreterEnvironment.NameStack.Pop();
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
