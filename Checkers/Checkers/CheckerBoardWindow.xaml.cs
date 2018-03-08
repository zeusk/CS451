using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for CheckerBoardWindow.xaml
    /// </summary>
    public partial class CheckerBoardWindow : Page
    {
        private static GameClient gc = GameClient.GetInstance();
        private static List<int> movePair = new List<int>();
        private static List<int> chosenPiece = new List<int>();
        private static int boardSize = 400;
        private static LinearGradientBrush redGradient = new LinearGradientBrush();
        private static LinearGradientBrush blackGradient = new LinearGradientBrush();
        private static System.Windows.Threading.DispatcherTimer recvTimer;

        public static CheckerBoardWindow Instance { get; private set; }

        public CheckerBoardWindow()
        {
            InitializeComponent();
            Instance = this;

            SetGradient(redGradient, Color.FromRgb(220, 141, 124), Color.FromRgb(177, 8, 1), Color.FromRgb(131, 22, 2));
            SetGradient(blackGradient, Color.FromRgb(139, 141, 136), Color.FromRgb(43, 43, 45), Color.FromRgb(68, 67, 63));

            if (gc.testLocal)
                gc.GetGameState().player2Name = "testLocalPlayer";

            refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), false));

            recvTimer = new System.Windows.Threading.DispatcherTimer();
            recvTimer.Tick += new EventHandler(GetMove);
            recvTimer.Interval = TimeSpan.FromMilliseconds(166);
            recvTimer.Start();
        }

        private static void SetGradient(LinearGradientBrush brush, Color s, Color m, Color e)
        {
            brush.StartPoint = new Point(0.5, 0);
            brush.EndPoint = new Point(0.5, 1);
            brush.GradientStops.Add(new GradientStop { Offset = 0.0, Color = s });
            brush.GradientStops.Add(new GradientStop { Offset = 0.2, Color = m });
            brush.GradientStops.Add(new GradientStop { Offset = 0.7, Color = e });
        }

        private static LinearGradientBrush getColorForPlayer()
        {
            if (Util.amPlayer1())
                return redGradient;
            else
                return blackGradient;
        }

        public static Border generateCheckerBoardUI(int size, GameState gs, bool clickable)
        {
            Grid myGrid = new Grid();
            double gridSize = size * 0.99;
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Center;
            int[,] myboard = gs.getBoard();

            List<ColumnDefinition> gridCols = new List<ColumnDefinition>();
            List<RowDefinition> gridRows = new List<RowDefinition>();
            for (int i = 0; i < 8; i++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(gridSize / 8);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(gridSize / 8);
                gridCols.Add(gridCol1);
                gridRows.Add(gridRow1);
            }

            foreach (ColumnDefinition c in gridCols)
                myGrid.ColumnDefinitions.Add(c);
            foreach (RowDefinition r in gridRows)
                myGrid.RowDefinitions.Add(r);


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ImageBrush brush = new ImageBrush();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

                    if (i % 2 == j % 2)
                        bitmap.UriSource = new Uri("../../Resource/DarkGrid.JPG", UriKind.Relative);
                    else
                        bitmap.UriSource = new Uri("../../Resource/LightGrid.JPG", UriKind.Relative);

                    brush.ImageSource = bitmap;

                    Button checkerboxButton = new Button();
                    checkerboxButton.SetValue(Grid.RowProperty, i);
                    checkerboxButton.SetValue(Grid.ColumnProperty, j);
                    checkerboxButton.Background = brush;
                    checkerboxButton.Tag = i.ToString() + " " + j.ToString();
                    checkerboxButton.Click += (s, e) => {
                        string coor = (string)((Button)s).Tag;

                        int row = Int32.Parse(coor.Split(' ')[0]);
                        int col = Int32.Parse(coor.Split(' ')[1]);

                        addMove(row, col, gs.cb);
                    };
                    checkerboxButton.IsHitTestVisible = clickable;

                    double ellipSize = (size / 8) * 0.8;

                    if (myboard[i, j] != 0)
                    {
                        Border OuterBorder = new Border();
                        OuterBorder.Width = ellipSize;
                        OuterBorder.Height = ellipSize;

                        if (myboard[i, j] == 1)
                        {
                            //player 1, regular
                            OuterBorder.Background = redGradient;
                            OuterBorder.BorderThickness = new Thickness(0);
                        }
                        else if (myboard[i, j] == 3)
                        {
                            //player 1, kinged
                            OuterBorder.Background = redGradient;
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Wheat);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 10);
                        }
                        else if (myboard[i, j] == 2)
                        {
                            //player 2, regular
                            OuterBorder.Background = blackGradient;
                            OuterBorder.BorderThickness = new Thickness(0);
                        }
                        else if (myboard[i, j] == 4)
                        {
                            //player 2, kinged
                            OuterBorder.Background = blackGradient;
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Wheat);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 10);
                        }
                        OuterBorder.CornerRadius = new CornerRadius(20);
                        OuterBorder.HorizontalAlignment = HorizontalAlignment.Center;
                        if (Util.amPlayer1(gs))
                        {
                            RotateTransform myRotateTransform = new RotateTransform(180, 0.5, 0.5);
                            OuterBorder.LayoutTransform = myRotateTransform;
                        }
                        checkerboxButton.Content = OuterBorder;
                    }
                    bitmap.EndInit();

                    myGrid.Children.Add(checkerboxButton);
                }
            }

            if (Util.amPlayer1(gs))
            {
                RotateTransform myRotateTransform = new RotateTransform(180, 0.5, 0.5);
                myGrid.LayoutTransform = myRotateTransform; // flip the board 180 degrees
            }

            Border gridBorder = new Border();

            gridBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 51, 0));
            gridBorder.Height = size;
            gridBorder.Width = size;
            gridBorder.BorderThickness = new Thickness(5);
            gridBorder.Child = myGrid;

            return gridBorder;
        }

        private static void WaitForTask(Task<int> t)
        {
            t.Wait();
            if (t.Result != 0)
                MessageBox.Show("Disconnected from server");
            if (t.Result != 0 || gc.GetGameState().getResult() != -1)
                Instance.navigateToEndWindow();
        }

        private static bool canInteract()
        {
            return (Util.isMyTurn() && !string.IsNullOrEmpty(Util.GetOpponentName()));
        }

        // addMove is called within a lambda from onClick
        // if addMove blocks, does the UI block?
        protected static void addMove(int i, int j, CheckerBoard cb)
        {
            if (!canInteract())
                return; // TODO: warn user it's not their turn or wait for opponent?

            if (movePair.Count == 0)
            {
                movePair.Add(i);
                movePair.Add(j);

                Debug.WriteLine($"first click --" + i + " " + j);
            }
            else if (movePair.Count == 2)
            {
                movePair.Add(i);
                movePair.Add(j);

                Debug.WriteLine($"second click-- " + movePair[0] + " " + movePair[1] + " " + movePair[2] + " " + movePair[3]);

                GameState newS = gc.GetGameState().applyMove(movePair, Util.myPlayerNum());

                if (newS != null) {
                    bool peiceJumped = (Math.Abs(movePair[2] - movePair[0]) + Math.Abs(movePair[3] - movePair[1])) > 2;
                    if (!peiceJumped || !gc.GetGameState().checkAvailableJump(movePair[2], movePair[3], Util.myPlayerNum()))
                        gc.GetGameState().endTurn();

                    Instance.refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), canInteract()));
                    WaitForTask(Task<int>.Factory.StartNew(() => gc.SendState()));

                    if (!Util.isMyTurn())
                        recvTimer.Start();
                } else MessageBox.Show("Your move was not valid");

                movePair.Clear();
            }
        }

        private void GetMove(object sender, EventArgs e)
        {
            if (canInteract())
                recvTimer.Stop();

            WaitForTask(Task<int>.Factory.StartNew(() => gc.ReceiveState(gc.GetGameState())));
            Instance.refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), canInteract()));
        }

        private void refreshBoard(Border newG)
        {
            dynamicGrid.Children.Clear();

            playerColorCircle.Fill = getColorForPlayer();
            turnToMoveText.Text = Util.GetGameState().playerTurn + "'s turn";
            connectedPlayerName.Text = string.IsNullOrEmpty(Util.GetOpponentName()) ? "Waiting to join" : Util.GetOpponentName();

            dynamicGrid.Children.Add(newG);
        }

        private void CloseGame(object sender, RoutedEventArgs e)
        {
            if (gc.inGame)
                gc.QuitGame();
            gc.Disconnect();
            Application.Current.Shutdown();
        }

        protected void navigateToEndWindow()
        {
            NavigationService.Navigate(new Uri("EndWindow.xaml", UriKind.Relative), gc);
        }
    }
}
