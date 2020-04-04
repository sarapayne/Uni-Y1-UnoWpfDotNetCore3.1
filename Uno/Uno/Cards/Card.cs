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
    abstract class Card
    {
        private string  mImageName;
        private int mUniqueItentifier;
        
        [NonSerialized]
        private BitmapImage mImage;

        public Card()
        {

        }

        public Card (string pImageName)
        {
            this.mImageName = pImageName;
            RestoreCardImage();
        }

        public string ImageName
        {
            get { return mImageName; }
        }

        public int UniqueIdentifier
        {
            get { return this.mUniqueItentifier; }
            set { this.mUniqueItentifier = value; }
        }

        public BitmapImage Image
        {
            get { return this.mImage; }
        }

        public void RestoreCardImage()
        {
            this.mImage = Utilities.GetPlayerCardImage(mImageName);
        }
    }
}
