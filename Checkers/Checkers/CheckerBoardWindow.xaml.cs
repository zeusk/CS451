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
using System.Windows.Shapes;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for CheckerBoardWindow.xaml
    /// </summary>
    public partial class CheckerBoardWindow : Window
    {
        //The player the the user is currently playing against
        private string playerName = getConnectedPlayerName();

        //Check if the player is 0(red) or 1(wheat)
        private int player = getCurrentPlayer();

        public CheckerBoardWindow()
        {
            InitializeComponent();
            CheckerBoard myboard = CheckerBoard.getBoard();
            generateCheckerBoardUI(myboard);

            //Set player info on UI
            connectedPlayerName.Text = playerName;
            if(player == 0)
            {
                playerColorCircle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Red); 
            }else
            {
                playerColorCircle.Fill = new SolidColorBrush(System.Windows.Media.Colors.Wheat);
            }
        }

        public static StackPanel generateCheckerBoardUI(CheckerBoard myboard)
        {
            Grid board = new Grid();
            return board;
        }

    }
}
