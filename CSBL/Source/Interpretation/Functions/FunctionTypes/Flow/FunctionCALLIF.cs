using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Flow
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [call-if] function.
    /// </summary>
    public class FunctionCALLIF : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionCALL class.
        /// </summary>
        public FunctionCALLIF()
            : base("call-if")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken callIf;
            TransformedToken label;

            if(interpreterEnvironment.ValueStack.Count > 0 && interpreterEnvironment.LabelStack.Count > 0)
            {
                callIf = interpreterEnvironment.ValueStack.Pop();
                label = interpreterEnvironment.LabelStack.Pop();
            }
            else
            {
                Errors.EmptyStack.Report(
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Data[0],
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
                return false;
            }

            if(label.Type == TransformedTokenType.LabelUsage)
            {
                if(callIf.Type == TransformedTokenType.Bool)
                {
                    if(interpreterEnvironment.DefinedLabels.ContainsKey(label.Data[0]))
                    {
                        if(callIf.Data[0])
                        {
                            interpreterEnvironment.CallStack.Push(interpreterEnvironment.CurrentTokenIndex);
                            interpreterEnvironment.CurrentTokenIndex = interpreterEnvironment.DefinedLabels[label.Data[0]];
                        }
                        return true;
                    }
                    else
                    {
                        Errors.UnknownLabel.Report(
                            label.Data[0],
                            label.Position.Line,
                            label.Position.Column
                        );
                        return false;
                    }
                }
                else
                {
                    Errors.InvalidValue.Report(
                        callIf.Data[0],
                        callIf.Position.Line,
                        callIf.Position.Column
                    );
                    return false;
                }
            }
            else
            {
                Errors.InvalidToken.Report(
                    label.Data[0],
                    label.Position.Line,
                    label.Position.Column
                );
                return false;
            }
        }
    }
}
