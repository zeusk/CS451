using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Checkers
{
    public partial class StartWindow : Window
    {
        //Retrieve user name from local disc
        private string userName = Settings.getUserNameFromLocalDisc();
        public GameClient gc = new GameClient();

        public StartWindow()
        {
            InitializeComponent();

            //Set the user name field for popup window
            Popup popup = connectionPopup;
            popup.IsOpen = true;
            enteredUserName.Text = userName;
        }

        private void connectUserToServer(object sender, RoutedEventArgs e)
        {
            string ipAddress = enteredUserIPAddress.Text;
            userName = enteredUserName.Text;
            //Connect to the server
            this.gc.connect(ipAddress, userName);
            //Show text indicating connection to server
            connectingToServerText.Visibility = Visibility.Visible;


            //Go to next page if success
            navigateToGameBrowserPage();
        }

        private void navigateToGameBrowserPage()
        {
            Uri uri = new Uri("GameBrowserWindow.xaml", UriKind.Relative);
            NavigationService.Navigate(uri);
        }
    }
}
