using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This enum contains all of the different errors that can be encountered
    /// at any stage of the execution of a CSBL program.
    /// </summary>
    public enum ErrorType
    {
        InvalidToken,
        UnknownToken,
        MisplacedTypeSeparator,
        MisplacedArrayEnd,
        MissingArrayEnd,
        UnknownFunction,
        UnknownOperator,
        UnknownCustomFunction,
        UnexpectedToken,
        RuntimeUnbalancedParentheses,
        InvalidArrayIndex,
        IncompatibleOperatorTypes
    }
}
