﻿using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Pop
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [pop-name] function.
    /// </summary>
    public class FunctionPOPNAME : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionPOPNAME class.
        /// </summary>
        public FunctionPOPNAME()
            : base("pop-name")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            if(interpreterEnvironment.NameStack.Count > 0)
            {
                interpreterEnvironment.NameStack.Pop();
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
