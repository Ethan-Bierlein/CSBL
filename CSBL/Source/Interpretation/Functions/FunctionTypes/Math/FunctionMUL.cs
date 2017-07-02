﻿using System;
using System.Linq;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Functions.FunctionTypes.Math
{
    /// <summary>
    /// This class is a subclass of the FunctionBase class and represents
    /// the [*] function.
    /// </summary>
    public class FunctionMUL : FunctionBase
    {
        /// <summary>
        /// Constructor for the FunctionMUL class.
        /// </summary>
        public FunctionMUL()
            : base("*")
        { }

        /// <summary>
        /// Execute the function.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b = interpreterEnvironment.ValueStack.Pop();
            TransformedToken a = interpreterEnvironment.ValueStack.Pop();

            if(a.Type == TransformedTokenType.Number && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.Number,
                        a.Data[0] * b.Data[0]
                    )
                );
                return true;
            }

            else if(a.Type == TransformedTokenType.String && b.Type == TransformedTokenType.Number)
            {
                interpreterEnvironment.ValueStack.Push(
                    new TransformedToken(
                        a.Position,
                        TransformedTokenType.String,
                        string.Concat(Enumerable.Repeat(a.Data[0], (int)b.Data[0]))
                    )
                );
                return true;
            }

            else
            {
                Errors.IncompatibleOperatorTypes.Report(
                    a.Type,
                    b.Type,
                    "*",
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Line,
                    interpreter.InputTokens[interpreterEnvironment.CurrentTokenIndex].Position.Column
                );
                return false;
            }
        }
    }
}