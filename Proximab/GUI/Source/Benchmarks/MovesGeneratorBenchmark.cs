﻿using Core.Boards;
using Core.Commons.Colors;
using GUI.Source.ConsoleSubsystem;
using System;
using System.Diagnostics;

namespace GUI.Source.Benchmarks
{
    internal class MovesGeneratorBenchmark
    {
        ConsoleManager _consoleManager;

        public MovesGeneratorBenchmark(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
        }

        public void Run(Color initialColor, FriendlyBoard friendlyBoard, int depth, bool verifyChecks)
        {
            var benchmarkData = new BenchmarkData();
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var freshBitBoard = new BitBoard(friendlyBoard);
            CalculateBitBoard(initialColor, freshBitBoard, depth, verifyChecks, benchmarkData);

            benchmarkData.Ticks = stopwatch.Elapsed.Ticks;

            DisplayBenchmarkResult(benchmarkData);
        }

        void CalculateBitBoard(Color color, BitBoard bitBoard, int depth, bool verifyChecks, BenchmarkData benchmarkData)
        {
            var enemyColor = ColorOperations.Invert(color);

            if (depth <= 0)
            {
                bitBoard.Calculate(CalculationMode.OnlyAttacks);
                benchmarkData.EndNodes++;
            }
            else
            {
                if(color == Color.White)
                {
                    bitBoard.Calculate(CalculationMode.WhiteMovesPlusAttacks);
                }
                else
                {
                    bitBoard.Calculate(CalculationMode.BlackMovesPlusAttacks);
                }

                var availableMoves = bitBoard.GetAvailableMoves();
                foreach (var move in availableMoves)
                {
                    var bitBoardAfterMove = bitBoard.Move(move);
                    if (verifyChecks && bitBoardAfterMove.IsCheck(color))
                        continue;

                    CalculateBitBoard(enemyColor, bitBoardAfterMove, depth - 1, verifyChecks, benchmarkData);
                }
            }

            benchmarkData.TotalNodes++;
        }

        void DisplayBenchmarkResult(BenchmarkData benchmarkData)
        {
            _consoleManager.WriteLine("");
            _consoleManager.WriteLine("$wBenchmark result:");
            _consoleManager.WriteLine($"$wTotal nodes: $g{benchmarkData.TotalNodes} N");
            _consoleManager.WriteLine($"$wEnd nodes: $g{benchmarkData.EndNodes} N");
            _consoleManager.WriteLine($"$wNodes per second: $c{benchmarkData.NodesPerSecond / 1000} kN");
            _consoleManager.WriteLine($"$wTime per node: $c{benchmarkData.TimePerNode} ns");
            _consoleManager.WriteLine($"$wTime: $m{benchmarkData.Time} s");
            _consoleManager.WriteLine("");
        }
    }
}