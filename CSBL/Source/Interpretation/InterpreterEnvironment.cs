﻿using System;
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
        public Stack<TransformedToken> OpenParenthesesStack { get; set; }
        public Stack<TransformedToken> CloseParenthesesStack { get; set; }
        public Stack<TransformedToken> ValueStack { get; set; }
        public Stack<int> CallStack { get; set; }
        public Dictionary<string, CustomFunction> FunctionDefinitions { get; set; }

        /// <summary>
        /// Constructor for the InterpreterEnvironment class.
        /// </summary>
        public InterpreterEnvironment()
        {
            this.CurrentTokenIndex = 0;
            this.OpenParenthesesStack = new Stack<TransformedToken>() { };
            this.CloseParenthesesStack = new Stack<TransformedToken>() { };
            this.ValueStack = new Stack<TransformedToken>() { };
            this.CallStack = new Stack<int>() { };
            this.FunctionDefinitions = new Dictionary<string, CustomFunction>() { };
        }
    }
}