﻿using System;

namespace CSBL.Reporting
{
    /// <summary>
    /// This enum contains all the different stages a CSBL program can go
    /// through. This is used for error reporting.
    /// </summary>
    public enum ErrorStage
    {
        Tokenization,
        Transformation,
        PreEvaluation,
        Evaluation
    }
}
