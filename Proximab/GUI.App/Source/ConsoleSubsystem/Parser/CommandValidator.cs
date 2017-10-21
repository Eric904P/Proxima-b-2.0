﻿using GUI.ContentDefinitions.Commands;
using System;
using System.Globalization;

namespace GUI.App.Source.ConsoleSubsystem.Parser
{
    internal class CommandValidator
    {
        public bool Validate(RawCommand command, CommandDefinition definition)
        {
            if (command.Arguments.Count != definition.Arguments.Count)
                return false;
            
            for(int i=0; i<command.Arguments.Count; i++)
            {
                var commandArgument = command.Arguments[i];
                var definitionArgument = definition.Arguments[i];

                if (!TryParseToType(commandArgument, definitionArgument.Type))
                    return false;
            }

            return true;
        }

        bool TryParseToType(string value, string type)
        {
            var result = false;

            switch (type)
            {
                case "string": { result = true; break; }
                case "int":  { result = Int32.TryParse(value, out _); break; }
                case "bool": { result = Boolean.TryParse(value, out _); break; }
                case "float": { result = Single.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _); break; }
            }

            return result;
        }
    }
}