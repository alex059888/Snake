using grid;
using System;
using System.Collections.Generic;
using util;

namespace src
{
    public class GameState
    {
        public int rows { get; }
        public int cols { get;}
        public GridVals[,] grid { get; private set; }
        public Direction Dir { get; private set; }
        public int score { get; set; }
        public bool gameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();

        private readonly Random random = new Random();

        public GameState(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;
            grid = new GridVals[rows, cols];
            Dir = Direction.RIGHT;

            addSnake();
            AddFood();
        }

        public void addSnake()
        {
            int r = rows / 2;
            for(int i = 0;i< 3; i++)
            {
                grid[r, i] = GridVals.Snake;
                snakePositions.AddFirst(new Position(r, i));
            }
        }

        private IEnumerable<Position> EmptyPosition() { 
            for(int i = 0;i<rows;i++)
            {
                for(int j = 0;j<cols;j++)
                {
                    if (grid[i,j] == GridVals.Empty)
                    {
                        yield return new Position(i,j);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPosition());

            if (empty.Count == 0) return;
            Position p = empty[random.Next(empty.Count)];
            grid[p.getRow(),p.getCol()] = GridVals.Food;
        }

        public Position headPos()
        {
            return snakePositions.First.Value;
        }

        public Position tailPos()
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions() {  return snakePositions; }

        public void AddHead(Position head)
        {
            snakePositions.AddFirst(head);
            grid[head.getRow(), head.getCol()] = GridVals.Snake;
        }

        public void removeTail() {
            Position tail = snakePositions.Last.Value;
            snakePositions.RemoveLast();
            grid[tail.getRow(), tail.getCol()] = GridVals.Empty;
        }

        private Direction getLastDir()
        {
            if(dirChanges.Count == 0) {
                return Dir;
            }

            return dirChanges.Last.Value;
        }

        private bool canChangeDir(Direction dir)
        {
            if(dirChanges.Count == 2)
            {
                return false;
            }

            Direction lD = getLastDir();
            return dir != lD && dir != lD.Opposite();
        }

        public void changeDir(Direction d) {
            if(canChangeDir(d))
            {
                dirChanges.AddLast(d);
            }
        }

        private bool outsideGrid(Position p) { 
            return (p.getRow() < 0 || p.getCol() < 0 || p.getRow() >= rows || p.getCol() >= cols);
        }

        private GridVals willHit(Position newHeadPos) {
            if(outsideGrid(newHeadPos))
            {
                return GridVals.Outside;
            }
            if(newHeadPos == tailPos())
            {
                return GridVals.Empty;
            }
            return grid[newHeadPos.getRow(), newHeadPos.getCol()];
        }

        public void Move() {
            if(dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Position newHeadPos = headPos().Translate(Dir);
            GridVals hit = willHit(newHeadPos);
            if(hit == GridVals.Outside || hit == GridVals.Snake) {
                gameOver = true;
            } else if(hit == GridVals.Empty)
            {
                removeTail();
                AddHead(newHeadPos);
            } else if(hit == GridVals.Food) { 
                AddHead(newHeadPos);
                score++;
                AddFood();
            }
        }
    }
}
