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
        System.Windows.Threading.DispatcherTimer dispatcherTimer;

        public GameBrowserWindow()
        {
            InitializeComponent();

            //Set a timer that fresh the game lobby
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0,0, 500);
            dispatcherTimer.Start();
        }

        //update list of games and list of players on UI
        protected void updateGUI()
        {
            listOfPlayersPanel.Children.Clear();
            generateListOfPlayers();
            listOfGamesPanel.Children.Clear();
            generateListOfGames();
        }

        //Call refresh UI
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            updateGUI();
            dispatcherTimer.Start();
        }


        //Generate list of players
        protected void generateListOfPlayers()
        {
            List<String> listOfPlayers = gc.ListPlayers();

            foreach (string name in listOfPlayers)
            {
                TextBlock player = new TextBlock();
                player.Width = 150;
                player.Height = 20;
                player.FontSize = 13;
                player.Text = name;
                player.Style = Application.Current.Resources["textBlockTemplate"] as Style;
                listOfPlayersPanel.Children.Add(player);
            }
        }


        //generate list of games
        protected void generateListOfGames()
        {
            List<GameState> allGames = gc.ListGames();

            allGames = allGames.Where(g => string.IsNullOrEmpty(g.player1Name) || string.IsNullOrEmpty(g.player2Name)).ToList();

            foreach (GameState gs in allGames)
                listOfGamesPanel.Children.Add(generateGameOverview(gs));
        }

        //jouns an existing game
        protected void joinGame(GameState gs)
        {
            dispatcherTimer.Stop(); // TODO: Restart timer if we come back to this page
            int r = gs == null ? gc.JoinGame() : gc.JoinGame(gs);
            MainWindow.MainFrame.NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
        }

        //Generate game overview
        protected Grid generateGameOverview(GameState gs)
        {
            Grid game = new Grid();
            for (int i = 0; i < 3; i++)
            {
                ColumnDefinition gridCol1 = new ColumnDefinition();
                gridCol1.Width = new GridLength(listOfGamesPanel.Width / 3);
                game.ColumnDefinitions.Add(gridCol1);
            }
            //create the join button 
            Button joinButton = new Button();

            joinButton.Content = "Join";
            joinButton.Height = 20;
            joinButton.Width = 50;
            joinButton.Style = Application.Current.Resources["buttonTemplate"] as Style;
            joinButton.Tag = gs;
            joinButton.Click += (s, e) => {
                joinGame((GameState) ((Button) s).Tag);
            };
            joinButton.SetValue(Grid.ColumnProperty, 0);

            //gnerate the overview of the board
            Border mygame = CheckerBoardWindow.generateCheckerBoardUI(70, gs, false);
            mygame.SetValue(Grid.ColumnProperty, 1);


            TextBlock player = new TextBlock();
            //Add the player's name who's currently in that game
            player.Text = string.IsNullOrEmpty(gs.player1Name) ? gs.player2Name : gs.player1Name;
            player.Width = 70;
            player.Height = 20;
            player.Style = Application.Current.Resources["textBlockTemplate"] as Style;
            player.SetValue(Grid.ColumnProperty, 2);

            //Add all elements to the stack panel
            game.Children.Add(joinButton);
            game.Children.Add(mygame);
            game.Children.Add(player);

            return game;
        }

        //Close the application and delete player from server
        private void CloseGame(object sender, RoutedEventArgs e)
        {
            gc.Disconnect();
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        //Start a new game
        protected void startNewGame(object sender, RoutedEventArgs e) => joinGame(null);
    }
}
