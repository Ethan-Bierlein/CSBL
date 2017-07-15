using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Dup
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [dup-name] function.
    /// </summary>
    public class FunctionDUPNAME : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionDUPNAME class.
        /// </summary>
        public FunctionDUPNAME()
            : base("dup-name")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken nameToDuplicate;

            if(interpreterEnvironment.NameStack.Count > 0)
            {
                nameToDuplicate = interpreterEnvironment.NameStack.Peek();
                interpreterEnvironment.NameStack.Push(nameToDuplicate);
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
