﻿using System;
using System.Collections.Generic;
using CSBL.Reporting;
using CSBL.Transformation;
using CSBL.Interpretation.Functions;

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
        public Dictionary<string, FunctionBase> Functions { get; private set; }
        public InterpreterEnvironment Environment { get; private set; }

        /// <summary>
        /// Constructor for the Interpreter class.
        /// </summary>
        /// <param name="inputTokens">The input tokens generated by the transformer.</param>
        /// <param name="functions">The default functions within CSBL.</param>
        public Interpreter(List<TransformedToken> inputTokens, Dictionary<string, FunctionBase> functions)
        {
            this.InputTokens = inputTokens;
            this.Functions = functions;
            this.Environment = new InterpreterEnvironment();
        }

        /// <summary>
        /// Iterate over the list of transforemd tokens generated from the token list generated
        /// by the tokenizer and generate a list of 
        /// </summary>
        public bool PreInterpret()
        {
            Dictionary<string, int> definedLabels = new Dictionary<string, int>() { };
            for(int i = 0; i < this.InputTokens.Count; i++)
            {
                TransformedToken currentToken = this.InputTokens[i];
                if(
                    currentToken.Type == TransformedTokenType.LabelDefinition || 
                    currentToken.Type == TransformedTokenType.StacklessLabelDefinition
                )
                {
                    if(!definedLabels.ContainsKey(currentToken.Data[0]))
                    {
                        definedLabels.Add(currentToken.Data[0], i);
                    }
                }
            }

            this.Environment.DefinedLabels = definedLabels;
            return true;
        }

        /// <summary>
        /// Iterate over the list of transformed tokens generated from the token list generated
        /// by the tokenizer and produce a result.
        /// </summary>
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
                                currentToken.Position.File,
                                currentToken.Position.Line,
                                currentToken.Position.Column,
                                currentToken.Data[0]
                            );
                            return;
                        }
                        this.Environment.CurrentTokenIndex++;
                        break;

                    case TransformedTokenType.LabelDefinition:
                        this.Environment.CurrentTokenIndex++;
                        break;

                    case TransformedTokenType.LabelUsage:
                        this.Environment.LabelStack.Push(currentToken);
                        this.Environment.CurrentTokenIndex++;
                        break;

                    case TransformedTokenType.StacklessLabelDefinition:
                        this.Environment.CurrentTokenIndex++;
                        break;

                    case TransformedTokenType.StacklessLabelUsage:
                        this.Environment.LabelStack.Push(currentToken);
                        this.Environment.CurrentTokenIndex++;
                        break;

                    case TransformedTokenType.Name:
                        this.Environment.NameStack.Push(currentToken);
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
