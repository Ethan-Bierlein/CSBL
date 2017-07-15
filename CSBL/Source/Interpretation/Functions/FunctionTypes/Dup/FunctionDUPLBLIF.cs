using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-lbl-if] function.
    /// </summary>
    public class FunctionDUPLBLIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPLBLIF class.
        /// </summary>
        public FunctionDUPLBLIF()
            : base("dup-lbl-if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken shouldDuplicate;
            TransformedToken labelToDuplicate;

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

            if(interpreterEnvironment.LabelStack.Count > 0)
            {
                if(shouldDuplicate.Data[0])
                {
                    labelToDuplicate = interpreterEnvironment.LabelStack.Peek();
                    interpreterEnvironment.LabelStack.Push(labelToDuplicate);
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
