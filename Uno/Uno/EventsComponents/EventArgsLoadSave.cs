using System;
using System.Collections.Generic;
using System.Text;

namespace Uno.EventsComponents
{
    class EventArgsLoadSave
    {
        private string mName;
        private string mExtraInfo;

        public EventArgsLoadSave(string pName)
        {
            this.mName = pName;
            this.mExtraInfo = "";
        }

        public EventArgsLoadSave(string pName, string pExtraInfo)
        {
            this.mName = pName;
            this.mExtraInfo = pExtraInfo;
        }

        public string Name
        {
            get { return this.mName; }
        }

        public string ExtraInfo
        {
            get { return this.mExtraInfo; }
        }
    }
}
