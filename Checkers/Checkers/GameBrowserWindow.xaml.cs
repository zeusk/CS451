using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
    public partial class GameBrowserWindow : Window
    {
        public GameBrowserWindow()
        {
            InitializeComponent();
            generateListOfPlayers();
            generateListOfGames();
        }

        //Generate list of players
        private void generateListOfPlayers()
        {
            Array listOfPlayers = getOnlinePlayers();
            foreach (string name in listOfPlayers)
            {
                TextBox player = new TextBox();
                player.Text = name; 
                listOfPlayersPanel.Children.Add(player);
            }
        }


        //generate list of games
        private void generateListOfGames()
        {
            string[] gameIds = getGameIds();
            foreach (string id in gameIds)
            {
                StackPanel currentGame = generateGameOverview(id);
                listOfGamesPanel.Children.Add(currentGame);
            }
        }

        //Generate game overview
        private StackPanel generateGameOverview(string gameId)
        {
            StackPanel game = new StackPanel();

            //create the join button 
            Button joinButton = new Button();
            joinButton.Content = "join";
            joinButton.Width = 10;
            joinButton.Height = 10;
            joinButton.Click += (s, e) => {
                //Go to the main game page
                Uri uri = new Uri("CheckerBoardWindow.xaml", UriKind.Relative);
                NavigationService.Navigate(uri);
            };

            //gnerate the overview of the board
            Grid mygame = CheckerBoardWindow.generateCheckerBoardUI();
            mygame.Width = 25;
            mygame.Height = 25;

            //generate the name of the player who's in the game at the moment 
            string playerName = getPlayerNameByGame(gameId);
            TextBox player = new TextBox();
            player.Text = playerName;
            player.Width = 25;
            player.Height = 10;


            //Add all elements to the stack panel
            game.Orientation = Orientation.Horizontal;
            game.Children.Add(joinButton);
            game.Children.Add(mygame);
            game.Children.Add(player);

            return game;
        }

        //Start a new game
        private void startNewGame(object sender, RoutedEventArgs e)
        {
            //Go to the main game page
            Uri uri = new Uri("CheckerBoardWindow.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}
