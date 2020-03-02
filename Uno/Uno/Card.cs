using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace Uno
{
    [Serializable()]
    class Card
    {
        private string mImage;

        public Card (string pImage)
        {
            this.mImage = pImage;
        }
    }
}
