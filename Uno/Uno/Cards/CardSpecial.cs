using System;
using Uno.Interfaces;
using Image = System.Windows.Controls.Image;

namespace Uno
{

    [Serializable()]
    abstract class CardSpecial: CardSuit
    {

        public CardSpecial(string pImageName, Suit pSuit): base (pImageName, pSuit)
        {
            
        }
    }
}
