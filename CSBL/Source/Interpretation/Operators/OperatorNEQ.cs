﻿using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Operators
{
    /// <summary>
    /// This class is a subclass of the OperatorBase class and represents
    /// the [!=] operator.
    /// </summary>
    public class OperatorNEQ : OperatorBase
    {
        /// <summary>
        /// Constructor for the OperatorNE class.
        /// </summary>
        public OperatorNEQ()
            : base("!=")
        { }

        /// <summary>
        /// Execute the operator.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override void Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b = interpreterEnvironment.ValueStack.Pop();
            TransformedToken a = interpreterEnvironment.ValueStack.Pop();

            interpreterEnvironment.ValueStack.Push(
                new TransformedToken(
                    a.Position,
                    TransformedTokenType.Bool,
                    a.Data != b.Data
                )
            );
        }
    }
}
