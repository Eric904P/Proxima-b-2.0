﻿using GUI.App.CommandsSubsystem;
using GUI.App.ConsoleSubsystem;
using GUI.App.GameSubsystem.Exceptions;
using GUI.App.GameSubsystem.Modes;

namespace GUI.App.GameSubsystem
{
    /// <summary>
    /// Represents a factory of modes.
    /// </summary>
    public class GameModeFactory
    {
        private ConsoleManager _consoleManager;
        private CommandsManager _commandsManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameModeFactory"/> class.
        /// </summary>
        /// <param name="consoleManager">The console manager.</param>
        /// <param name="commandsManager">The commands manager.</param>
        public GameModeFactory(ConsoleManager consoleManager, CommandsManager commandsManager)
        {
            _consoleManager = consoleManager;
            _commandsManager = commandsManager;
        }

        /// <summary>
        /// Creates a new instance of the mode specified in the parameter.
        /// </summary>
        /// <param name="modeType">The mode type.</param>
        /// <returns>The mode instance.</returns>
        public GameModeBase Create(GameModeType modeType)
        {
            switch (modeType)
            {
                case GameModeType.Editor: return new EditorMode(_consoleManager, _commandsManager);
                case GameModeType.AIvsAi: return new AiVsAiMode(_consoleManager, _commandsManager);
            }

            throw new GameModeNotFoundException();
        }
    }
}
