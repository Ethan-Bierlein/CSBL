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
            TransformedToken nameValue = interpreterEnvironment.ValueStack.Pop();
            TransformedToken name = interpreterEnvironment.NameStack.Pop();

            if(
                nameValue.Type == TransformedTokenType.Bool ||
                nameValue.Type == TransformedTokenType.String ||
                nameValue.Type == TransformedTokenType.Number
            )
            {
                if(!interpreterEnvironment.DefinedValues.ContainsKey(name.Data[0]))
                {
                    interpreterEnvironment.DefinedValues.Add(name.Data[0], nameValue);
                    return true;
                }
                else
                {
                    Errors.RedefinedName.Report(
                        name.Data[0],
                        name.Position.Line,
                        name.Position.Column
                    );
                    return false;
                }
            }
            else
            {
                Errors.InvalidValue.Report(
                    name.Data[0],
                    name.Position.Line,
                    name.Position.Column
                );
                return false;
            }
        }
    }
}
