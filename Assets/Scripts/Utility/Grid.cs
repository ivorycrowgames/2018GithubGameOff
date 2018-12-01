using IvoryCrow.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace IvoryCrow.Utilities
{

    public class Position : IEquatable<Position>
    {
        public int X;
        public int Y;

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Position other)
        {
            if (other != null)
            {
                return this.X == other.X && this.Y == other.Y;
            }

            return false;
        }
    }

    public class Grid<T> : IEnumerable<T>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        private T[][] _gameBoardState;
        private Queue<T> _temporaryIterationList;

        public Grid(int width, int height)
        {
            Throw.IfFalse(default(T) == null, "Cannot use grid with non-nullable type");
            Throw.IfTrue(width == 0, "Grid width cannot be 0");
            Throw.IfTrue(height == 0, "Grid height cannot be 0");

            Width = width;
            Height = height;

            _gameBoardState = new T[Height][];
            for (int y = 0; y < Height; ++y)
            {
                _gameBoardState[y] = new T[Width];
            }

            _temporaryIterationList = new Queue<T>(width * height);
        }

        public T Get(int x, int y)
        {
            CheckBounds(x, y);
            return _gameBoardState[y][x];
        }

        public T Remove(int x, int y)
        {
            CheckBounds(x, y);
            var previousPiece = _gameBoardState[y][x];
            _gameBoardState[y][x] = default(T);
            return previousPiece;
        }

        public bool IsFree(int x, int y)
        {
            return Get(x, y) == null;
        }

        public bool IsOccupied(int x, int y)
        {
            return !IsFree(x, y);
        }

        public bool InBounds(int x, int y)
        {
            return (x >= 0 && x < Width) && (y >= 0 && y < Height);
        }

        public void PlaceGamePiece(int x, int y, T gamePiece)
        {
            CheckBounds(x, y);
            Throw.IfTrue(IsOccupied(x, y), "Cell is already occupied by game piece");

            _gameBoardState[y][x] = gamePiece;
        }

        public bool IsBoardEmpty()
        {
            return !this.Any();
        }

        public int TotalOnBoard()
        {
            return this.Count();
        }

        public void ShiftBoard(Direction dir)
        {
            if (Directions.IsVertical(dir))
            {
                List<T> temporaryColumn = new List<T>(Height);
                for (int x = 0; x < Width; ++x)
                {
                    temporaryColumn.Clear();
                    for (int y = 0; y < Height; ++y)
                    {
                        if (!IsFree(x, y))
                        {
                            temporaryColumn.Add(Get(x, y));
                        }
                    }

                    int lastPieceSquare = temporaryColumn.Count - 1;
                    for (int y = 0; y < Height; ++y)
                    {
                        if (y <= lastPieceSquare)
                        {
                            _gameBoardState[y][x] = temporaryColumn[y];
                        }
                        else
                        {
                            _gameBoardState[y][x] = default(T);
                        }

                    }
                }
            }
            else
            {
                for (int y = 0; y < Height; ++y)
                {
                    var consolidatedRow = _gameBoardState[y].Where(obj => (obj != null)).ToArray();

                    // If we shift left we will have consolidatedRow at the start
                    // If we shift right we will have consolidatedRow at the end
                    int gamePieceFillStartIndex = (dir == Direction.Left) ? 0 : Width - consolidatedRow.Length;

                    // Clear our array and copy in the consolidatedRow at the proper index
                    Array.Clear(_gameBoardState[y], 0, _gameBoardState[y].Length);
                    Array.Copy(consolidatedRow, 0, _gameBoardState[y], gamePieceFillStartIndex, consolidatedRow.Length);
                }
            }
        }

        public void Clear()
        {
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    _gameBoardState[y][x] = default(T);
                }
            }
        }

        private void CheckBounds(int x, int y)
        {
            Throw.IfTrue((x < 0 || x >= Width), "X component is not within the bounds of the board");
            Throw.IfTrue((y < 0 || y >= Height), "Y component the bounds of the board");
        }

        public IEnumerator<T> GetEnumerator()
        {
            _temporaryIterationList.Clear();

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    if (IsOccupied(x, y))
                    {
                        _temporaryIterationList.Enqueue(Get(x, y));
                    }
                    
                }
            }

            return _temporaryIterationList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
