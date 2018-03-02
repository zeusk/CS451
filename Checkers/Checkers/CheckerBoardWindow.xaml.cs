using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for CheckerBoardWindow.xaml
    /// </summary>
    public partial class CheckerBoardWindow : Window
    {
        //The player the the user is currently playing against
        private string opponentName = GameBrowserWindow.playerId==1?GameState.player1Name:GameState.player2Name;
        private bool isMyTurn;
        private GameClient gc;
        private GameState gs;
        private static List<int> movePair = new List<int>();

        public CheckerBoardWindow()
        {
            InitializeComponent();
  
            checkerBoardGrid = generateCheckerBoardUI(gs);
            checkerBoardGrid.Margin = new Thickness(249, 25, 35, 61);
            checkerBoardGrid.Height = 240;
            checkerBoardGrid.Width = 240;


            //Set player info on UI
            connectedPlayerName.Text = opponentName;
            if(GameState.currentPlayer == 1)
            {
                playerColorCircle.Fill = new SolidColorBrush(Colors.Red);
                turnToMoveText.Visibility = Visibility.Visible;
                playerColorCircle.Visibility = Visibility.Visible;
            }
            else
            {
                playerColorCircle.Fill = new SolidColorBrush(Colors.Blue);
                turnToMoveText.Visibility = Visibility.Hidden;
                playerColorCircle.Visibility = Visibility.Hidden;
            }

            /*while (Game not end){
                //check if it's the user's turn
                //If it is, show texts, ask for move 
                //update the UI
            }*/


        }

        protected void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = e;
            gs = gc.receiveMove();
        }

        public static Grid generateCheckerBoardUI(GameState gs)
        {
            Grid myGrid = new Grid();
            int[,] myboard = gs.getBoard();

            // Create Columns
            List<ColumnDefinition> gridCols = new List<ColumnDefinition>();
            List<RowDefinition> gridRows = new List<RowDefinition>();
            for (int i = 1; i< 8; i++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(30);
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(30);
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
                    Shape ellips = new Ellipse() { Height = 20, Width = 20, HorizontalAlignment = HorizontalAlignment.Center };
                    ellips.SetValue(Grid.RowProperty, i);
                    ellips.SetValue(Grid.ColumnProperty, j);
                    Panel.SetZIndex(ellips, 1);
                    //Check if there is a checker piece on this box
                    if (myboard[i, j] == 1)
                    {
                        //player 1, regular
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = 2;
                    }
                    else if(myboard[i, j] == 2)
                    {
                        //player 1, kinged
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = 4;
                    }
                    else if(myboard[i, j] == 3)
                    {
                        //player 2, regular
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = 2;
                    }
                    else if (myboard[i, j] == 4)
                    {
                        //player 2, kinged
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = 4;
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
                int[,] newS = cb.applyMove(movePair);
                movePair.Clear();
                if (newS != null)
                {
                    //update game view
                }
                else
                {
                    MessageBox.Show("Your move was not valid");
                }
            }
        }

        protected void navigateToEndWindow(object sender, RoutedEventArgs e)
        {
            NavigationService n = NavigationService.GetNavigationService(this);
            n.Navigate(new Uri("EndWindow.xaml", UriKind.Relative), gc);
        }

    }
}
