﻿using System;
using System.Text.RegularExpressions;
using CSBL.Reporting;

namespace CSBL.Preprocessing
{
    /// <summary>
    /// 
    /// </summary>
    public class PreprocessorTokenDefinition
    {
        public PreprocessorTokenType Type { get; set; }
        public Regex Regex { get; set; }
        
        /// <summary>
        /// Constructor for the PreprocessorTokenDefinition class.
        /// </summary>
        /// <param name="type">The type of preprocessor token.</param>
        /// <param name="regex">The regex for matching the preprocessor token.</param>
        public PreprocessorTokenDefinition(PreprocessorTokenType type, Regex regex)
        {
            this.Type = type;
            this.Regex = regex;
        }

        /// <summary>
        /// Check to see whether or a match occurs with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <returns>Whether or not a match occured.</returns>
        public bool IsMatch(string other)
        {
            return this.Regex.IsMatch(other);
        }

        /// <summary>
        /// Check to see whether or a match occurs with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <param name="startAt">Where to start matching at.</param>
        /// <returns>Whether or not a match occured.</returns>
        public bool IsMatch(string other, int startAt)
        {
            return this.Regex.IsMatch(other, startAt);
        }

        /// <summary>
        /// Return a match object with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <returns>The match object.</returns>
        public Match Match(string other)
        {
            return this.Regex.Match(other);
        }

        /// <summary>
        /// Return a match object with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <param name="startAt">Where to start matching at.</param>
        /// <returns>The match object.</returns>
        public Match Match(string other, int startAt)
        {
            return this.Regex.Match(other, startAt);
        }

        /// <summary>
        /// Return a match collection object with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <returns>The match collection object.</returns>
        public MatchCollection Matches(string other)
        {
            return this.Regex.Matches(other);
        }

        /// <summary>
        /// Return a match collection object with a given string.
        /// </summary>
        /// <param name="other">The string to match with.</param>
        /// <param name="startAt">Where to start matching at.</param>
        /// <returns>The match collection object.</returns>
        public MatchCollection Matches(string other, int startAt)
        {
            return this.Regex.Matches(other, startAt);
        }
    }
}
