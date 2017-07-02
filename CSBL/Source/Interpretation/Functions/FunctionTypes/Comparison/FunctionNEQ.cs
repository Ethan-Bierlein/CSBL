using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Comparison
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [!=] function.
    /// </summary>
    public class FunctionNEQ : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionNE class.
        /// </summary>
        public FunctionNEQ()
            : base("!=")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b = interpreterEnvironment.ValueStack.Pop();
            TransformedToken a = interpreterEnvironment.ValueStack.Pop();

            interpreterEnvironment.ValueStack.Push(
                new TransformedToken(
                    a.Position,
                    TransformedTokenType.Bool,
                    a.Data != b.Data
                )
            );

            return true;
        }
    }
}
