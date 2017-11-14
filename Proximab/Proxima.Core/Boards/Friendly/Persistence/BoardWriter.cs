﻿using Proxima.Core.Boards.Friendly;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Notation;
using Proxima.Core.Commons.Positions;
using System.IO;
using System.Linq;

namespace Proxima.Core.Boards.Friendly.Persistence
{
    public class BoardWriter
    {
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
            writer.WriteLine(PersistenceContants.BoardSection);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    var field = pieces.FirstOrDefault(p => p.Position == new Position(x + 1, 8 - y));

                    if (field == null)
                    {
                        writer.Write(PersistenceContants.EmptyBoardField);
                    }
                    else
                    {
                        writer.Write(ColorConverter.GetSymbol(field.Color));
                        writer.Write(PieceConverter.GetSymbol(field.Type));
                    }

                    writer.Write(" ");
                }

                writer.WriteLine();
            }
        }

        void WriteCastling(StreamWriter writer, FriendlyCastling castling)
        {
            writer.WriteLine(PersistenceContants.CastlingSection);

            writer.WriteLine(castling.WhiteShortCastlingPossibility.ToString());
            writer.WriteLine(castling.WhiteLongCastlingPossibility.ToString());
            writer.WriteLine(castling.BlackShortCastlingPossibility.ToString());
            writer.WriteLine(castling.BlackLongCastlingPossibility.ToString());

            writer.WriteLine(castling.WhiteCastlingDone.ToString());
            writer.WriteLine(castling.BlackCastlingDone.ToString());
        }

        void WriteEnPassant(StreamWriter writer, FriendlyEnPassant enPassant)
        {
            writer.WriteLine(PersistenceContants.EnPassantSection);

            WritePosition(writer, enPassant.WhiteEnPassant.Value);
            WritePosition(writer, enPassant.BlackEnPassant.Value);
        }

        void WritePosition(StreamWriter writer, Position position)
        {
            if(position == null)
            {
                writer.WriteLine(PersistenceContants.NullEnPassant);
                return;
            }

            writer.WriteLine(NotationConverter.ToString(position));
        }
    }
}