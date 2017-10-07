﻿using Core.Boards.MoveGenerators;
using Core.Commons;
using Core.Commons.Colors;
using Core.Commons.Moves;
using System.Collections.Generic;

namespace Core.Boards.MoveParsers
{
    public class KingMovesParser
    {
        BitBoard _bitBoard;

        public KingMovesParser(BitBoard bitBoard)
        {
            _bitBoard = bitBoard;
        }

        public List<Move> GetMoves(Color color)
        {
            var moves = new List<Move>();

            var friendlyOccupation = _bitBoard.Occupancy[(int)color];
            var enemyOccupation = _bitBoard.Occupancy[(int)ColorOperations.Invert(color)];

            var pieces = _bitBoard.Pieces[(int)color, (int)PieceType.King];

            while (pieces != 0)
            {
                var pieceLSB = BitOperations.GetLSB(ref pieces);
                var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

                var pattern = PredefinedMoves.King[pieceIndex] & ~_bitBoard.Occupancy[(int)color];

                while (pattern != 0)
                {
                    var patternLSB = BitOperations.GetLSB(ref pattern);
                    var patternIndex = BitOperations.GetBitIndex(patternLSB);

                    var from = BitPositionConverter.ToPosition(pieceLSB);
                    var to = BitPositionConverter.ToPosition(patternLSB);
                    var moveType = GetMoveType(patternLSB, enemyOccupation);

                    var move = new Move(from, to, color, moveType);
                    moves.Add(move);

                    _bitBoard.Attacks[(int)color, patternIndex] |= pieceLSB;
                }
            }

            return moves;
        }

        MoveType GetMoveType(ulong patternLSB, ulong enemyOccupation)
        {
            if((patternLSB & enemyOccupation) != 0)
            {
                return MoveType.Kill;
            }

            return MoveType.Quiet;
        }
    }
}
