using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for GameBrowserWindow.xaml
    /// </summary>
    public partial class GameBrowserWindow : Page
    {
        private GameClient gc = GameClient.GetInstance();

        public GameBrowserWindow()
        {
            InitializeComponent();

            generateListOfPlayers();
            generateListOfGames();
        }

        //Generate list of players
        protected void generateListOfPlayers()
        {
            List<String> listOfPlayers = gc.ListPlayers();

            foreach (string name in listOfPlayers)
            {
                TextBox player = new TextBox();

                player.Width = 100;
                player.Height = 30;
                player.FontSize = 15;
                player.HorizontalAlignment = HorizontalAlignment.Left;
                player.BorderThickness = new Thickness(0);
                player.Text = name;

                listOfPlayersPanel.Children.Add(player);
            }
        }


        //generate list of games
        protected void generateListOfGames()
        {
            List<GameState> allGames = gc.ListGames();

            foreach (GameState gs in allGames)
                listOfGamesPanel.Children.Add(generateGameOverview(gs));
        }

        //Generate game overview
        protected StackPanel generateGameOverview(GameState gs)
        {
            StackPanel game = new StackPanel();
            game.Width = 200;
            game.Height = 60;

            //create the join button 
            Button joinButton = new Button();

            joinButton.Content = "Join";
            joinButton.Width = 40;
            joinButton.Height = 20;
            joinButton.Click += (s, e) => {
                gc.JoinGame(gs);
                NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
            };
            joinButton.HorizontalAlignment = HorizontalAlignment.Left;

            //gnerate the overview of the board
            Grid mygame = CheckerBoardWindow.generateCheckerBoardUI(60, gs);

            mygame.HorizontalAlignment = HorizontalAlignment.Center;

            TextBox player = new TextBox();

            player.Text = gs.player1Name; // string.IsNullOrEmpty(gs.player1Name) ? gs.player2Name : gs.player1Name;
            player.Width = 80;
            player.Height = 20;
            player.BorderThickness = new Thickness(0);
            player.HorizontalAlignment = HorizontalAlignment.Right;

            //Add all elements to the stack panel
            game.Orientation = Orientation.Horizontal;
            game.Children.Add(joinButton);
            game.Children.Add(mygame);
            game.Children.Add(player);

            return game;
        }

        //Start a new game
        protected void startNewGame(object sender, RoutedEventArgs e)
        {
            //Go to the main game page
            gc.JoinGame();
            NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
        }
    }
}
