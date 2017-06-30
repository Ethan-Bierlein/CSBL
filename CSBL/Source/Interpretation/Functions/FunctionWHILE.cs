using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [while] function.
    /// </summary>
    public class FunctionWHILE : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionWHILE class.
        /// </summary>
        public FunctionWHILE()
            : base("while")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override void Execute(Interpreter interpreter, InterpreterEnvironment intepreterEnvironment)
        {

        }
    }
}
