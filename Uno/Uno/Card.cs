using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Uno
{
    [Serializable()]
    class Card
    {
        private string  mImageName;
        private ImageSource mImage;

        public Card (string pImageName, ImageSource pImage)
        {
            this.mImageName = pImageName;
            this.mImage = pImage;
        }
    }
}
