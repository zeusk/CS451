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
        //private int playerId = 1;
        private GameClient gc = GameClient.getInstance();
        public static int playerId;

        public GameBrowserWindow()
        {
            InitializeComponent();
            generateListOfPlayers();
            generateListOfGames();
        }


        //Generate list of players
        protected void generateListOfPlayers()
        {
            List<String> listOfPlayers = gc.listPlayers();
            //List<String> listOfPlayers = new List<string>();
            //listOfPlayers.Add("Andy");
            //listOfPlayers.Add("Marry");
            foreach (string name in listOfPlayers)
            {
                TextBlock player = new TextBlock();
                player.Width = 150;
                player.Height = 40;
                player.FontSize = 20;
                player.Style = Application.Current.Resources["textBlockTemplate"] as Style;
                player.Text = name; 
                listOfPlayersPanel.Children.Add(player);
            }
        }


        //generate list of games
        protected void generateListOfGames()
        {
            List<GameState> allGames = gc.listGames();
            //List<GameState> allGames = new List<GameState>();

            foreach (GameState gs in allGames)
            {
                StackPanel currentGame = generateGameOverview(gs);
                listOfGamesPanel.Children.Add(currentGame);
            }
        }

        //Generate game overview
        protected StackPanel generateGameOverview(GameState gs)
        {
            StackPanel game = new StackPanel();
            game.Width = 300;
            game.Height = 100;

            //create the join button 
            Button joinButton = new Button();
            joinButton.Content = "Join";
            joinButton.Height = 60;
            joinButton.Width = 70;
            joinButton.Style = Application.Current.Resources["buttonTemplate"] as Style; ;
            joinButton.Click += (s, e) => {
                //Go to the main game page
                playerId = 2;
                gc.joinGame(gs);
                NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
            };
            joinButton.HorizontalAlignment = HorizontalAlignment.Left;

            //gnerate the overview of the board
            Border mygame = CheckerBoardWindow.generateCheckerBoardUI(90, gs, playerId);

            mygame.HorizontalAlignment = HorizontalAlignment.Center;

            //generate the name of the player who's in the game at the moment 
            string playerName = gs.player1Name;
            TextBlock player = new TextBlock();
            player.Text = playerName;
            player.Width = 100;
            player.Height = 60;
            player.Style = Application.Current.Resources["textBlockTemplate"] as Style;
            player.HorizontalAlignment = HorizontalAlignment.Right;

            //Add all elements to the stack panel
            game.Orientation = Orientation.Horizontal;
            game.Children.Add(joinButton);
            game.Children.Add(mygame);
            game.Children.Add(player);

            return game;
        }

        private void CloseGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        //Start a new game
        protected void startNewGame(object sender, RoutedEventArgs e)
        {
            playerId = 1;
            //Go to the main game page
            gc.joinGame();
            NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
        }
    }
}
