using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Pop
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [pop-name-if] function.
    /// </summary>
    public class FunctionPOPNAMEIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionPOPNAMEIF class.
        /// </summary>
        public FunctionPOPNAMEIF()
            : base("pop-name-if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken shouldPop;

            if(interpreterEnvironment.ValueStack.Count > 0)
            {
                shouldPop = interpreterEnvironment.ValueStack.Pop();
                if(shouldPop.Type != TransformedTokenType.Bool)
                {
                    Errors.InvalidValue.Report(
                        shouldPop.Position.File,
                        shouldPop.Position.Line,
                        shouldPop.Position.Column,
                        shouldPop.Data[0]
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

            if(interpreterEnvironment.NameStack.Count > 0)
            {
                if(shouldPop.Data[0])
                {
                    interpreterEnvironment.NameStack.Pop();
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
