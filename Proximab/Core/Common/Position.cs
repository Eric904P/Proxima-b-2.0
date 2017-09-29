﻿using Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position() : this(1, 1)
        {

        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;

            if (!IsValid())
                throw new PositionOutOfRangeException();
        }

        bool IsValid()
        {
            return (X >= 1 && X <= 8) && (Y >= 1 && Y <= 8);
        }
    }
}
