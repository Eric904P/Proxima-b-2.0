﻿using GUI.App.Source.BoardSubsystem;
using GUI.App.Source.BoardSubsystem.Pieces;
using GUI.App.Source.ConsoleSubsystem;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.InputSubsystem;
using GUI.App.Source.PromotionSubsystem;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using Proxima.Helpers.Persistence;
using System;
using System.Collections.Generic;

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
            if(!_promotionWindow.Active)
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

        protected void UpdateBitBoard(FriendlyBoard friendlyBoard)
        {
            _bitBoard = new BitBoard(friendlyBoard);
            _bitBoard.Calculate();

            _visualBoard.SetFriendlyBoard(_bitBoard.GetFriendlyBoard());
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.Occupancy: { DrawOccupancy(command); break; }
                case CommandType.Attacks: { DrawAttacks(command); break; }
                case CommandType.SaveBoard: { SaveBoard(command); break; }
                case CommandType.LoadBoard: { LoadBoard(command); break; }
                case CommandType.IsCheck: { IsCheck(command); break; }
            }
        }

        void DrawOccupancy(Command command)
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

        void DrawAttacks(Command command)
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

        void SaveBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardWriter = new BoardWriter();
            var board = _visualBoard.GetFriendlyBoard();

            var path = $"Boards\\{boardNameArgument}.board";
            boardWriter.Write(path, board);
        }

        void LoadBoard(Command command)
        {
            var boardNameArgument = command.GetArgument<string>(0);

            var boardReader = new BoardReader();
            var path = $"Boards\\{boardNameArgument}.board";

            if (!boardReader.BoardExists(path))
            {
                _consoleManager.WriteLine($"$rBoard {path} not found");
                return;
            }

            UpdateBitBoard(boardReader.Read(path));
        }

        void IsCheck(Command command)
        {
            var colorArgument = command.GetArgument<string>(0);
            var colorType = Color.White;

            if (colorArgument == "white" || colorArgument == "w")
            {
                colorType = Color.White;
            }
            else if (colorArgument == "black" || colorArgument == "b")
            {
                colorType = Color.Black;
            }
            else
            {
                _consoleManager.WriteLine($"$rInvalid color name");
                return;
            }

            if (_bitBoard.IsCheck(colorType))
            {
                _consoleManager.WriteLine($"$gYES");
            }
            else
            {
                _consoleManager.WriteLine($"$rNO");
            }
        }
    }
}
