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
        private GameClient gc;
        public static int playerId;

        public GameBrowserWindow()
        {
            InitializeComponent();
            generateListOfPlayers();
            generateListOfGames();
        }


        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = (GameClient)e.ExtraData;
            Debug.Write("gc is here");
        }

        //Generate list of players
        protected void generateListOfPlayers()
        {
            //List<String> listOfPlayers = gc.listPlayers();
            List<String> listOfPlayers = new List<string>();
            listOfPlayers.Add("Andy");
            listOfPlayers.Add("Marry");
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
            //List<GameState> allGames = gc.listGames();
            List<GameState> allGames = new List<GameState>();
            allGames.Add(new GameState());
            allGames.Add(new GameState());
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
            game.Width = 200;
            game.Height = 60;

            //create the join button 
            Button joinButton = new Button();
            joinButton.Content = "Join";
            joinButton.Width = 40;
            joinButton.Height = 20;
            joinButton.Click += (s, e) => {
                //Go to the main game page
                playerId = 2;
                NavigationService n = NavigationService.GetNavigationService(this);
                n.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative), gc);
            };
            joinButton.HorizontalAlignment = HorizontalAlignment.Left;


            //gnerate the overview of the board
            Grid mygame = CheckerBoardWindow.generateCheckerBoardUI(60, gs);

            mygame.HorizontalAlignment = HorizontalAlignment.Center;

            //generate the name of the player who's in the game at the moment 
            //GameState gs = go.getGameState();
            string playerName = "temp";
            TextBox player = new TextBox();
            player.Text = playerName;
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
            gc = new GameClient();
            playerId = 1;
            //Go to the main game page
            NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative), gc);
        }
    }
}
