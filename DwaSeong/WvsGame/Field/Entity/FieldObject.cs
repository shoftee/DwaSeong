using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using WvsGame.WZ;

namespace WvsGame.Field.Entity
{
    public abstract class FieldObject
    {
        public int mObjectID { get; set; }
        public Field mMap { get; set; }
        public Point mPosition { get; set; }

        protected FieldObject()
		{
            mObjectID = -1;
		}

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(mObjectID);
            sb.Append(GetType());
            sb.Append("," + mMap.FieldID);
            sb.Append("," + mPosition);
            return sb.ToString();
        }
    }
}
