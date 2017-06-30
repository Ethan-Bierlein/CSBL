using System;

namespace CSBL.Transformation
{
    /// <summary>
    /// This enumeration contains all the transformed token types.
    /// </summary>
    public enum TransformedTokenType
    {
        CodeBlockOpen,
        CodeBlockClose,
        Type,
        TypedName,
        UntypedName,
        Array,
        String,
        Number,
        CallOperator,
        CallFunction,
        CallCustomFunction
    }
}
