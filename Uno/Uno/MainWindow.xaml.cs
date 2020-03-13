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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Uno
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            EventPublisher.RaiseShutDownRoutine += MainWindow_ShutDown;
            UnoMain unoMain = new UnoMain();
            this.Hide();
        }

        private void MainWindow_ShutDown(object sender, EventArgs eventArgs)
        {
            EventPublisher.CloseWindow();
            Application.Current.Shutdown();
        }
    }
}
