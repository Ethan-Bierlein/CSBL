using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This enum contains all of the different errors that can be encountered
    /// at any stage of the execution of a CSBL program.
    /// </summary>
    public enum ErrorType
    {
        ExitPreprocessor,
        ExitTokenizer,
        ExitTransformer,
        ExitInterpreter,
        InvalidPreprocessorToken,
        InvalidToken,
        UnknownToken,
        UnknownLabel,
        UnknownFunction,
        IncompatibleOperatorTypes,
        EmptyCallStack,
        InvalidValue,
        RedefinedName,
        UndefinedName,
        EmptyStack
    }
}
