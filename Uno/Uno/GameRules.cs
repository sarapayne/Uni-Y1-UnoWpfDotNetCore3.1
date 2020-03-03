using System;
using System.Collections.Generic;
using System.Text;

namespace Uno
{
    [Serializable()]
    enum GameRulesType
    {
        Standard, House1, House2, House3
    }

    [Serializable()]
    class GameRules
    {
        //All methods in here need to be virutal, so they can be over ridden
    }
}
