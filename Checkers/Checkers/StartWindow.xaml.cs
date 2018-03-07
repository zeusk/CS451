using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public partial class StartWindow : Page
    {
        //Retrieve user name from local disc
        private string userName = "Mike";
        //private string userName = Settings.getUserNameFromLocalDisc();
        private GameClient gc;

        public StartWindow()
        {
            InitializeComponent();

            //Set the user name field for popup window
            enteredUserName.Text = userName;
            connectionPopup.IsOpen = true;

            gc = GameClient.GetInstance();
        }

        protected void connectUserToServer(object sender, RoutedEventArgs e)
        {
            connectionPopup.IsOpen = false;

            //Show the text Connecting to server
            connectingToServerText.Visibility = Visibility.Visible;

            //Pass in user name and ip address
            string ipAddress = enteredUserIPAddress.Text;
            userName = enteredUserName.Text;

            //Connect to the server
            Task<int> taskConnect = Task<int>.Factory.StartNew( () => this.gc.Connect(ipAddress, userName) );

            taskConnect.Wait();

            if (taskConnect.Result == 0)
            {
                navigateToGameBrowserWindow(); // Continue if successfully connected to host
            } else if (taskConnect.Result == -2) {
                MessageBox.Show("User name taken. Please try again.");
                connectionPopup.IsOpen = true;
            } else if (taskConnect.Result == -3) {
                MessageBox.Show("Invalid IP address. Please try again.");
                connectionPopup.IsOpen = true;
            } else {
                MessageBox.Show("Connection failed.");
                connectionPopup.IsOpen = true;
            }
        }


        protected void navigateToGameBrowserWindow()
        {
             NavigationService.Navigate(new Uri("GameBrowserWindow.xaml", UriKind.Relative));
        }
    }
}
