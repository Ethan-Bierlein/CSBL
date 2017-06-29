﻿using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This class contains all error types that can be encountered.
    /// </summary>
    public static class Errors
    {
        public static Error InvalidToken = new Error(ErrorStage.Tokenization, ErrorType.InvalidToken, "Invalid token '{0}' at line {1}, column {2}.");
        public static Error UnknownToken = new Error(ErrorStage.Transformation, ErrorType.UnknownToken, "Unknown token '{0}' at line {1}, column {2}.");
        public static Error MisplacedTypeSeparator = new Error(ErrorStage.Transformation, ErrorType.MisplacedTypeSeparator, "Misplaced separator at line {1}, column {2}.");
        public static Error MisplacedArrayEnd = new Error(ErrorStage.Transformation, ErrorType.MisplacedArrayEnd, "Misplaced end at line {1}, column {2}.");
        public static Error MissingArrayEnd = new Error(ErrorStage.Transformation, ErrorType.MissingArrayEnd, "Array close for array at line {1}, columnn {2} not found.");
    }
}
