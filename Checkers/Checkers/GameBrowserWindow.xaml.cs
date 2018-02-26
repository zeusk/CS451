using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
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
        //private int playerId = 1;
        private GameClient gc;
        public static int playerId;

        public GameBrowserWindow()
        {
            InitializeComponent();
            generateListOfPlayers();
            generateListOfGames();
        }


        protected void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            gc = e;
        }

        //Generate list of players
        protected void generateListOfPlayers()
        {
            List<String> listOfPlayers = gc.listPlayers();
            foreach (string name in listOfPlayers)
            {
                TextBox player = new TextBox();
                player.Text = name; 
                listOfPlayersPanel.Children.Add(player);
            }
        }


        //generate list of games
        protected void generateListOfGames()
        {
            List<GameState> allGames = gc.listGames();
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

            //create the join button 
            Button joinButton = new Button();
            joinButton.Content = "join";
            joinButton.Width = 10;
            joinButton.Height = 10;
            joinButton.Click += (s, e) => {
                //Go to the main game page
                playerId = 2;
                NavigationService n = NavigationService.GetNavigationService(this);
                n.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative), gc);
            };

            //gnerate the overview of the board
            Grid mygame = CheckerBoardWindow.generateCheckerBoardUI(gs);
            mygame.Width = 25;
            mygame.Height = 25;

            //generate the name of the player who's in the game at the moment 
            //GameState gs = go.getGameState();
            string playerName = "temp";
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
        protected void startNewGame(object sender, RoutedEventArgs e)
        {
            playerId = 1;
            //Go to the main game page
            NavigationService n = NavigationService.GetNavigationService(this);
            n.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative), gc);
        }
    }
}
