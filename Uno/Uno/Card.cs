using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;

namespace Uno
{
    [Serializable()]
    class Card
    {
        [NonSerialized]
        private string  mImageName;

        [NonSerialized]
        private Image mImage;

        public Card()
        {

        }

        public Card (string pImageName, Image pImage)
        {
            this.mImageName = pImageName;
            this.mImage = pImage;
        }

        public Image Image
        {
            get { return mImage; }
        }

        public string ImageName
        {
            get { return mImageName; }
        }
    }
}
