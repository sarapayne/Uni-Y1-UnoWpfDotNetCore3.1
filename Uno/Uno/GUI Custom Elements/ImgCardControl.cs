using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Uno.GUI_Custom_Elements
{
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
