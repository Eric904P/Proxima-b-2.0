﻿using System;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace CECP.App.GameSubsystem.Modes.Game.Moves
{
    /// <summary>
    /// Represents a set of methods to parse CECP moves.
    /// </summary>
    public class CecpMoveParser
    {
        /// <summary>
        /// Parses move in text format to CECP move object.
        /// </summary>
        /// <param name="moveText">The move in text format to parse.</param>
        /// <returns>The CECP move object.</returns>
        public CecpMove Parse(string moveText)
        {
            if (moveText.Length == 4)
            {
                return ParseNormalMove(moveText);
            }
            else if (moveText.Length == 5)
            {
                return ParsePromotionMove(moveText);
            }

            return null;
        }

        /// <summary>
        /// Parses normal (not promoting) move to CECP move object.
        /// </summary>
        /// <param name="moveText">The move in text format to parse.</param>
        /// <returns>The CECP move object.</returns>
        private CecpMove ParseNormalMove(string moveText)
        {
            var fromText = moveText.Substring(0, 2);
            var toText = moveText.Substring(2, 2);

            var fromPosition = PositionConverter.ToPosition(fromText);
            var toPosition = PositionConverter.ToPosition(toText);

            return new CecpMove(fromPosition, toPosition);
        }

        /// <summary>
        /// Parses promoting move to CECP move object.
        /// </summary>
        /// <param name="moveText">The move in text format to parse.</param>
        /// <returns>The CECP move object.</returns>
        private CecpMove ParsePromotionMove(string moveText)
        {
            var fromText = moveText.Substring(0, 2);
            var toText = moveText.Substring(2, 2);
            var promotionPieceSymbol = Convert.ToChar(moveText.Substring(4, 1));

            var fromPosition = PositionConverter.ToPosition(fromText);
            var toPosition = PositionConverter.ToPosition(toText);
            var promotionPieceType = PieceConverter.GetPiece(promotionPieceSymbol);

            return new CecpMove(fromPosition, toPosition, promotionPieceType);
        }
    }
}
