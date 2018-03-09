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
        public StartWindow()
        {
            InitializeComponent();

            // Set the user name field for popup window
            enteredUserName.Text = Util.GetMyName();
            connectionPopup.IsOpen = true;
        }

        protected void connectUserToServer(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(enteredUserName.Text))
            {
                MessageBox.Show("Invalid username. Please try again.");
                return;
            }

            connectionPopup.IsOpen = false;

            // Show the text Connecting to server
            connectingToServerText.Visibility = Visibility.Visible;

            // Pass in user name and ip address
            string ipAddress = enteredUserIPAddress.Text;
            Util.SetMyName(enteredUserName.Text);

            // Connect to the server
            Task<int> taskConnect = Task<int>.Factory.StartNew( () => GameClient.GetInstance().Connect(ipAddress));

            taskConnect.Wait();

            if (taskConnect.Result == 0)
                navigateToGameBrowserWindow(); // Continue if successfully connected to host
            else {
                connectingToServerText.Visibility = Visibility.Hidden;

                if (taskConnect.Result == -2)
                    MessageBox.Show("User name taken. Please try again.");
                else if (taskConnect.Result == -3)
                    MessageBox.Show("Invalid IP address. Please try again.");
                else
                    MessageBox.Show("Connection failed.");

                connectionPopup.IsOpen = true;
            }
        }

        //Go to the game lobby
        protected void navigateToGameBrowserWindow()
        {
            MainWindow.MainFrame.NavigationService.Navigate(new Uri("GameBrowserWindow.xaml", UriKind.Relative));
        }

        //Close the application and delete player from server
        private void CloseGame(object sender, RoutedEventArgs e)
        {
            if (GameClient.GetInstance().inGame)
                GameClient.GetInstance().QuitGame();
            GameClient.GetInstance().Disconnect();
            Properties.Settings.Default.Save();
            Application.Current.Shutdown();
        }
    }
}
