using System;
using System.Collections.Generic;
using CSBL.Transformation;

namespace CSBL.Interpretation
{
    /// <summary>
    /// This class is responsible for managing and storing an interpreter 
    /// environment. It stores things like a open/closing parentheses stacks,
    /// value stacks, etc.
    /// </summary>
    public class InterpreterEnvironment
    {
        public int CurrentTokenIndex { get; set; }
        public Stack<TransformedToken> ValueStack { get; set; }
        public Stack<TransformedToken> LabelStack { get; set; }
        public Stack<TransformedToken> NameStack { get; set; }
        public Stack<int> CallStack { get; set; }
        public Dictionary<string, int> DefinedLabels { get; set; }
        public Dictionary<string, TransformedToken> DefinedValues { get; set; }

        /// <summary>
        /// Constructor for the InterpreterEnvironment class.
        /// </summary>
        public InterpreterEnvironment()
        {
            this.CurrentTokenIndex = 0;
            this.ValueStack = new Stack<TransformedToken>() { };
            this.LabelStack = new Stack<TransformedToken>() { };
            this.NameStack = new Stack<TransformedToken>() { };
            this.CallStack = new Stack<int>() { };
            this.DefinedLabels = new Dictionary<string, int>() { };
            this.DefinedValues = new Dictionary<string, TransformedToken>() { };
        }
    }
}