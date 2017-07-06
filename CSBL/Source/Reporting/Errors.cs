using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This class contains all error types that can be encountered.
    /// </summary>
    public static class Errors
    {
        public static Error ExitPreprocessor = new Error(ErrorStage.Preprocessor, ErrorType.ExitPreprocessor, "");
        public static Error ExitTokenizer = new Error(ErrorStage.Tokenization, ErrorType.ExitTokenizer, "");
        public static Error ExitTransformer = new Error(ErrorStage.Transformation, ErrorType.ExitTransformer, "");
        public static Error ExitInterpreter = new Error(ErrorStage.Interpretation, ErrorType.ExitInterpreter, "");
        public static Error NoFileProvided = new Error(ErrorStage.Initialization, ErrorType.NoFileProvided, "No input file provided to CSBL.");
        public static Error InvalidPreprocessorToken = new Error(ErrorStage.Preprocessor, ErrorType.InvalidPreprocessorToken, "Invalid preprocessor token '{0}' at character index {1}.");
        public static Error RedefinedPreprocessorImport = new Error(ErrorStage.Preprocessor, ErrorType.RedefinedPreprocessorImport, "Redefined preprocessor import '{0}'.");
        public static Error ErrorOpeningFile = new Error(ErrorStage.Preprocessor, ErrorType.ErrorOpeningFile, "Error opening file '{0}' with #use.");
        public static Error InvalidToken = new Error(ErrorStage.Tokenization, ErrorType.InvalidToken, "Invalid token '{0}' at line {1}, column {2}.");
        public static Error UnknownToken = new Error(ErrorStage.Transformation, ErrorType.UnknownToken, "Unknown token '{0}' at line {1}, column {2}.");
        public static Error UnknownLabel = new Error(ErrorStage.Interpretation, ErrorType.UnknownLabel, "Unknown label '{0}' at line {1}, column {2}.");
        public static Error UnknownFunction = new Error(ErrorStage.Interpretation, ErrorType.UnknownFunction, "Unknown function '{0}' at line {1}, column {2}.");
        public static Error IncompatibleOperatorTypes = new Error(ErrorStage.Interpretation, ErrorType.IncompatibleOperatorTypes, "Incompatible types '{0}' and '{1}' for operator '{2}' at line {3}, column {4}.");
        public static Error EmptyCallStack = new Error(ErrorStage.Interpretation, ErrorType.EmptyCallStack, "[ret] called with empty call stack at line {0}, column {2}.");
        public static Error InvalidValue = new Error(ErrorStage.Interpretation, ErrorType.InvalidValue, "Invalid value '{0}' at line {1}, column {2}.");
        public static Error RedefinedName = new Error(ErrorStage.Interpretation, ErrorType.RedefinedName, "Redefined name '{0}' at line {1}, column {2}.");
        public static Error UndefinedName = new Error(ErrorStage.Interpretation, ErrorType.UndefinedName, "Undefined name '{0}' at line {1}, column {2}.");
        public static Error EmptyStack = new Error(ErrorStage.Interpretation, ErrorType.EmptyStack, "Attempted to pop empty stack with '{0}' at line {1}, column {2}.");
    }
}
