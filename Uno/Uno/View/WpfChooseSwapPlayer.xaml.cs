using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Uno.View
{
    /// <summary>
    /// Interaction logic for WpfChooseSwapPlayer.xaml
    /// </summary>
    public partial class WpfChooseSwapPlayer : Window
    {
        List<Player> mPlayers;

        public WpfChooseSwapPlayer()
        {
            InitializeComponent();
            mPlayers = new List<Player>();
        }
    }
}
