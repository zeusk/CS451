using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for EndWindow.xaml
    /// </summary>
    public partial class EndWindow : Page
    {
        //Check whether the user won or lost
        private bool currentPlayerWon;
        private GameClient gc = GameClient.getInstance();

        public EndWindow()
        {
            InitializeComponent();
            currentPlayerWon = (gc.getGameState().getResult() == GameBrowserWindow.playerId);

            //Generate the text to indicate won or lost
            if (currentPlayerWon)
            {
                gameResultWon.Visibility = Visibility.Visible;
            }else
            {
                gameResultLost.Visibility = Visibility.Visible;
            }
        }


        public void NavigateToGameBrowserWindow(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("GameBrowserWindow.xaml", UriKind.Relative));
        }

        //exit the game
        protected void CloseGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
