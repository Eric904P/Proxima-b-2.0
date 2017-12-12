﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxima.FICS.Source.ConfigSubsystem;
using Proxima.FICS.Source.FICSModes.Exceptions;

namespace Proxima.FICS.Source.FICSModes
{
    public class FICSModeFactory
    {
        ConfigManager _configManager;

        public FICSModeFactory(ConfigManager configManager)
        {
            _configManager = configManager;
        }

        public FICSModeBase Create(FICSModeType type)
        {
            switch(type)
            {
                case FICSModeType.Auth: return new AuthMode(_configManager);
            }

            throw new FICSModeNotFoundException();
        }
    }
}
