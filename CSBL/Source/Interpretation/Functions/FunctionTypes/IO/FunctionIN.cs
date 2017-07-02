using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.IO
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [in] function.
    /// </summary>
    public class FunctionIN : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionOUT class.
        /// </summary>
        public FunctionIN()
            : base("in")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            return true;
        }
    }
}
