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
        private GameState gc;
        private static List<int> movePair = new List<int>();

        public CheckerBoardWindow()
        {
            InitializeComponent();
            CheckerBoard myboard = CheckerBoard.getBoard();
            checkerBoardGrid = generateCheckerBoardUI(myboard);
            checkerBoardGrid.Margin = new Thickness(249, 25, 35, 61);
            checkerBoardGrid.Height = 240;
            checkerBoardGrid.Width = 240;


            //Set player info on UI
            connectedPlayerName.Text = opponentName;
            if(GameState.currentPlayer == 1)
            {
                playerColorCircle.Fill = new SolidColorBrush(Colors.Red); 
            }else
            {
                playerColorCircle.Fill = new SolidColorBrush(Colors.Wheat);
            }
        }

        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = e;
        }

        public static Grid generateCheckerBoardUI(CheckerBoard cb)
        {
            Grid myGrid = new Grid();

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

            int[,] board = cb.getBoard();
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
                            addMove(i, j);
                        }
                    };
                    Shape ellips = new Ellipse() { Height = 20, Width = 20, HorizontalAlignment = HorizontalAlignment.Center };
                    ellips.SetValue(Grid.RowProperty, i);
                    ellips.SetValue(Grid.ColumnProperty, j);
                    Panel.SetZIndex(ellips, 1);
                    //Check if there is a checker piece on this box
                    if (board[i, j] == 1)
                    {
                        //player 1, regular
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = 4;
                    }else if(board[i, j] == 2)
                    {
                        //player 1, kinged
                        ellips.Fill = new SolidColorBrush(Colors.Red);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = 2;
                    }
                    else if(board[i, j] == 3)
                    {
                        //player 2, regular
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Orange);
                        ellips.StrokeThickness = 4;
                    }
                    else if (board[i, j] == 4)
                    {
                        //player 2, kinged
                        ellips.Fill = new SolidColorBrush(Colors.White);
                        ellips.Stroke = new SolidColorBrush(Colors.Black);
                        ellips.StrokeThickness = 2;
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

        private static void addMove(int i, int j)
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
                GameState newS = GameState.makeMove(movePair);
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

    }
}
