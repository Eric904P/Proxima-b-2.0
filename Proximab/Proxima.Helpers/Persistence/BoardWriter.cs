﻿using Proxima.Core.Boards;
using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Positions;
using System;
using System.Linq;
using System.IO;
using Proxima.Core.Commons.Notation;

namespace Proxima.Helpers.BoardSubsystem.Persistence
{
    public class BoardWriter
    {
        public BoardWriter()
        {

        }

        public void Write(string path, FriendlyBoard friendlyBoard)
        {
            using (var writer = new StreamWriter(path))
            {
                WriteBoard(writer, friendlyBoard.Pieces);
                writer.WriteLine();
                WriteCastling(writer, friendlyBoard.Castling);
                writer.WriteLine();
                WriteEnPassant(writer, friendlyBoard.EnPassant);
            }
        }

        void WriteBoard(StreamWriter writer, FriendlyPiecesList pieces)
        {
            writer.WriteLine("!Board");
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = pieces.FirstOrDefault(p => p.Position == new Position(x + 1, 8 - y));

                    if (field == null)
                    {
                        writer.Write("00");
                    }
                    else
                    {
                        writer.Write(ColorOperations.GetSymbol(field.Color));
                        writer.Write(PieceOperations.GetSymbol(field.Type));
                    }

                    writer.Write(" ");
                }

                writer.WriteLine();
            }
        }

        void WriteCastling(StreamWriter writer, FriendlyCastling castling)
        {
            writer.WriteLine("!Castling");

            writer.WriteLine(castling.WhiteShortCastling.ToString());
            writer.WriteLine(castling.WhiteLongCastling.ToString());
            writer.WriteLine(castling.BlackShortCastling.ToString());
            writer.WriteLine(castling.BlackLongCastling.ToString());
        }

        void WriteEnPassant(StreamWriter writer, FriendlyEnPassant enPassant)
        {
            writer.WriteLine("!EnPassant");

            WritePosition(writer, enPassant.WhiteEnPassant);
            WritePosition(writer, enPassant.BlackEnPassant);
        }

        void WritePosition(StreamWriter writer, Position position)
        {
            if(position == null)
            {
                writer.WriteLine("NULL");
                return;
            }

            writer.WriteLine(NotationConverter.ToString(position));
        }
    }
}
