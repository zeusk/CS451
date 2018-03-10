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

            //Set gradiant for the checker pieces
            SetGradient(redGradient, Color.FromRgb(220, 141, 124), Color.FromRgb(177, 8, 1), Color.FromRgb(131, 22, 2));
            SetGradient(blackGradient, Color.FromRgb(139, 141, 136), Color.FromRgb(43, 43, 45), Color.FromRgb(68, 67, 63));

            //name for local 
            if (gc.testLocal)
                gc.GetGameState().player2Name = "testLocalPlayer";

            //Generate the game board
            refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), false));

            //Set up the backend timer to run the game client
            recvTimer = new System.Windows.Threading.DispatcherTimer();
            recvTimer.Tick += new EventHandler(GetMove);
            recvTimer.Interval = TimeSpan.FromMilliseconds(166);
            recvTimer.Start();
        }

        //Set up the gradiant 
        private static void SetGradient(LinearGradientBrush brush, Color s, Color m, Color e)
        {
            brush.StartPoint = new Point(0.5, 0);
            brush.EndPoint = new Point(0.5, 1);
            brush.GradientStops.Add(new GradientStop { Offset = 0.0, Color = s });
            brush.GradientStops.Add(new GradientStop { Offset = 0.2, Color = m });
            brush.GradientStops.Add(new GradientStop { Offset = 0.7, Color = e });
        }

        //get the color for player ID (either red or black)
        private static LinearGradientBrush getColorForPlayer()
        {
            if (Util.GetPlayer1Name().Equals(Util.GetPlayerTurnName()))
                return redGradient;
            else
                return blackGradient;
        }

        //Generate the checker board UI
        public static Border generateCheckerBoardUI(int size, GameState gs, bool clickable)
        {
            SetGradient(redGradient, Color.FromRgb(220, 141, 124), Color.FromRgb(177, 8, 1), Color.FromRgb(131, 22, 2));
            SetGradient(blackGradient, Color.FromRgb(139, 141, 136), Color.FromRgb(43, 43, 45), Color.FromRgb(68, 67, 63));

            //Set a grid
            Grid myGrid = new Grid();
            double gridSize = size * 0.99;
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Center;
            int[,] myboard = gs.getBoard();
            
            //Setup 8 columns and rows for the checkerboard
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

            //Add rows and columns to the grid
            foreach (ColumnDefinition c in gridCols)
                myGrid.ColumnDefinitions.Add(c);
            foreach (RowDefinition r in gridRows)
                myGrid.RowDefinitions.Add(r);

            //For each grid, set that exact piece
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    //Get the background of that grid (dark or light)
                    ImageBrush brush = new ImageBrush();
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;

                    if (i % 2 == j % 2)
                        bitmap.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() +"/Resource/DarkGrid.JPG", UriKind.Relative);
                    else
                        bitmap.UriSource = new Uri(System.IO.Directory.GetCurrentDirectory() + "/Resource/LightGrid.JPG", UriKind.Relative);

                    brush.ImageSource = bitmap;

                    //Set the grid to a clickable button 
                    Button checkerboxButton = new Button();
                    checkerboxButton.SetValue(Grid.RowProperty, i);
                    checkerboxButton.SetValue(Grid.ColumnProperty, j);
                    checkerboxButton.Background = brush;
                    checkerboxButton.Tag = i.ToString() + " " + j.ToString();
                    checkerboxButton.Click += (s, e) => {
                        string coor = (string)((Button)s).Tag;
                        //add the column and row as tag of that grid
                        int row = Int32.Parse(coor.Split(' ')[0]);
                        int col = Int32.Parse(coor.Split(' ')[1]);

                        addMove(row, col, gs.cb);
                    };
                    checkerboxButton.IsHitTestVisible = clickable;

                    //Draw the checker piece in the button
                    double ellipSize = size > 150 ? (size / 8) * 0.8 : (size / 8) * 0.65;
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
                        OuterBorder.CornerRadius = new CornerRadius(7);
                        OuterBorder.HorizontalAlignment = HorizontalAlignment.Center;

                        //Rotate the pieces so the pieces stay the same direction when we rotate the board if playerID == 1
                        if (Util.amPlayer1(gs))
                        {
                            RotateTransform myRotateTransform = new RotateTransform(180, 0.5, 0.5);
                            OuterBorder.LayoutTransform = myRotateTransform;
                        }
                        checkerboxButton.Content = OuterBorder;
                    }
                    bitmap.EndInit();
                    //Add that grid to the board grid
                    myGrid.Children.Add(checkerboxButton);
                }
            }

            //Rotate the board for the other player
            if (Util.amPlayer1(gs))
            {
                RotateTransform myRotateTransform = new RotateTransform(180, 0.5, 0.5);
                myGrid.LayoutTransform = myRotateTransform; // flip the board 180 degrees
            }

            Border gridBorder = new Border();

            //Add a overall grid to the checkerboard
            gridBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(102, 51, 0));
            gridBorder.Height = size;
            gridBorder.Width = size;
            gridBorder.BorderThickness = size > 150 ? new Thickness(5) : new Thickness(1);
            gridBorder.Child = myGrid;

            return gridBorder;
        }

        //Wait for the other player to make a move
        private static void WaitForTask(Task<int> t)
        {
            t.Wait();
            if (t.Result != 0)
                MessageBox.Show("Disconnected from server");
            if (t.Result != 0 || gc.GetGameState().getResult() != -1)
                Instance.navigateToEndWindow();
        }

        //Check whether the current player can interact with the board
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
                //This is the second click 
                movePair.Add(i);
                movePair.Add(j);

                Debug.WriteLine($"second click-- " + movePair[0] + " " + movePair[1] + " " + movePair[2] + " " + movePair[3]);

                //Pass the move pair to Gamestate and get the new board 
                int prvPos = gc.GetGameState().cb.getBoard()[movePair[0], movePair[1]];
                GameState newS = gc.GetGameState().applyMove(movePair, Util.myPlayerNum());

                //if the move is valid, switch turn and refresh board
                if (newS != null) {
                    int newPos = gc.GetGameState().cb.getBoard()[movePair[2], movePair[3]];
                    bool peiceJumped = (Math.Abs(movePair[2] - movePair[0]) + Math.Abs(movePair[3] - movePair[1])) > 2;
                    if (!peiceJumped || newPos != prvPos || !gc.GetGameState().checkAvailableJump(movePair[2], movePair[3], Util.myPlayerNum()))
                        gc.GetGameState().endTurn();

                    Instance.refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), canInteract()));
                    WaitForTask(Task<int>.Factory.StartNew(() => gc.SendState()));

                    if (!Util.isMyTurn())
                        recvTimer.Start();
                } else MessageBox.Show("Your move was not valid"); //otherwise show error message box

                movePair.Clear();
            }
        }

        //Stop the current user to get a move from the opponent
        private void GetMove(object sender, EventArgs e)
        {
            WaitForTask(Task<int>.Factory.StartNew(() => gc.ReceiveState(gc.GetGameState())));
            Instance.refreshBoard(generateCheckerBoardUI(boardSize, gc.GetGameState(), canInteract()));

            if (canInteract())
                recvTimer.Stop();
        }

        //Refresh the board grid in the dynamic grid in UI
        private void refreshBoard(Border newG)
        {
            dynamicGrid.Children.Clear();

            playerColorCircle.Background = getColorForPlayer();
            turnToMoveText.Text = Util.isMyTurn() ? "Your turn" : Util.GetOpponentName() + "'s turn";
            connectedPlayerName.Text = string.IsNullOrEmpty(Util.GetOpponentName()) ? "Waiting to join" : Util.GetOpponentName();

            dynamicGrid.Children.Add(newG);
        }

        //Close the program and delete player from server 
        private void CloseGame(object sender, RoutedEventArgs e)
        {
            gc.Disconnect();
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        //Go to the end window
        protected void navigateToEndWindow()
        {
            recvTimer.Stop();
            MainWindow.MainFrame.NavigationService.Navigate(new Uri("EndWindow.xaml", UriKind.Relative), gc);
        }
    }
}
