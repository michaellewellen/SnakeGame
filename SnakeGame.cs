namespace SnakeGameApp
{
    public class SnakeGameApp
    {
        #region Constants
        private const int GridWidth = 50;
        private const int GridHeight = 25;
        private const int InnerLeft = 1;
        private const int InnerTop = 1;
        private const int InnerRight = GridWidth - 2;
        private const int InnerBottom = GridHeight - 2;
        private const int MinGameSpeedDelay = 40;
        #endregion

         #region Fields
        private int applePitchHz = 400;
        private Direction currentDirection = Direction.Right;
        private Direction? nextDirection = null;
        private int gameSpeedDelay = 160;
        private bool exitRequested = false;
        private bool justAteApple = false;
        private bool showYum = false;
        private bool gameOver = false;
        private bool startNewGame = false;

        private readonly Random rnd = new();
        private List<SnakeSegment> snake = new();
        private int appleX;
        private int appleY;
        private ConsoleColor snakeColor = ConsoleColor.DarkGreen;
        private ConsoleColor appleColor = ConsoleColor.Red;
        #endregion

        #region Game Loop
        public async Task RunAsync()
        {
            Console.CursorVisible = false;
            Console.Clear();
            DrawBorder();
            _ = Task.Run(ListenForKeyPresses);

            SpawnNewGame();

            while (!exitRequested)
            {
                if (gameOver)
                {
                    await ShowDeathAnimation();
                    if (exitRequested) break;
                    SpawnNewGame();
                }
                else
                {
                    MoveSnake();
                    DrawGame();
                    DrawScore();
                    await Task.Delay(gameSpeedDelay);
                }
            }

            Console.ResetColor();
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine("Thank you for playing");
        }
        #endregion

        #region Game Setup
        private void SpawnNewGame()
        {
            Console.Clear();
            snake = SpawnSnake();
            applePitchHz = 400;
            snakeColor = ConsoleColor.DarkGreen;
            foreach (var seg in snake)
                seg.SegmentColor = snakeColor;

            (appleX, appleY) = SpawnApple();
            appleColor = GetRandomColor();
            currentDirection = Direction.Right;
            nextDirection = null;
            justAteApple = false;
            gameSpeedDelay = 160;
            DrawBorder();
        }

        private List<SnakeSegment> SpawnSnake()
        {
            int x = rnd.Next(InnerLeft + 2, InnerRight - 2);
            int y = rnd.Next(InnerTop, InnerBottom);
            return new List<SnakeSegment>
            {
                new SnakeSegment { GridX = x, GridY = y },
                new SnakeSegment { GridX = x - 1, GridY = y },
                new SnakeSegment { GridX = x - 2, GridY = y }
            };
        }

        private (int x, int y) SpawnApple()
        {
            int ax, ay;
            do
            {
                ax = rnd.Next(InnerLeft, InnerRight + 1);
                ay = rnd.Next(InnerTop, InnerBottom + 1);
            } while (snake.Any(s => s.GridX == ax && s.GridY == ay));

            return (ax, ay);
        }
        #endregion
    

        #region Game Logic
        private void MoveSnake()
        {
            if (nextDirection.HasValue)
            {
                currentDirection = nextDirection.Value;
                nextDirection = null;
            }

            var head = snake.First();
            var newHead = new SnakeSegment
            {
                GridX = head.GridX,
                GridY = head.GridY
            };

            switch (currentDirection)
            {
                case Direction.Up: newHead.GridY--; break;
                case Direction.Down: newHead.GridY++; break;
                case Direction.Left: newHead.GridX--; break;
                case Direction.Right: newHead.GridX++; break;
            }

            if (newHead.GridX < InnerLeft || newHead.GridX > InnerRight ||
                newHead.GridY < InnerTop || newHead.GridY > InnerBottom ||
                snake.Any(s => s.GridX == newHead.GridX && s.GridY == newHead.GridY))
            {
                gameOver = true;
                return;
            }

            if (newHead.GridX == appleX && newHead.GridY == appleY)
            {
                Console.Beep(applePitchHz, 120);
                applePitchHz = (int)(applePitchHz * Math.Pow(2.0, 1.0/36));
                justAteApple = true;
                showYum = true;
                (appleX, appleY) = SpawnApple();
                snakeColor = appleColor;
                appleColor = GetRandomColor();
                gameSpeedDelay = Math.Max(MinGameSpeedDelay, gameSpeedDelay - 20);
            }

            newHead.SegmentColor = snakeColor;
            snake.Insert(0, newHead);

            if (!justAteApple)
            {
                var tail = snake.Last();
                tail.ToRectangle(currentDirection, tail.SegmentColor).Remove();
                snake.RemoveAt(snake.Count - 1);
            }
            else
            {
                justAteApple = false;
            }
        }
        #endregion

        #region Rendering
        private void DrawGame()
        {
            var shapes = new List<IGraphic2D>();
            for (int i = 0; i < snake.Count; i++)
            {
                snake[i].IsHead = (i == 0);
                shapes.Add(snake[i].ToRectangle(currentDirection, snake[i].SegmentColor));
            }
            shapes.Add(CreateApple(appleX, appleY));
            AbstractGraphic2D.Display(shapes);
            DrawEyes(snake[0]);
            if (showYum) { DrawYum(snake[0]); showYum = false; }
        }

        private void DrawScore()
        {
            int score = (snake.Count - 3) * 10;
            string scoreText = $"  SCORE: {score}       ";
            int borderY = GridHeight * 2 - 1;
            int textX = GridWidth * 4 - scoreText.Length - 2;
            Console.SetCursorPosition(textX, borderY);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write(scoreText);
            Console.ResetColor();
        }

        private void DrawBorder() 
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int x = 0; x<GridWidth * 4; x++)
            {
                Console.SetCursorPosition(x,0);
                Console.Write("=");
                Console.SetCursorPosition(x, GridHeight * 2 -1);
                Console.Write("=");
            }

            for (int y = 0; y<GridHeight*2; y++)
            {
                Console.SetCursorPosition(0,y);
                Console.Write("║");
                Console.SetCursorPosition(GridWidth*4-1,y);
                Console.Write("║");
            }

            Console.SetCursorPosition(0, 0);
            Console.Write("╔");
            Console.SetCursorPosition(GridWidth * 4 - 1, 0);
            Console.Write("╗");
            Console.SetCursorPosition(0, GridHeight * 2 - 1);
            Console.Write("╚");
            Console.SetCursorPosition(GridWidth * 4 - 1, GridHeight * 2 - 1);
            Console.Write("╝");

            Console.ResetColor();
        }
        private void DrawYum(SnakeSegment head) 
        {
            int headLeft = head.GridX*4;
            int headTop = head.GridY*2;
            int textX = headLeft;
            int textY = headTop;

            switch (currentDirection)
            {
                case Direction.Up:
                    textY -= 1; break;
                case Direction.Down:
                    textY += 2; break;
                case Direction.Left:
                    textX -=4; break;
                case Direction.Right:
                    textX +=4; break;
            }

            if (textX>0 && textX + 3< Console.WindowWidth && textY >=0 && textY < Console.WindowHeight) 
            {
                Console.SetCursorPosition(textX, textY);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("YUM!");
                Console.ResetColor();
            }      
        }
        private void DrawEyes(SnakeSegment head) 
        {
            int x = head.GridX*4;
            int y = head.GridY*2;

            string eyes = currentDirection switch
            {
                Direction.Up => " o o ",
                Direction.Down => " o o ",
                Direction.Left => "o   ",
                Direction.Right => "   o",
                _ => "    "
            };
            if (x >= 0 && x + 3 < Console.WindowWidth && y >= 0 && y < Console.WindowHeight)
            {
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = snakeColor;
                Console.Write(eyes);
                Console.ResetColor();
            }
        }
        #endregion

        #region Inputspaw
        private void ListenForKeyPresses() 
        { 
             while(true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.UpArrow: if (currentDirection != Direction.Down) nextDirection = Direction.Up; break;
                        case ConsoleKey.DownArrow: if (currentDirection != Direction.Up) nextDirection = Direction.Down; break;
                        case ConsoleKey.LeftArrow: if (currentDirection != Direction.Right) nextDirection = Direction.Left; break;
                        case ConsoleKey.RightArrow: if (currentDirection != Direction.Left) nextDirection = Direction.Right; break;
                        case ConsoleKey.Escape: exitRequested = true; break;
                        case ConsoleKey.N:
                            if(gameOver)
                            {
                                startNewGame = true;
                                gameSpeedDelay = 160;
                            } break;
                    }
                }
                Thread.Sleep(10);
            }
        }
        #endregion

        #region Helpers
        private ConsoleColor GetRandomColor()
        {
            ConsoleColor[] colors = new[]
            {
                ConsoleColor.Red, ConsoleColor.DarkRed, ConsoleColor.Green, ConsoleColor.DarkGreen,
                ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Yellow, ConsoleColor.DarkYellow,
                ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.DarkMagenta
            };
            return colors[rnd.Next(colors.Length)];
        }

        private Rectangle CreateApple(int x, int y)
        {
            return new Rectangle(x * 4, y * 2, 4, 2)
            {
                DisplayChar = ' ',
                BackgroundColor = appleColor,
                ForegroundColor = appleColor
            };
        }

        private async Task ShowDeathAnimation()
        {
            bool toggle = false;
            while (!startNewGame && !exitRequested)
            {
                var shapes = new List<IGraphic2D>();
                foreach (var s in snake)
                {
                    shapes.Add(new Rectangle(s.GridX * 4, s.GridY * 2, 4, 2)
                    {
                        BackgroundColor = toggle ? snakeColor : ConsoleColor.White,
                        ForegroundColor = ConsoleColor.Black,
                        DisplayChar = ' '
                    });
                }
                AbstractGraphic2D.Display(shapes);
                toggle = !toggle;
                await Task.Delay(300);
            }
            gameOver = false;
            startNewGame = false;
        }
        #endregion
    }
}