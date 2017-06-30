using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Operators
{
    /// <summary>
    /// This class is a subclass of the OperatorBase class and represents
    /// the [+] operator.
    /// </summary>
    public class OperatorSUB : OperatorBase
    {
        /// <summary>
        /// Constructor for the OperatorADD class.
        /// </summary>
        public OperatorSUB()
            : base("-")
        { }

        /// <summary>
        /// Execute the operator.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override void Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b = interpreterEnvironment.ValueStack.Pop();
            TransformedToken a = interpreterEnvironment.ValueStack.Pop();

            if(a.Type == TransformedTokenType.Number && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.Number,
                        a.Data[0] - b.Data[0]
                    )
                );
            }

            else
            {
                Errors.IncompatibleOperatorTypes.Report(
                    a.Type,
                    b.Type,
                    '-',
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
            }
        }
    }
}
