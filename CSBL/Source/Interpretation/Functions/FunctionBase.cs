using System;

namespace CSBL.Interpretation.Functions
{
    /// <summary>
    /// This class serves as the base for operators within CSBL.
    /// </summary>
    public class FunctionBase
    {
        public string Name { get; set; }

        /// <summary>
        /// Constructor for the OperatorBase class.
        /// </summary>
        /// <param name="name">The name of the operator.</param>
        public FunctionBase(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Execute the operator.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public virtual bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            return true;
        }
    }
}
