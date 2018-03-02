using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        private static GameClient gc;
        private static GameState gs;
        private static List<int> movePair = new List<int>();
        private static Color currentPlayerColor = new Color();

        public CheckerBoardWindow()
        {
            InitializeComponent();
            
            checkerBoardGrid.Children.Add(generateCheckerBoardUI(240, gs));
            checkerBoardGrid.Margin = new Thickness(249, 25, 35, 61);

            if(GameBrowserWindow.playerId == 1)
            {
                currentPlayerColor = Colors.Red;
            }
            else
            {
                currentPlayerColor = Colors.White;
            }
            playerColorCircle.Fill = new SolidColorBrush(currentPlayerColor);

            //Set player info on UI
            connectedPlayerName.Text = opponentName;
            

            //while (Game not end){
            for (int i = 0; i< 3; i++) {
                //check if it's the user's turn
                //if (GameState.currentPlayer == GameBrowserWindow.playerId)
                if(true)
                {
                    turnToMoveText.Visibility = Visibility.Visible;
                    playerColorCircle.Visibility = Visibility.Visible;
                }
                else
                {
                    turnToMoveText.Visibility = Visibility.Hidden;
                    playerColorCircle.Visibility = Visibility.Hidden;
                }

            }
            //navigateToEndWindow();

        }

        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = (GameClient)e.ExtraData;
            Debug.Write("gc is here");
        }

        public static Grid generateCheckerBoardUI(int size, GameState gs)
        {
            Grid myGrid = new Grid();
            myGrid.Height = size;
            myGrid.Width = size;
            //int[,] myboard = gs.getBoard();
            int [,] myboard = new int[,] { { 0,1,0,1,0,1,0,1}, {1,0,1,0,1,0,1,0}, 
                { 0, 1, 0, 1, 0, 1, 0, 1 }, {0,0,0,0,0,0,0,0}, { 0, 0, 0, 0, 0, 0, 0, 0 },
                {3,0,3,0,3,0,3,0}, { 0, 3, 0, 3, 0, 3, 0, 3 }, {3,0,3,0,3,0,3,0} };

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
                    checkerboxButton.Background = new SolidColorBrush(backGroundColor); ;
                    checkerboxButton.Click += (s, e) => {
                        if(GameState.currentPlayer == GameBrowserWindow.playerId)
                        {
                            addMove(i, j, gs.cb);
                        }
                    };
                    double ellipSize = (size / 8) * 0.8;
                    Shape ellips = new Ellipse() { Height = ellipSize, Width = ellipSize, HorizontalAlignment = HorizontalAlignment.Center };
                    ellips.SetValue(Grid.RowProperty, i);
                    ellips.SetValue(Grid.ColumnProperty, j);
                    Panel.SetZIndex(ellips, 1);
                    //Check if there is a checker piece on this box
                    if (myboard[i, j] == 1)
                    {
                        //player 1, regular
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = ellipSize/10;
                    }
                    else if(myboard[i, j] == 2)
                    {
                        //player 1, kinged
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = ellipSize/5;
                    }
                    else if(myboard[i, j] == 3)
                    {
                        //player 2, regular
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = ellipSize/10;
                    }
                    else if (myboard[i, j] == 4)
                    {
                        //player 2, kinged
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = ellipSize/5;
                    }
                    else
                    {
                        Panel.SetZIndex(ellips, 0);
                    }

                    myGrid.Children.Add(checkerboxButton);
                    myGrid.Children.Add(ellips);
                }
            }
            return myGrid;
        }

        protected static void addMove(int i, int j, CheckerBoard cb)
        {
            if (movePair.Count == 0 )
            {
                movePair.Add(i);
                movePair.Add(j);
            }
            else if(movePair.Count == 2)
            {
                movePair.Add(i);
                movePair.Add(j);
                int[,] newS = gs.cb.applyMove(movePair);
                movePair.Clear();
                if (newS == null)
                {
                    MessageBox.Show("Your move was not valid");
                }
                else
                {
                    GameState.currentPlayer = 3 - GameBrowserWindow.playerId;
                }
            }
        }

        protected void navigateToEndWindow()
        {
            gc = new GameClient();
            NavigationService.Navigate(new Uri("EndWindow.xaml", UriKind.Relative), gc);
        }

    }
}
