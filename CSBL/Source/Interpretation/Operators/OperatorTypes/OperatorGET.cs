using System;
using System.Collections.Generic;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation.Operators.OperatorTypes
{
    /// <summary>
    /// This class is a subclass of the OperatorClass and represents 
    /// the [-&gt;] operator.
    /// </summary>
    public class OperatorGET : OperatorBase
    {
        /// <summary>
        /// Constructor for the OperatorGET class.
        /// </summary>
        public OperatorGET()
            : base("->")
        { }

        /// <summary>
        /// Execute the operator.
        /// </summary>
        /// <param name="interpreter">A reference to the current interpreter.</param>
        /// <param name="interpreterEnvironment">A reference to the current interpreter environment.</param>
        public override bool Execute(Interpreter interpreter, InterpreterEnvironment interpreterEnvironment)
        {
            TransformedToken b = interpreterEnvironment.ValueStack.Pop();
            TransformedToken a = interpreterEnvironment.ValueStack.Pop();

            if(a.Type == TransformedTokenType.Array && b.Type == TransformedTokenType.Number)
            {
                if(b.Data[0] >= 0 && b.Data[0] < ((List<TransformedToken>)a.Data[0]).Count)
                {
                    interpreterEnvironment.ValueStack.Push(
                        new TransformedToken(
                            a.Position,
                            ((TransformedToken)a.Data[0][(int)b.Data[0]]).Type,
                            ((TransformedToken)a.Data[0][(int)b.Data[0]]).Data
                         )
                    );
                    return true;
                }
                else
                {
                    Errors.InvalidArrayIndex.Report(
                        b.Data[0],
                        b.Position.Line,
                        b.Position.Column
                    );
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
