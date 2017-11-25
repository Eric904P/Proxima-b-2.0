﻿using System.IO;
using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Notation;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly.Persistence
{
    public class BoardReader
    {
        public bool BoardExists(string path)
        {
            return File.Exists(path);
        }

        public FriendlyBoard Read(string path)
        {
            FriendlyPiecesList pieces = null;
            FriendlyCastling castling = null;
            FriendlyEnPassant enPassant = null;

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    if (line.Length == 0)
                        continue;

                    switch (line)
                    {
                        case PersistenceContants.BoardSection:
                        {
                            pieces = ReadBoard(reader);
                            break;
                        }

                        case PersistenceContants.CastlingSection:
                        {
                            castling = ReadCastling(reader);
                            break;
                        }

                        case PersistenceContants.EnPassantSection:
                        {
                            enPassant = ReadEnPassant(reader);
                            break;
                        }
                    }
                }
            }

            return new FriendlyBoard(pieces, castling, enPassant);
        }

        private FriendlyPiecesList ReadBoard(StreamReader reader)
        {
            var pieces = new FriendlyPiecesList();

            for (int y = 0; y < 8; y++)
            {
                var line = reader.ReadLine().Trim();
                var splittedLine = line.Split(' ');

                for (int x = 0; x < 8; x++)
                {
                    if (splittedLine[x] == PersistenceContants.EmptyBoardField)
                    {
                        continue;
                    }

                    var position = new Position(x + 1, 8 - y);
                    var color = ColorConverter.GetColor(splittedLine[x][0]);
                    var piece = PieceConverter.GetPiece(splittedLine[x][1]);

                    pieces.Add(new FriendlyPiece(position, piece, color));
                }
            }

            return pieces;
        }

        private FriendlyCastling ReadCastling(StreamReader reader)
        {
            return new FriendlyCastling
            {
                WhiteShortCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                WhiteLongCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                BlackShortCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),
                BlackLongCastlingPossibility = bool.Parse(reader.ReadLine().Trim()),

                WhiteCastlingDone = bool.Parse(reader.ReadLine().Trim()),
                BlackCastlingDone = bool.Parse(reader.ReadLine().Trim())
            };
        }

        private FriendlyEnPassant ReadEnPassant(StreamReader reader)
        {
            return new FriendlyEnPassant
            {
                WhiteEnPassant = ReadPosition(reader),
                BlackEnPassant = ReadPosition(reader)
            };
        }

        private Position? ReadPosition(StreamReader reader)
        {
            var line = reader.ReadLine().Trim();

            if (line == PersistenceContants.NullEnPassant)
            {
                return null;
            }

            return NotationConverter.ToPosition(line);
        }
    }
}
