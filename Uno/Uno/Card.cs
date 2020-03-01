using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Controls;

namespace Uno
{
    class Card
    {
        enum Type
        {
            Suit,
            Wild
        }

        private Type mType;
        private Image mImage;
    }
}
