using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Memory
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [get] function.
    /// </summary>
    public class FunctionGET : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionGET class.
        /// </summary>
        public FunctionGET()
            : base("get")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken nameValueToGet;

            if(interpreterEnvironment.NameStack.Count > 0)
            {
                nameValueToGet = interpreterEnvironment.NameStack.Pop();
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

            if(interpreterEnvironment.DefinedValues.ContainsKey(nameValueToGet.Data[0]))
            {
                interpreterEnvironment.ValueStack.Push(interpreterEnvironment.DefinedValues[nameValueToGet.Data[0]]);
                return true;
            }
            else
            {
                Errors.UndefinedName.Report(
                    nameValueToGet.Position.File,
                    nameValueToGet.Position.Line,
                    nameValueToGet.Position.Column,
                    nameValueToGet.Data[0]
                );
                return false;
            }
        }
    }
}
