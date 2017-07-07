using System;

namespace CSBL.Tokenization
{
    /// <summary>
    /// This enum contains all of the token types.
    /// </summary>
    public enum TokenType
    {
        BoolLiteral,
        NumberLiteral,
        StringLiteral,
        CallFunction,
        LabelDefinition,
        LabelUsage,
        StacklessLabelDefinition,
        StacklessLabelUsage,
        Name,
    }
}
