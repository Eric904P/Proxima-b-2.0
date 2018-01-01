﻿using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Evaluation.PawnStructure
{
    /// <summary>
    /// Represents a set of evaluation parameters for pawn structure evaluation calculators.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class PawnStructureValues
    {
        public static readonly int[] DoubledPawnsRatio =
        {
            -30,  // Regular
            -5    // End
        };

        public static readonly int[] IsolatedPawnsRatio =
        {
            -30,  // Regular
            -5    // End
        };

        public static readonly int[] PawnChainRatio =
        {
            10,  // Regular
            5    // End
        };
    }
}
