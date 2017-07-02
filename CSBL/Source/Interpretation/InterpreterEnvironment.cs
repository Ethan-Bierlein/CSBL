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

        /// <summary>
        /// Constructor for the InterpreterEnvironment class.
        /// </summary>
        public InterpreterEnvironment()
        {
            this.CurrentTokenIndex = 0;
            this.ValueStack = new Stack<TransformedToken>() { };
        }
    }
}