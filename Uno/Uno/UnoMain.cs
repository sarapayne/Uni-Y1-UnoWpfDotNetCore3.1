using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Uno;

namespace Uno
{
    class UnoMain : System.Windows.Application
    {
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public void InitializeComponent()
        {

#line 5 "..\..\..\App.xaml"
            this.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

#line default
#line hidden
        }

        static UnoGame unoGame; //central point to be saved and restored to. Will allow easy imaging of any game state. 
        static Dictionary<int, Card> deck; //dictionary with all 108 cards present. Everything else will references this for information by key (ints). //will allow for one off generation of the dictionary and binary save/restore there after. 
        static string dictionaryFileName = "Dictionary.Bin";

        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.8.1.0")]
        public static void Main()
        {
            deck = new Dictionary<int, Card>();
            LoadDictionary();
            Uno.App app = new Uno.App();
            app.InitializeComponent();
            app.Run();
        }

        static void LoadDictionary()
        {
            try
            {
                using (Stream stream = File.Open (dictionaryFileName, FileMode.Open  ))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    deck = (Dictionary<int, Card>)bin.Deserialize(stream);
                }
            }
            catch(IOException)
            {
                MessageBox.Show("There was an error loading saved settings, generating new settings, if this is the first time you used this software, this is to be expected", "Dictionary load error");
                GenerateCardDictionary();
            }
        }

        static void GenerateCardDictionary()
        {
            //add code here to generate the dictionary.
            SaveDictionary();
        }

        static void SaveDictionary()
        {
            try
            {
                using (Stream stream = File.Open(dictionaryFileName, FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, deck);
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Sorry there was an error saving the setup files, is the drive writable?", "Save dictionary error");
            }
        }
    }
}
