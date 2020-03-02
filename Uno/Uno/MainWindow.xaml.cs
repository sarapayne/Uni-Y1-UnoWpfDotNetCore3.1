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
    [Serializable()]
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UnoGame unoGame; //central point to be saved and restored to. Will allow easy imaging of any game state. 
        [NonSerialized]
        private List<Card> cardDeck;
        static string dictionaryFileName = "CardDeck.Bin";

        public MainWindow()
        {
            InitializeComponent();
            UnoMain.LoadCardDeck();
        }

        
    }
}
