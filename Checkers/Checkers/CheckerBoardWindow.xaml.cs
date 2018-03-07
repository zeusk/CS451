using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for CheckerBoardWindow.xaml
    /// </summary>
    public partial class CheckerBoardWindow : Page
    {
        //The player the the user is currently playing against
        //private string opponentName = GameBrowserWindow.playerId==1?GameState.player1Name:GameState.player2Name;
        private string opponentName = "oppo";
        private static GameClient gc = GameClient.GetInstance();
        private static List<int> movePair = new List<int>();
        private static List<int> chosenPiece = new List<int>();
        private static Color currentPlayerColor = new Color();
        public static CheckerBoardWindow Instance { get; private set; }

        public CheckerBoardWindow()
        {
            InitializeComponent();
            Instance = this;
            refreshBoard(generateCheckerBoardUI(240, gc.GetGameState(), GameBrowserWindow.playerId));
            currentPlayerColor = getColorForPlayer(GameBrowserWindow.playerId);

            if (GameBrowserWindow.playerId == 1)
            {
                turnToMoveText.Visibility = Visibility.Visible;
                playerColorCircle.Visibility = Visibility.Visible;
            }
            else
            {
                turnToMoveText.Visibility = Visibility.Hidden;
                playerColorCircle.Visibility = Visibility.Hidden;
            }
            playerColorCircle.Fill = new SolidColorBrush(currentPlayerColor);

            //Set player info on UI
            connectedPlayerName.Text = opponentName;

        }

        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = (GameClient)e.ExtraData;
            Debug.Write("gc is here");
        }

        private static Color getColorForPlayer(int id)
        {
            if (id == 1)
            {
                return (Colors.Red);
            }
            else
            {
                return (Colors.White);
            }
        }

        public static Grid generateCheckerBoardUI(int size, GameState gs, int playerId)
        {
            Grid myGrid = new Grid();
            myGrid.Height = size;
            myGrid.Width = size;
            int[,] myboard = gs.getBoard();
            //int [,] myboard = new int[,] { { 0,1,0,1,0,1,0,1}, {1,0,1,0,1,0,1,0}, 
              //  { 0, 1, 0, 1, 0, 1, 0, 1 }, {0,0,0,0,0,0,0,0}, { 0, 0, 0, 0, 0, 0, 0, 0 },
                //{2,0,2,0,2,0,2,0}, { 0, 2, 0, 2, 0, 2, 0, 2 }, {2,0,2,0,2,0,2,0} };

            // Create Columns
            List<ColumnDefinition> gridCols = new List<ColumnDefinition>();
            List<RowDefinition> gridRows = new List<RowDefinition>();
            for (int i = 0; i< 8; i++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(size/8);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(size/8);
                gridCols.Add(gridCol1);
                gridRows.Add(gridRow1);
            }
            
            foreach(ColumnDefinition c in gridCols)
            {
                myGrid.ColumnDefinitions.Add(c);
            }
            foreach (RowDefinition r in gridRows)
            {
                myGrid.RowDefinitions.Add(r);
            }

            Color backGroundColor = new Color();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j< 8; j++)
                {
                    if(i%2 == j% 2)
                    {
                        backGroundColor = Colors.Wheat;
                    }
                    else
                    {
                        backGroundColor = Colors.Green;
                    }
                    Button checkerboxButton = new Button();
                    checkerboxButton.SetValue(Grid.RowProperty, i);
                    checkerboxButton.SetValue(Grid.ColumnProperty, j);
                    checkerboxButton.Background = new SolidColorBrush(backGroundColor);
                    checkerboxButton.Tag = i.ToString() + " " + j.ToString();
                    checkerboxButton.Click += (s, e) => {
                        string coor = (string)((Button)s).Tag;
                        Debug.WriteLine(coor);
                        int row = Int32.Parse(coor.Split(' ')[0]);
                        int col = Int32.Parse(coor.Split(' ')[1]);
                        addMove(row, col, gs.cb);
                    };
                    double ellipSize = (size / 8) * 0.8;

                   
                    if(myboard[i, j] != 0)
                    {
                        Border OuterBorder = new Border();
                        OuterBorder.Width = ellipSize;
                        OuterBorder.Height = ellipSize;

                        if (myboard[i, j] == 1)
                        {
                            //player 1, regular
                            OuterBorder.Background = new SolidColorBrush(Colors.Red);
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 20);
                        }
                        else if (myboard[i, j] == 3)
                        {
                            //player 1, kinged
                            OuterBorder.Background = new SolidColorBrush(Colors.Red);
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 10);
                        }
                        else if (myboard[i, j] == 2)
                        {
                            //player 2, regular
                            OuterBorder.Background = new SolidColorBrush(Colors.White);
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Black);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 20);
                        }
                        else if (myboard[i, j] == 4)
                        {
                            //player 2, kinged
                            OuterBorder.Background = new SolidColorBrush(Colors.White);
                            OuterBorder.BorderBrush = new SolidColorBrush(Colors.Yellow);
                            OuterBorder.BorderThickness = new Thickness(ellipSize / 10);
                        }
                        OuterBorder.CornerRadius = new CornerRadius(20);
                        OuterBorder.HorizontalAlignment = HorizontalAlignment.Center;
                        checkerboxButton.Content = OuterBorder;
                    }

                    myGrid.Children.Add(checkerboxButton);
                }
            }
            if(playerId == 1)
            {
                //flip the board 180 degrees 
                RotateTransform myRotateTransform = new RotateTransform(180, 0.5, 0.5);
                myGrid.LayoutTransform = myRotateTransform;
            }
            return myGrid;
        }

        protected static void addMove(int i, int j, CheckerBoard cb)
        {
            if (movePair.Count == 0 )
            {
                movePair.Add(i);
                movePair.Add(j);
                Debug.WriteLine($"first click --"+ i+ " "+ j);
            }
            else if(movePair.Count == 2)
            {
                movePair.Add(i);
                movePair.Add(j);
                Debug.WriteLine($"second click-- " + movePair[0] + " "+ movePair[1] + " " +  movePair[2]+ " "+ movePair[3]);
                GameState newS = gc.GetGameState().applyMove(movePair, GameBrowserWindow.playerId);

                if (newS == null)
                {
                    MessageBox.Show("Your move was not valid");
                }
                else
                {
                    
                    Task<int> taskSendState = Task<int>.Factory.StartNew(() => gc.SendState(newS));
                    taskSendState.Wait();
                    if (taskSendState.Result != 0)
                    {
                        MessageBox.Show("Failed updating move");
                        movePair.Clear();
                        return;
                    }
                   
                    refreshBoard(generateCheckerBoardUI(240, gc.GetGameState(), GameBrowserWindow.playerId));
                    if (gc.GetGameState().getResult() != -1)
                    {
                        //Game end, go to end window
                        Instance.navigateToEndWindow();
                    }
                    else
                    {
                        int dist = (Math.Abs(movePair[2] - movePair[0]) + Math.Abs(movePair[3] - movePair[1]));
                        if (dist <= 2 || !gc.GetGameState().checkAvailableJump(movePair[2], movePair[3], GameBrowserWindow.playerId))
                        {
                            //----------------------------------------
                            //------------------------------------------
                            GameBrowserWindow.playerId = 3 - GameBrowserWindow.playerId;
                            Instance.playerColorCircle.Fill = new SolidColorBrush(getColorForPlayer(GameBrowserWindow.playerId));
                            //----------------------------------------------
                            //--------------------------------------------

                            Instance.turnToMoveText.Visibility = Visibility.Hidden;
                            Instance.playerColorCircle.Visibility = Visibility.Hidden;
                            Task<int> taskReceiveState = Task<int>.Factory.StartNew(() => gc.ReceiveState(newS));
                            taskReceiveState.Wait();
                            if (taskReceiveState.Result != 0)
                            {
                                //The other player drop, go to end window
                                Instance.navigateToEndWindow();
                            }
                            else
                            {
                                refreshBoard(generateCheckerBoardUI(240, gc.GetGameState(), GameBrowserWindow.playerId));
                                if (gc.GetGameState().getResult() != -1)
                                {
                                    //Game end, go to end window
                                    Instance.navigateToEndWindow();
                                }
                                else
                                {
                                    Instance.turnToMoveText.Visibility = Visibility.Visible;
                                    Instance.playerColorCircle.Visibility = Visibility.Visible;
                                }
                            } 
                        }
                    }
                    
                }
                movePair.Clear();
            }            
        }

        private static void refreshBoard(Grid newG)
        {
            Instance.dynamicGrid.Children.Clear();
            Instance.dynamicGrid.Children.Add(newG);
        }

        protected void navigateToEndWindow()
        {
            NavigationService.Navigate(new Uri("EndWindow.xaml", UriKind.Relative), gc);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
