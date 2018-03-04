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

            GameClient.init();
            gc = GameClient.getInstance();
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
            int isConnected = taskConnect.Result;
            //int isConnected = 0;
            if (taskConnect.Status == TaskStatus.Faulted || (isConnected != 0 && isConnected != -1))
            //if ( isConnected != 0 && isConnected != -1)
            {
                MessageBox.Show("Connection failed. Please try again.");
                connectionPopup.IsOpen = true;
            }
            else
            {
                //Go to next page if success
                navigateToGameBrowserWindow();
            }
            
            
        }


        protected void navigateToGameBrowserWindow()
        {
             NavigationService.Navigate(new Uri("GameBrowserWindow.xaml", UriKind.Relative));
        }
    }
}
