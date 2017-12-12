﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.FICSModes.Exceptions;

namespace Proxima.FICS.Source.FICSModes
{
    /// <summary>
    /// Represents a factory of FICS modes.
    /// </summary>
    public class FICSModeFactory
    {
        private ConfigManager _configManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="FICSModeFactory"/> class.
        /// </summary>
        /// <param name="configManager">The configuration manager.</param>
        public FICSModeFactory(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        /// <summary>
        /// Creates a new instance of the FICS mode specified in the parameter.
        /// </summary>
        /// <param name="type">The FICS mode.</param>
        /// <returns>The FICS mode instance.</returns>
        public FICSModeBase Create(FICSModeType type)
        {
            switch (type)
            {
                case FICSModeType.Auth: return new AuthMode(_configManager);
                case FICSModeType.Seek: return new SeekMode(_configManager);
                case FICSModeType.Game: return new GameMode(_configManager);
            }

            throw new FICSModeNotFoundException();
        }
    }
}
