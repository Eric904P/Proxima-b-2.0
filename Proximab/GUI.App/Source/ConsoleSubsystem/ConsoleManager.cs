﻿using GUI.ContentDefinitions.Colors;
using GUI.ContentDefinitions.Commands;
using GUI.App.Source.ConsoleSubsystem.Parser;
using GUI.App.Source.DiagnosticSubsystem;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;
using System.Threading.Tasks;
using ColorfulConsole;

namespace GUI.App.Source.ConsoleSubsystem
{
    internal class ConsoleManager
    {
        public event EventHandler<NewCommandEventArgs> OnNewCommand;

        ColorfulConsoleManager _colorfulConsole;

        Task _consoleLoop;
        CommandParser _commandParser;
        CommandValidator _commandValidator;
        EnvironmentInfoProvider _environmentInfoProvider;

        CommandDefinitionsContainer _commandDefinitionsContainer;
        ColorDefinitionsContainer _colorDefinitionsContainer;

        public ConsoleManager()
        {
            _consoleLoop = new Task(() => Loop());

            OnNewCommand += ConsoleManager_OnNewCommand;

            _colorfulConsole = new ColorfulConsoleManager();
            _commandParser = new CommandParser();
            _commandValidator = new CommandValidator();
            _environmentInfoProvider = new EnvironmentInfoProvider();
        }        

        public void LoadContent(ContentManager contentManager)
        {
            _commandDefinitionsContainer = contentManager.Load<CommandDefinitionsContainer>("XML\\CommandDefinitions");
            _colorDefinitionsContainer = contentManager.Load<ColorDefinitionsContainer>("XML\\ColorDefinitions");

            _colorfulConsole.LoadContent(_colorDefinitionsContainer);

            WriteConsoleHeader();
        }

        public void Run()
        {
            _consoleLoop.Start();
        }

        public void WriteLine()
        {
            _colorfulConsole.WriteLine();
        }

        public void WriteLine(string output)
        {
            _colorfulConsole.WriteLine(output);
        }

        void ConsoleManager_OnNewCommand(object sender, NewCommandEventArgs e)
        {
            var command = e.Command;

            switch (command.Type)
            {
                case CommandType.Help: { WriteCommandsList(); break; }
                case CommandType.Colors: { WriteColorsList(); break; }
            }
        }

        void WriteCommandsList()
        {
            WriteLine($"$wAvailable commands ({_commandDefinitionsContainer.Definitions.Count}):");

            foreach (var commandDefinition in _commandDefinitionsContainer.Definitions)
            {
                WriteLine($"$g{commandDefinition.Name}$w - {commandDefinition.Description}");

                foreach (var argument in commandDefinition.Arguments)
                {
                    WriteLine($"$c  <{argument.Type}>$w - {argument.Description}");
                }
            }
        }

        void WriteColorsList()
        {
            WriteLine($"$wAvailable colors ({_colorDefinitionsContainer.Definitions.Count}):");

            foreach (var colorDefinition in _colorDefinitionsContainer.Definitions)
            {
                WriteLine($"$w - ${colorDefinition.Symbol}{colorDefinition.Color} - {colorDefinition.Symbol}");
            }
        }

        void Loop()
        {
            while(true)
            {
                ProcessCommand(Console.ReadLine());
            }
        }

        void ProcessCommand(string input)
        {
            var rawCommand = _commandParser.Parse(input.ToLower().Trim());
            if(rawCommand == null)
            {
                WriteEmptyCommandMessage();
                return;
            }

            var definition = _commandDefinitionsContainer.Definitions
                .FirstOrDefault(p => p.Name.ToLower().Trim() == rawCommand.Name);

            if(definition == null)
            {
                WriteCommandNotFoundMessage(input);
                return;
            }

            var validationResult = _commandValidator.Validate(rawCommand, definition);
            if(!validationResult)
            {
                WriteInvalidCommandFormatMessage(input);
                return;
            }

            var enumType = (CommandType)Enum.Parse(typeof(CommandType), definition.EnumType);

            var command = new Command(enumType, rawCommand.Arguments);
            var commandEventArgs = new NewCommandEventArgs(DateTime.Now, command);

            OnNewCommand?.Invoke(this, commandEventArgs);
        }

        void WriteEmptyCommandMessage()
        {
            WriteLine("$rEmpty command");
        }

        void WriteCommandNotFoundMessage(string command)
        {
            WriteLine($"$rCommand not found: {command}");
        }

        void WriteInvalidCommandFormatMessage(string command)
        {
            WriteLine($"$rInvalid command format: {command}");
        }

        void WriteConsoleHeader()
        {
            var osInfo = _environmentInfoProvider.GetOSInfo();
            var cpuPlatform = _environmentInfoProvider.GetCPUPlatformVersion();
            var processPlatform = _environmentInfoProvider.GetProcessPlatformVersion();
            var coresCount = _environmentInfoProvider.GetCPUCoresCount();

            WriteLine($"$gProxima b 2.0 GUI$w | {osInfo} (CPU $c{cpuPlatform}$w, {coresCount}$w | Process $c{processPlatform}$w)");
        }
    }
}