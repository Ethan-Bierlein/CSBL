using System;

namespace CSBL.Transformation
{
    /// <summary>
    /// This enumeration contains all the transformed token types.
    /// </summary>
    public enum TransformedTokenType
    {
        Bool,
        String,
        Number,
        CallFunction,
        LabelDefinition,
        LabelUsage
    }
}
