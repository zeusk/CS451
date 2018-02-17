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
        private bool won = getResult();

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

        private void natigateToGameBrowserWindow(object sender, RoutedEventArgs e)
        {
            Uri uri = new Uri("GameBrowserWindow.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }

        //exit the game
        private void closeGame(object sender, RoutedEventArgs e)
        {
            Application.Exit();
        }
    }
}
