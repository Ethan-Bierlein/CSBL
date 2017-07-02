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
        BoolLiteral,
        NumberLiteral,
        StringLiteral,
        CallOperator,
        CallFunction,
    }
}
