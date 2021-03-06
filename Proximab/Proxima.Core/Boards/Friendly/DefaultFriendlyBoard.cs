﻿using Proxima.Core.Commons.Colors;
using Proxima.Core.Commons.Pieces;
using Proxima.Core.Commons.Positions;

namespace Proxima.Core.Boards.Friendly
{
    /// <summary>
    /// Represents a default (all pieces on their starting position) chess board.
    /// </summary>
    public class DefaultFriendlyBoard : FriendlyBoard
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultFriendlyBoard"/> class.
        /// </summary>
        public DefaultFriendlyBoard()
        {
            // White pieces
            SetPiece(new FriendlyPiece(new Position(1, 1), PieceType.Rook, Color.White));
            SetPiece(new FriendlyPiece(new Position(2, 1), PieceType.Knight, Color.White));
            SetPiece(new FriendlyPiece(new Position(3, 1), PieceType.Bishop, Color.White));
            SetPiece(new FriendlyPiece(new Position(4, 1), PieceType.Queen, Color.White));
            SetPiece(new FriendlyPiece(new Position(5, 1), PieceType.King, Color.White));
            SetPiece(new FriendlyPiece(new Position(6, 1), PieceType.Bishop, Color.White));
            SetPiece(new FriendlyPiece(new Position(7, 1), PieceType.Knight, Color.White));
            SetPiece(new FriendlyPiece(new Position(8, 1), PieceType.Rook, Color.White));

            SetPiece(new FriendlyPiece(new Position(1, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(2, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(3, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(4, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(5, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(6, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(7, 2), PieceType.Pawn, Color.White));
            SetPiece(new FriendlyPiece(new Position(8, 2), PieceType.Pawn, Color.White));

            // Black pieces
            SetPiece(new FriendlyPiece(new Position(1, 8), PieceType.Rook, Color.Black));
            SetPiece(new FriendlyPiece(new Position(2, 8), PieceType.Knight, Color.Black));
            SetPiece(new FriendlyPiece(new Position(3, 8), PieceType.Bishop, Color.Black));
            SetPiece(new FriendlyPiece(new Position(4, 8), PieceType.Queen, Color.Black));
            SetPiece(new FriendlyPiece(new Position(5, 8), PieceType.King, Color.Black));
            SetPiece(new FriendlyPiece(new Position(6, 8), PieceType.Bishop, Color.Black));
            SetPiece(new FriendlyPiece(new Position(7, 8), PieceType.Knight, Color.Black));
            SetPiece(new FriendlyPiece(new Position(8, 8), PieceType.Rook, Color.Black));

            SetPiece(new FriendlyPiece(new Position(1, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(2, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(3, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(4, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(5, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(6, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(7, 7), PieceType.Pawn, Color.Black));
            SetPiece(new FriendlyPiece(new Position(8, 7), PieceType.Pawn, Color.Black));

            // Castling
            Castling.WhiteShortCastlingPossibility = true;
            Castling.WhiteLongCastlingPossibility = true;
            Castling.BlackShortCastlingPossibility = true;
            Castling.BlackLongCastlingPossibility = true;

            Castling.WhiteCastlingDone = false;
            Castling.BlackCastlingDone = false;

            // EnPassant
            EnPassant.WhiteEnPassant = null;
            EnPassant.BlackEnPassant = null;
        }
    }
}
