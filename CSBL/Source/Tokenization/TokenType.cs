using System;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This enum contains all of the token types.
    /// </summary>
    public enum TokenType
    {
        CodeBlockOpen,
        CodeBlockClose,

        Type,
        Name,
        TypeNameSeparator,

        NumberLiteral,
        StringLiteral,
        ArrayOpenLiteral,
        ArrayCloseLiteral,
        ArrayValueSeparator,

        CallOperator,
        CallFunction,
        CallCustomFunction
    }
}
