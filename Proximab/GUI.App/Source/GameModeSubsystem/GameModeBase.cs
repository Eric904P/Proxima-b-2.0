﻿using System;
using System.Collections.Generic;
using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.InputSubsystem;
using GUI.App.Source.PromotionSubsystem;
using GUI.ColorfulConsole;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Boards.Friendly.Persistence;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Moves;
using Proxima.Core.Commons.Positions;
using Proxima.Core.MoveGenerators;

namespace GUI.App.Source.GameModeSubsystem
{
    internal abstract class GameModeBase
    {
        protected ConsoleManager _consoleManager;
        protected PiecesProvider _piecesProvider;

        protected VisualBoard _visualBoard;
        protected PromotionWindow _promotionWindow;

        protected BitBoard _bitBoard;

        public GameModeBase(ConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
            _consoleManager.OnNewCommand += ConsoleManager_OnNewCommand;

            _piecesProvider = new PiecesProvider();

            _visualBoard = new VisualBoard(_piecesProvider);
            _promotionWindow = new PromotionWindow(_piecesProvider);
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _piecesProvider.LoadContent(contentManager);
            _visualBoard.LoadContent(contentManager);
            _promotionWindow.LoadContent(contentManager);
        }

        public virtual void Input(InputManager inputManager)
        {
            if (!_promotionWindow.Active)
            {
                _visualBoard.Input(inputManager);
            }

            _promotionWindow.Input(inputManager);
        }

        public virtual void Logic()
        {
            _visualBoard.Logic();
            _promotionWindow.Logic();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            _visualBoard.Draw(spriteBatch);
            _promotionWindow.Draw(spriteBatch);
        }

        protected void CalculateBitBoard(FriendlyBoard friendlyBoard)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(friendlyBoard, mode, mode);
        }

        protected void CalculateBitBoard(Move move)
        {
            var mode = GeneratorMode.CalculateMoves | GeneratorMode.CalculateAttacks;
            CalculateBitBoard(move, mode, mode);
        }

        protected void CalculateBitBoard(FriendlyBoard friendlyBoard, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            _bitBoard = new BitBoard(friendlyBoard);
            _bitBoard.Calculate(whiteMode, blackMode);

            _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }

        protected void CalculateBitBoard(Move move, GeneratorMode whiteMode, GeneratorMode blackMode)
        {
            _bitBoard = _bitBoard.Move(move);
            _bitBoard.Calculate(whiteMode, blackMode);

            _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }

        private void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.Attacks: { DrawAttacks(command); break; }
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.Check: { DisplayCheckStatus(command); break; }
                case CommandType.Castling: { DisplayCastlingFlags(command); break; }
                case CommandType.Evaluation: { DisplayEvaluation(command); break; }
                case CommandType.Hash: { DisplayBoardHash(command); break; }
            }
        }

        private void DrawOccupancy(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> occupancy;
            if (colorArgument == "all")
            {
                occupancy = _visualBoard.GetFriendlyBoard().GetOccupancy();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                occupancy = _visualBoard.GetFriendlyBoard().GetOccupancy(colorType);
            }

            _visualBoard.AddExternalSelections(occupancy);
        }

        private void DrawAttacks(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);

            List<Position> attacks;
            if (colorArgument == "all")
            {
                attacks = _visualBoard.GetFriendlyBoard().GetAttacks();
            }
            else
            {
                var colorTypeParseResult = Enum.TryParse(colorArgument, true, out Color colorType);
                if (!colorTypeParseResult)
                {
                    _consoleManager.WriteLine($"$rInvalid color parameter ($R{colorArgument}$r)");
                    return;
                }

                attacks = _visualBoard.GetFriendlyBoard().GetAttacks(colorType);
            }

            _visualBoard.AddExternalSelections(attacks);
        }

        private void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = _visualBoard.GetFriendlyBoard();

            var path = $"Boards\\{boardNameArgument}.board";
            boardWriter.Write(path, board);
        }

        private void LoadBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardReader = new BoardReader();
            var path = $"Boards\\{boardNameArgument}.board";

            if (!boardReader.BoardExists(path))
            {
                _consoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            CalculateBitBoard(boardReader.Read(path));
        }

        private void DisplayCheckStatus(Command command)
        {
            var whiteCheck = _bitBoard.IsCheck(Color.White);
            var blackCheck = _bitBoard.IsCheck(Color.Black);

            _consoleManager.WriteLine($"$cWhite king checked: ${ColorfulConsoleHelpers.ParseBool(whiteCheck)}");
            _consoleManager.WriteLine($"$cBlack king checked: ${ColorfulConsoleHelpers.ParseBool(blackCheck)}");
        }

        private void DisplayCastlingFlags(Command command)
        {
            var castlingFlags = _visualBoard.GetFriendlyBoard().Castling;

            _consoleManager.WriteLine($"$cWhite short possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteShortCastlingPossibility)}");
            _consoleManager.WriteLine($"$cWhite long possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteLongCastlingPossibility)}");
            _consoleManager.WriteLine($"$cBlack short possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackShortCastlingPossibility)}");
            _consoleManager.WriteLine($"$cBlack long possibility: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackLongCastlingPossibility)}");

            _consoleManager.WriteLine($"$cWhite done: {ColorfulConsoleHelpers.ParseBool(castlingFlags.WhiteCastlingDone)}");
            _consoleManager.WriteLine($"$cBlack done: {ColorfulConsoleHelpers.ParseBool(castlingFlags.BlackCastlingDone)}");
        }

        private void DisplayEvaluation(Command command)
        {
            var evaluation = _bitBoard.GetDetailedEvaluation();

            _consoleManager.WriteLine($"$c\t\tWhite\tBlack");
            _consoleManager.WriteLine($"$cMaterial:\t$w{evaluation.Material.WhiteMaterial}\t{evaluation.Material.BlackMaterial}");
            _consoleManager.WriteLine($"$cMobility:\t$w{evaluation.Mobility.WhiteMobility}\t{evaluation.Mobility.BlackMobility}");
            _consoleManager.WriteLine($"$cCastling:\t$w{evaluation.Castling.WhiteCastling}\t{evaluation.Castling.BlackCastling}");
            _consoleManager.WriteLine($"$cPosition:\t$w{evaluation.Position.WhitePosition}\t{evaluation.Position.BlackPosition}");
            _consoleManager.WriteLine($"$cDoubled pawns:\t$w{evaluation.PawnStructure.WhiteDoubledPawns}\t{evaluation.PawnStructure.BlackDoubledPawns}");
            _consoleManager.WriteLine($"$cIsolated pawns:\t$w{evaluation.PawnStructure.WhiteIsolatedPawns}\t{evaluation.PawnStructure.BlackIsolatedPawns}");
            _consoleManager.WriteLine($"$cPawn chain:\t$w{evaluation.PawnStructure.WhitePawnChain}\t{evaluation.PawnStructure.BlackPawnChain}");
            _consoleManager.WriteLine($"$cKing neighbrs:\t$w{evaluation.KingSafety.WhiteAttackedNeighbours}\t{evaluation.KingSafety.BlackAttackedNeighbours}");

            _consoleManager.WriteLine();
            _consoleManager.WriteLine($"$cTotal: $w{evaluation.Total}");
        }

        private void DisplayBoardHash(Command command)
        {
            var trueHash = _bitBoard.Hash.ToString();
            _consoleManager.WriteLine($"$c{trueHash}");
        }
    }
}
