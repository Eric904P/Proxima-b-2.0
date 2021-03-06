﻿using System.Diagnostics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.MoveGenerators;
using Proxima.Core.Persistence;

namespace Proxima.Core.Tests
{
    /// <summary>
    /// Represents a set of methods for testing speed of move generators and evaluation functions.
    /// </summary>
    public class MovesTest
    {
        private readonly Color _initialColor = Color.White;

        /// <summary>
        /// Runs moves test with specific board and depth (0 = one level, 1 = two levels, ...).
        /// If verifyIntegrity is false, then flag in returned MovesTestData object will be
        /// always true.
        /// </summary>
        /// <param name="friendlyBoard">Initial board from which test will begin.</param>
        /// <param name="depth">Number of in-depth nodes (where 0 means calculating moves only for initial board.</param>
        /// <param name="calculateEndNodes">If true, every end node will calculate attacks and evaluation function.</param>
        /// <param name="verifyIntegrity">If true, every board will be checked whether incremental-updating parameters are correctly calculated.</param>
        /// <returns>Result of the test.</returns>
        public MovesTestData Run(FriendlyBoard friendlyBoard, int depth, bool calculateEndNodes, bool verifyIntegrity)
        {
            var testData = new MovesTestData();
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            CalculateBitboard(_initialColor, new Bitboard(friendlyBoard), depth, calculateEndNodes, verifyIntegrity, testData);
            testData.Ticks = stopwatch.Elapsed.Ticks;

            return testData;
        }

        /// <summary>
        /// Recursive method for calculating bitboard. If depth is equal or less than zero, then
        /// current node is the last and next CalculateBitboard call will not be executed.
        /// </summary>
        /// <param name="color">Color of the current player.</param>
        /// <param name="bitboard">Currently calculated bitboard</param>
        /// <param name="depth">Current depth</param>
        /// <param name="calculateEndNodes">If true, every end node will calculate attacks and evaluation function.</param>
        /// <param name="verifyIntegrity">If true, every board will be checked whether incremental-updating parameters are correctly calculated.</param>
        /// <param name="testData">Container for test data which will be returned when test is done.</param>
        private void CalculateBitboard(Color color, Bitboard bitboard, int depth, bool calculateEndNodes, bool verifyIntegrity, MovesTestData testData)
        {
            if (verifyIntegrity)
            {
                bitboard.Calculate(GeneratorMode.CalculateAttacks, false);
                if (!bitboard.VerifyIntegrity())
                {
                    testData.Integrity = false;

                    var boardWriter = new BoardWriter();
                    boardWriter.Write("Boards/error.board", new FriendlyBoard(bitboard));
                }
            }

            if (depth <= 0)
            {
                if (calculateEndNodes)
                {
                    bitboard.Calculate(GeneratorMode.CalculateAttacks, false);
                    bitboard.GetEvaluation();
                }

                testData.EndNodes++;
            }
            else
            {
                var enemyColor = ColorOperations.Invert(color);
                var whiteMode = GetGeneratorMode(color);
                var blackMode = GetGeneratorMode(enemyColor);

                bitboard.Calculate(whiteMode, blackMode, false);

                foreach (var move in bitboard.Moves)
                {
                    var bitboardAfterMove = bitboard.Move(move);

                    CalculateBitboard(enemyColor, bitboardAfterMove, depth - 1, calculateEndNodes, verifyIntegrity, testData);
                }
            }

            testData.TotalNodes++;
        }

        /// <summary>
        /// Calculates generator mode for the specific color.If current color and initial color are
        /// the same, then returned enum will have flags for calculating moves and attacks. Otherwise,
        /// it will have only attacks flag.
        /// </summary>
        /// <param name="currentColor">Color of the current player.</param>
        /// <returns>Generator mode for the specified color.</returns>
        private GeneratorMode GetGeneratorMode(Color currentColor)
        {
            return currentColor == Color.White && currentColor == _initialColor ?
                GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks :
                GeneratorMode.CalculateAttacks;
        }
    }
}
