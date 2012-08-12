using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WvsGame.Field.Entity
{
    public class Portal : FieldObject
    {
        public int mPortalID { get; set; }
        public string mName { get; set; }
        public PortalType mType { get; set; }
        public int mDestinationMapID { get; set; }
        public string mDestinationName { get; set; }
        public string mScriptName { get; set; }
        public int mOnce { get; set; }
    }
}
