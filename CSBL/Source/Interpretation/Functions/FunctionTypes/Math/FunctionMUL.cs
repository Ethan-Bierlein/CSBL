using System;
using System.Linq;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Math
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [*] function.
    /// </summary>
    public class FunctionMUL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionMUL class.
        /// </summary>
        public FunctionMUL()
            : base("*")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b;
            TransformedToken a;

            if(interpreterEnvironment.ValueStack.Count >= 2)
            {
                b = interpreterEnvironment.ValueStack.Pop();
                a = interpreterEnvironment.ValueStack.Pop();
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

            if(a.Type == TransformedTokenType.Number && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.Number,
                        a.Data[0] * b.Data[0]
                    )
                );
                return true;
            }

            else if(a.Type == TransformedTokenType.String && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.String,
                        string.Concat(Enumerable.Repeat(a.Data[0], (int)b.Data[0]))
                    )
                );
                return true;
            }

            else
            {
                Errors.IncompatibleOperatorTypes.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column,
                    a.Type,
                    b.Type,
                    this.Name
                );
                return false;
            }
        }
    }
}