﻿using System;
using GUI.App.Source;
using GUI.App.Source.ConsoleSubsystem;
using Proxima.Core;

namespace GUI.App
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            var consoleManager = new ConsoleManager();
            consoleManager.Run();

            ProximaCore.Init();

            using (var game = new GUICore(consoleManager))
            {
                game.Run();
            }
        }
    }
}
