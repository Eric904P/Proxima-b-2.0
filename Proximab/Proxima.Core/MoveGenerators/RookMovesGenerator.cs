﻿using Proxima.Core.Boards;
using Proxima.Core.Commons;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Performance;
using Proxima.Core.MoveGenerators.MagicBitboards;

namespace Proxima.Core.MoveGenerators
{
    public static class RookMovesGenerator
    {
        public static void Calculate(PieceType pieceType, GeneratorParameters opt)
        {
            var piecesToParse = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, pieceType)];

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

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, opt.Occupancy);
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
                    opt.Moves.AddLast(new QuietMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }
                else
                {
                    opt.Moves.AddLast(new KillMove(piecePosition, to, pieceType, opt.FriendlyColor));
                }

                if ((opt.Mode & GeneratorMode.CalculateAttacks) != 0)
                {
                    opt.Attacks[patternIndex] |= pieceLSB;
                    opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
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

            var blockersToRemove = opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Rook)] |
                                   opt.Pieces[FastArray.GetPieceIndex(opt.FriendlyColor, PieceType.Queen)];

            var occupancyWithoutBlockers = opt.Occupancy & ~blockersToRemove;          
            var pieceIndex = BitOperations.GetBitIndex(pieceLSB);

            var pattern = MagicContainer.GetRookAttacks(pieceIndex, occupancyWithoutBlockers);
            pattern ^= movesPattern;

            while (pattern != 0)
            {
                var patternLSB = BitOperations.GetLSB(pattern);
                pattern = BitOperations.PopLSB(pattern);

                var patternIndex = BitOperations.GetBitIndex(patternLSB);

                opt.Attacks[patternIndex] |= pieceLSB;
                opt.AttacksSummary[(int)opt.FriendlyColor] |= patternLSB;
            }
        }
    }
}
