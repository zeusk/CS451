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

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            dispatcherTimer.Start();
        }

        protected void updateGUI()
        {
            listOfPlayersPanel.Children.Clear();
            generateListOfPlayers();
            listOfGamesPanel.Children.Clear();
            generateListOfGames();
        }

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
                player.Height = 40;
                player.FontSize = 20;
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

        protected void joinGame(GameState gs)
        {
            dispatcherTimer.Stop(); // TODO: Restart timer if we come back to this page
            int r = gs == null ? gc.JoinGame() : gc.JoinGame(gs);
            MainWindow.MainFrame.NavigationService.Navigate(new Uri("CheckerBoardWindow.xaml", UriKind.Relative));
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
            joinButton.Style = Application.Current.Resources["buttonTemplate"] as Style;
            joinButton.Tag = gs;
            joinButton.Click += (s, e) => {
                joinGame((GameState) ((Button) s).Tag);
            };
            joinButton.HorizontalAlignment = HorizontalAlignment.Left;

            //gnerate the overview of the board
            Border mygame = CheckerBoardWindow.generateCheckerBoardUI(90, gs, false);

            mygame.HorizontalAlignment = HorizontalAlignment.Center;

            TextBlock player = new TextBlock();

            player.Text = string.IsNullOrEmpty(gs.player1Name) ? gs.player2Name : gs.player1Name;
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
            if (gc.inGame)
                gc.QuitGame();
            gc.Disconnect();
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }

        //Start a new game
        protected void startNewGame(object sender, RoutedEventArgs e) => joinGame(null);
    }
}
