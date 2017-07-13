using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Memory
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [set] function.
    /// </summary>
    public class FunctionSET : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionSET class.
        /// </summary>
        public FunctionSET()
            : base("set")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken nameValue;
            TransformedToken name;

            if(interpreterEnvironment.ValueStack.Count > 0 && interpreterEnvironment.NameStack.Count > 0)
            {
                nameValue = interpreterEnvironment.ValueStack.Pop();
                name = interpreterEnvironment.NameStack.Pop();
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

            if(
                nameValue.Type == TransformedTokenType.Bool ||
                nameValue.Type == TransformedTokenType.String ||
                nameValue.Type == TransformedTokenType.Number
            )
            {
                if(!interpreterEnvironment.DefinedValues.ContainsKey(name.Data[0]))
                {
                    interpreterEnvironment.DefinedValues.Add(name.Data[0], nameValue);
                }
                else
                {
                    interpreterEnvironment.DefinedValues[name.Data[0]] = nameValue;
                }
                return true;
            }
            else
            {
                Errors.InvalidValue.Report(
                    name.Position.File,
                    name.Position.Line,
                    name.Position.Column,
                    name.Data[0]
                );
                return false;
            }
        }
    }
}
