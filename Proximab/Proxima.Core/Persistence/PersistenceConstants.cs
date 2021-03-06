﻿using System.Diagnostics.CodeAnalysis;

namespace Proxima.Core.Persistence
{
    /// <summary>
    /// Represents a set of constants for internal board file format.
    /// </summary>
    [SuppressMessage("ReSharper", "MissingXmlDoc")]
    public static class PersistenceConstants
    {
        public const string BoardSection = "!Board";
        public const string CastlingSection = "!Castling";
        public const string EnPassantSection = "!EnPassant";

        public const string EmptyBoardField = "--";
        public const string NullValue = "Null";
    }
}
