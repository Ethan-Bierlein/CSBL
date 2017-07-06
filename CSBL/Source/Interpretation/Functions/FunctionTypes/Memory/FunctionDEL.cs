using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Memory
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [del] function.
    /// </summary>
    public class FunctionDEL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionSET class.
        /// </summary>
        public FunctionDEL()
            : base("del")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken nameToDelete;

            if(interpreterEnvironment.NameStack.Count > 0)
            {
                nameToDelete = interpreterEnvironment.NameStack.Pop();
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

            if(interpreterEnvironment.DefinedValues.ContainsKey(nameToDelete.Data[0]))
            {
                interpreterEnvironment.DefinedValues.Remove(nameToDelete.Data[0]);
                return true;
            }
            else
            {
                Errors.UndefinedName.Report(
                    nameToDelete.Data[0],
                    nameToDelete.Position.Line,
                    nameToDelete.Position.Column
                );
                return false;
            }
        }
    }
}
