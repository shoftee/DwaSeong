using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WvsGame.Field
{
    public class Foothold
    {
        public FootholdTree mFootholdTree { get; set; }
        public int mID { get; set; }
        public int mLayerID { get; set; }
        public int mGroupID { get; set; }
        public Point mStart { get; set; }
        public Point mEnd { get; set; }
        public int mPrevious { get; set; }
        public int mNext { get; set; }
        public int mForce { get; set; }
        public int mForbidFallDown { get; set; }
        public int mPiece { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(mID);
            sb.Append("," + mLayerID);
            sb.Append("," + mGroupID);
            sb.Append("," + mStart);
            sb.Append("," + mEnd);
            sb.Append("," + mPrevious);
            sb.Append("," + mNext);
            sb.Append("," + mForce);
            sb.Append("," + mForbidFallDown);
            sb.Append("," + mPiece);
            return sb.ToString();   
        }
    }
}
