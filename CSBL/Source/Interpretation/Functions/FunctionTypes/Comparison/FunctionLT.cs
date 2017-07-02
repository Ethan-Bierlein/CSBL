using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Comparison
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [&lt;] function.
    /// </summary>
    public class FunctionLT : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionLT class.
        /// </summary>
        public FunctionLT()
            : base("<")
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

            if(a.Type == TransformedTokenType.Number && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.Bool,
                        a.Data[0] < b.Data[0]
                    )
                );
                return true;
            }

            else
            {
                Errors.IncompatibleOperatorTypes.Report(
                    a.Type,
                    b.Type,
                    "<",
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
                return false;
            }
        }
    }
}