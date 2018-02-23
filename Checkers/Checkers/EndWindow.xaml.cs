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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for EndWindow.xaml
    /// </summary>
    public partial class EndWindow : Window
    {
        //Check whether the user won or lost
        private bool won = CheckerBoard.getResult;
        private GameState gc;

        public EndWindow()
        {
            InitializeComponent();

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
            gc = e;
        }

        private void natigateToGameBrowserWindow(object sender, RoutedEventArgs e)
        {
            NavigationService n = NavigationService.GetNavigationService(this);
            n.Navigate(new Uri("GameBrowserWindow.xaml", UriKind.Relative), gc);
        }

        //exit the game
        private void closeGame(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
