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
        private bool won;
        private GameClient gc = GameClient.getInstance();

        public EndWindow()
        {
            InitializeComponent();
            won = gc.getGameState().getResult();

            //Generate the text to indicate won or lost
            if (won)
            {
                gameResultWon.Visibility = Visibility.Visible;
            }else
            {
                gameResultLost.Visibility = Visibility.Visible;
            }
        }

        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            //gc = (GameClient)e.ExtraData;
            Debug.Write("gc is here");
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
