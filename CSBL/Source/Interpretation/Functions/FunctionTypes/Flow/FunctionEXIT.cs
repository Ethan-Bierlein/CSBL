using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Flow
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [exit] function.
    /// </summary>
    public class FunctionEXIT : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionEXIT class.
        /// </summary>
        public FunctionEXIT()
            : base("exit")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            Errors.ExitInterpreter.Report("", 0, 0);
            return false;
        }
    }
}
