using grid;
using img;
using src;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using util;

namespace Snake
{
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridVals, ImageSource> gridValToImg = new()
        {
            {GridVals.Empty, Images.Empty },
            {GridVals.Snake, Images.Body },
            {GridVals.Food, Images.Food }
        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.UP, 0},
            {Direction.RIGHT, 90},
            {Direction.DOWN, 180},
            {Direction.LEFT, 270}
        };

        private readonly int rows = 15, cols = 15;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        private int tickDelay = 150; //pt a modifica viteza sarpelui
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
        }

        private async Task RunGame() {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e) { 
            if (Overlay.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) { 
            if(gameState.gameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.changeDir(Direction.LEFT);
                    break;
                case Key.Right:
                    gameState.changeDir(Direction.RIGHT);
                    break;
                case Key.Up:
                    gameState.changeDir(Direction.UP);
                    break;
                case Key.Down:
                    gameState.changeDir(Direction.DOWN);
                    break;
                case Key.A:
                    gameState.changeDir(Direction.LEFT);
                    break;
                case Key.D:
                    gameState.changeDir(Direction.RIGHT);
                    break;
                case Key.W:
                    gameState.changeDir(Direction.UP);
                    break;
                case Key.S:
                    gameState.changeDir(Direction.DOWN);
                    break;
            }
        }

        private async Task GameLoop()
        {
            while(!gameState.gameOver)
            {
                await Task.Delay(tickDelay);
                gameState.Move();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);
            GameGrid.Height = GameGrid.Width * (rows / (double)cols);
            for (int i = 0; i < rows; i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    Image img = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[i, j] = img;
                    GameGrid.Children.Add(img);
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            drawSnakeHead();
            ScoreText.Text = $"Score {gameState.score}";
        }

        private void DrawGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GridVals gridVal = gameState.grid[i, j];
                    gridImages[i, j].Source = gridValToImg[gridVal];
                    gridImages[i, j].RenderTransform = Transform.Identity;
                }
            }
        }

        private void drawSnakeHead()
        {
            Position headPos = gameState.headPos();
            Image img = gridImages[headPos.getRow(), headPos.getCol()];
            img.Source = Images.Head;

            int rotation = dirToRotation[gameState.Dir];
            img.RenderTransform = new RotateTransform(rotation);
        }

        private async Task drawDeadSnake() {
            List<Position> positions = new List<Position>(gameState.SnakePositions());

            for(int i = 0;i < positions.Count;i++)
            {
                Position position = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[position.getRow(), position.getCol()].Source = source;
                await Task.Delay(50);
            }
        }

        private async Task ShowCountDown()
        {
            for (int i =3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await drawDeadSnake();
            await Task.Delay(1000);
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "PRESS ANY KEY TO START";
        }
    }
}
