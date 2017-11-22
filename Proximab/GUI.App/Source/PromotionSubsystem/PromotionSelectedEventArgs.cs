﻿using System;
using Proxima.Core.Commons.Moves;

namespace GUI.App.Source.PromotionSubsystem
{
    internal class PromotionSelectedEventArgs : EventArgs
    {
        public PromotionMove Move { get; private set; }

        public PromotionSelectedEventArgs(PromotionMove move)
        {
            Move = move;
        }
    }
}
