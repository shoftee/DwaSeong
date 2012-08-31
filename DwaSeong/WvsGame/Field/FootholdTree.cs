using System.Collections.Generic;

namespace WvsGame.Field
{
    public class FootholdTree
    {
        public Field mField { get; set; }
        public List<Foothold> mFootholds { get; set; }

        public FootholdTree()
        {
            mFootholds = new List<Foothold>();
        }
    }
}
