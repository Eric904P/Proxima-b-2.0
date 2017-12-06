﻿using Proxima.Core.Boards;
using Proxima.Core.Commons.Performance;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.MoveGenerators.MagicBitboards;
using Proxima.Core.MoveGenerators.Moves;

namespace Proxima.Core.MoveGenerators
{
    /// <summary>
    /// Represents a set of methods to generating rook moves.
    /// </summary>
    public static class RookMovesGenerator
    {
        /// <summary>
        /// Generates available moves.
        /// </summary>
        /// <param name="pieceType">The piece type.</param>
        /// <param name="opt">The generator parameters.</param>
        public static void Generate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];

            while (piecesToParse != 0)
            {
                var pieceLSB = BitOperations.GetLSB(piecesToParse);
                piecesToParse = BitOperations.PopLSB(piecesToParse);

                var patternContainer = CalculateMoves(pieceType, pieceLSB, opt);
                CalculateAttacks(pieceType, pieceLSB, patternContainer, opt);
            }
        }

        private static ulong CalculateMoves(PieceType pieceType, ulong pieceLSB, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateMoves) == 0)
            {
                return 0;
            }

            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);
            var piecePosition = BitPositionConverter.ToPosition(pieceIndex);

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, opt.OccupancySummary);
            pattern &= ~opt.FriendlyOccupancy;

            var excludeFromAttacks = pattern;
            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);
                    
                var to = BitPositionConverter.ToPosition(patternIndex);

                if ((patternLSB & opt.EnemyOccupancy) == 0)
                {
                    opt.Bitboard.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.Bitboard.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Bitboard.Attacks[patternIndex] |= pieceLSB;
                    opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
                }
            }

            return excludeFromAttacks;
        }

        private static void CalculateAttacks(PieceType pieceType, ulong pieceLSB, ulong movesPattern, GeneratorParameters opt)
        {
            if ((opt.Mode & GeneratorMode.CalculateAttacks) == 0)
            {
                return;
            }

            var blockersToRemove = opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] |
                                   opt.Bitboard.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var occupancyWithoutBlockers = opt.OccupancySummary & ~blockersToRemove;          
            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, occupancyWithoutBlockers);
            pattern ^= movesPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Bitboard.Attacks[patternIndex] |= pieceLSB;
                opt.Bitboard.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }
    }
}
