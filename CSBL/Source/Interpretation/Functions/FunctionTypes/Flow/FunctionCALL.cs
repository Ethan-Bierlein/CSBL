using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Flow
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [call] function.
    /// </summary>
    public class FunctionCALL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionCALL class.
        /// </summary>
        public FunctionCALL()
            : base("call")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken label;

            if(interpreterEnvironment.LabelStack.Count > 0)
            {
                label = interpreterEnvironment.LabelStack.Pop();
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

            if(label.Type == TransformedTokenType.LabelUsage)
            {
                if(interpreterEnvironment.DefinedLabels.ContainsKey(label.Data[0]))
                {
                    if(interpreter.InputTokens[interpreterEnvironment.DefinedLabels[label.Data[0]]].Type == TransformedTokenType.LabelDefinition)
                    {
                        interpreterEnvironment.CallStack.Push(interpreterEnvironment.CurrentTokenIndex);
                        interpreterEnvironment.CurrentTokenIndex = interpreterEnvironment.DefinedLabels[label.Data[0]];
                        return true;
                    }
                    else
                    {
                        Errors.InvalidLabelType.Report(
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                        );
                        return false;
                    }
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
            else if(label.Type == TransformedTokenType.StacklessLabelUsage)
            {
                if(interpreterEnvironment.DefinedLabels.ContainsKey(label.Data[0]))
                {
                    if(interpreter.InputTokens[interpreterEnvironment.DefinedLabels[label.Data[0]]].Type == TransformedTokenType.StacklessLabelDefinition)
                    {
                        interpreterEnvironment.CurrentTokenIndex = interpreterEnvironment.DefinedLabels[label.Data[0]];
                        return true;
                    }
                    else
                    {
                        Errors.InvalidLabelType.Report(
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.File,
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                            interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                        );
                        return false;
                    }
                }
                else
                {
                    Errors.UnknownLabel.Report(
                        label.Position.File,
                        label.Position.Line,
                        label.Position.Column,
                        label.Data[0]
                    );
                    return false;
                }
            }
            else
            {
                Errors.InvalidToken.Report(
                    label.Position.File,
                    label.Position.Line,
                    label.Position.Column,
                    label.Data[0]
                );
                return false;
            }
        }
    }
}
