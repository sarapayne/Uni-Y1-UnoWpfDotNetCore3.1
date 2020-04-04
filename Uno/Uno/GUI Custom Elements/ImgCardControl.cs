using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Uno.GUI_Custom_Elements
{
    /// <summary>
    /// An easy way of having full card details in every GUI element which looks like a playing card. It just adds a CARD member parameter
    /// to an Image WPF gui element. If being built from scratch to work this needs the project building before the main program is compiled. 
    /// </summary>
    [Serializable]
    class ImgCardControl:Image
    {
        private Card mCard;

        public ImgCardControl()
        {
            
        }

        public ImgCardControl(Card pCard)
        {
            this.mCard = pCard;
        }

        public Card Card
        {
            get { return this.mCard; }
            set { this.mCard = value; }
        }
            
    }

}
