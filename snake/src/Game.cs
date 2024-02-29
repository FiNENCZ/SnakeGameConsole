using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake.src
{
    internal class Game
    {
        private const int ScreenWidth = 64;
        private const int ScreenHeight = 32;
        private const int InitialSnakeLength = 5;
        private const int MillisecondsPerMove = 100;

        private readonly Random _random = new Random();
        private readonly ConsoleColor _headColor = ConsoleColor.Red;
        private readonly ConsoleColor _bodyColor = ConsoleColor.Green;
        private readonly ConsoleColor _berryColor = ConsoleColor.Cyan;
        private readonly List<int> _xPositions = new List<int>();
        private readonly List<int> _yPositions = new List<int>();

        private int _score = InitialSnakeLength;
        private bool _gameOver;
        private Direction _currentDirection = Direction.Right;
        private int _berryX;
        private int _berryY;

        public void Run()
        {
            Console.WindowHeight = ScreenHeight;
            Console.WindowWidth = ScreenWidth;
            InitializeGame();

            while (!_gameOver)
            {
                Console.Clear();
                DrawBorder();
                DrawSnake();
                DrawBerry();
                HandleInput();
                MoveSnake();
                CheckCollision();
                System.Threading.Thread.Sleep(MillisecondsPerMove);
            }

            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2);
            Console.WriteLine($"Game over, Score: {_score}");
            Console.SetCursorPosition(ScreenWidth / 5, ScreenHeight / 2 + 1);
        }

        private void InitializeGame()
        {
            int initialX = ScreenWidth / 2;
            int initialY = ScreenHeight / 2;
            for (int i = 0; i < InitialSnakeLength; i++)
            {
                _xPositions.Add(initialX - i);
                _yPositions.Add(initialY);
            }

            _berryX = _random.Next(1, ScreenWidth - 2);
            _berryY = _random.Next(1, ScreenHeight - 2);
        }

        private void DrawBorder()
        {
            Console.ForegroundColor = _bodyColor;
            for (int i = 0; i < ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, ScreenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(ScreenWidth - 1, i);
                Console.Write("■");
            }
        }

        private void DrawSnake()
        {
            Console.ForegroundColor = _headColor;
            Console.SetCursorPosition(_xPositions[0], _yPositions[0]);
            Console.Write("■");

            Console.ForegroundColor = _bodyColor;
            for (int i = 1; i < _xPositions.Count; i++)
            {
                Console.SetCursorPosition(_xPositions[i], _yPositions[i]);
                Console.Write("■");
            }
        }

        private void DrawBerry()
        {
            Console.ForegroundColor = _berryColor;
            Console.SetCursorPosition(_berryX, _berryY);
            Console.Write("■");
        }

        private void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow && _currentDirection != Direction.Down)
                    _currentDirection = Direction.Up;
                else if (key.Key == ConsoleKey.DownArrow && _currentDirection != Direction.Up)
                    _currentDirection = Direction.Down;
                else if (key.Key == ConsoleKey.LeftArrow && _currentDirection != Direction.Right)
                    _currentDirection = Direction.Left;
                else if (key.Key == ConsoleKey.RightArrow && _currentDirection != Direction.Left)
                    _currentDirection = Direction.Right;
            }
        }

        private void MoveSnake()
        {
            for (int i = _xPositions.Count - 1; i > 0; i--)
            {
                _xPositions[i] = _xPositions[i - 1];
                _yPositions[i] = _yPositions[i - 1];
            }

            switch (_currentDirection)
            {
                case Direction.Up:
                    _yPositions[0]--;
                    break;
                case Direction.Down:
                    _yPositions[0]++;
                    break;
                case Direction.Left:
                    _xPositions[0]--;
                    break;
                case Direction.Right:
                    _xPositions[0]++;
                    break;
            }
        }
        private void ExtendSnake()
        {
            int lastX = _xPositions[_xPositions.Count - 1];
            int lastY = _yPositions[_yPositions.Count - 1];
            _xPositions.Add(lastX);
            _yPositions.Add(lastY);
        }

        private void CheckCollision()
        {
            // Border collision
            if (_xPositions[0] == 0 || _xPositions[0] == ScreenWidth - 1 || _yPositions[0] == 0 || _yPositions[0] == ScreenHeight - 1)
                _gameOver = true;

            // Berry collision
            if (_xPositions[0] == _berryX && _yPositions[0] == _berryY)
            {
                _score++;
                ExtendSnake();
                GenerateNewBerry();
            }

            // Head-body collision
            for (int i = 1; i < _xPositions.Count; i++)
            {
                if (_xPositions[i] == _xPositions[0] && _yPositions[i] == _yPositions[0])
                {
                    _gameOver = true;
                    break;
                }
            }
        }

        private void GenerateNewBerry()
        {
            _berryX = _random.Next(1, ScreenWidth - 2);
            _berryY = _random.Next(1, ScreenHeight - 2);
        }
    }
}
