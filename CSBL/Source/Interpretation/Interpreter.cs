﻿using System;
using System.Collections.Generic;
using CSBL.Reporting;
using CSBL.Transformation;
using CSBL.Interpretation.Operators;

namespace CSBL.Interpretation
{
    /// <summary>
    /// This class is responsible for taking a list of transformed tokens and interpreting
    /// them. The interpreter also stores an environment which is managed and changed by
    /// the input code.
    /// </summary>
    public class Interpreter
    {
        public List<TransformedToken> InputTokens { get; private set; }
        public Dictionary<string, OperatorBase> Functions { get; private set; }
        public InterpreterEnvironment Environment { get; private set; }

        /// <summary>
        /// Constructor for the Interpreter class.
        /// </summary>
        /// <param name="inputTokens">The input tokens generated by the transformer.</param>
        /// <param name="functions">The default operators within CSBL.</param>
        public Interpreter(List<TransformedToken> inputTokens, Dictionary<string, OperatorBase> functions)
        {
            this.InputTokens = inputTokens;
            this.Functions = functions;
            this.Environment = new InterpreterEnvironment();
        }

        public void PreInterpret()
        {

        }

        public void Interpret()
        {
            while(this.Environment.CurrentTokenIndex < this.InputTokens.Count)
            {
                TransformedToken currentToken = this.InputTokens[this.Environment.CurrentTokenIndex];
                switch(currentToken.Type)
                {
                    case TransformedTokenType.CallFunction:
                        if(this.Functions.ContainsKey(currentToken.Data[0]))
                        {
                            bool success = this.Functions[currentToken.Data[0]].Execute(this, this.Environment);
                            if(!success)
                            {
                                return;
                            }
                        }
                        else
                        {
                            Errors.UnknownFunction.Report(
                                currentToken.Data[0],
                                currentToken.Position.Line,
                                currentToken.Position.Column
                            );
                            return;
                        }
                        this.Environment.CurrentTokenIndex++;
                        break;

                    default:
                        this.Environment.ValueStack.Push(currentToken);
                        this.Environment.CurrentTokenIndex++;
                        break;
                }
            }
        }
    }
}
