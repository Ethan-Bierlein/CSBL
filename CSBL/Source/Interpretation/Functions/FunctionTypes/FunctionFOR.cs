﻿using System;
using CSBL.Reporting;

namespace CSBL.Interpretation.Functions.FunctionTypes
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [for] function.
    /// </summary>
    public class FunctionFOR : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionFOR class.
        /// </summary>
        public FunctionFOR()
            : base("for")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="intepreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment intepreterEnvironment)
        {
            return false;
        }
    }
}