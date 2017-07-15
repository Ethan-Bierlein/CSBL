using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-call-if] function.
    /// </summary>
    public class FunctionDUPCALLIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPCALLIF class.
        /// </summary>
        public FunctionDUPCALLIF()
            : base("dup-call-if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken shouldDuplicate;
            int callToDuplicate;

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

            if(interpreterEnvironment.CallStack.Count > 0)
            {
                if(shouldDuplicate.Data[0])
                {
                    callToDuplicate = interpreterEnvironment.CallStack.Peek();
                    interpreterEnvironment.CallStack.Push(callToDuplicate);
                }
                return true;
            }
            else
            {
                Errors.EmptyCallStack.Report(
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
