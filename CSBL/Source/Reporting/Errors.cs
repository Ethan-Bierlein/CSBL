using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This class contains all error types that can be encountered.
    /// </summary>
    public static class Errors
    {
        public static Error InvalidToken = new Error(ErrorStage.Tokenization, ErrorType.InvalidToken, "Invalid token '{0}' at line {1}, column {2}.");
    }
}
