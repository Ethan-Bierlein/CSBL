using System;
using CSBL.Reporting;
using CSBL.Transformation;

namespace CSBL.Interpretation
{
    /// <summary>
    /// This class represents a custom function, and 
    /// </summary>
    public class CustomFunction
    {
        public string Name { get; set; }
        public int StartTokenIndex { get; set; }
        public TransformedToken[] Parameters { get; set; }

        /// <summary>
        /// Constructor for the CustomFunction class.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="startTokenIndex">The start index of the function.</param>
        /// <param name="parameters">The parameters passed to the function.</param>
        public CustomFunction(string name, int startTokenIndex, params TransformedToken[] parameters)
        {
            this.Name = name;
            this.StartTokenIndex = startTokenIndex;
            this.Parameters = parameters;
        }
    }
}
