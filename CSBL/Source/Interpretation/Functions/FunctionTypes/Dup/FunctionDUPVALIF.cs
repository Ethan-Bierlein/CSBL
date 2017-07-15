using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-val-if] function.
    /// </summary>
    public class FunctionDUPVALIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPVALIF class.
        /// </summary>
        public FunctionDUPVALIF()
            : base("dup-val-if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken shouldDuplicate;
            TransformedToken valueToDuplicate;

            if(interpreterEnvironment.ValueStack.Count > 0)
            {
                shouldDuplicate = interpreterEnvironment.ValueStack.Pop();
                if(shouldDuplicate.Type != TransformedTokenType.Bool)
                {
                    Errors.InvalidValue.Report(
                        shouldDuplicate.Position.File,
                        shouldDuplicate.Position.Line,
                        shouldDuplicate.Position.Column,
                        shouldDuplicate.Data[0]
                    );
                    return false;
                }
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

            if(interpreterEnvironment.ValueStack.Count > 0)
            {
                if(shouldDuplicate.Data[0])
                {
                    valueToDuplicate = interpreterEnvironment.ValueStack.Peek();
                    interpreterEnvironment.ValueStack.Push(valueToDuplicate);
                }
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
