using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WvsGame.Field.Entity
{
    public class Seat : FieldObject
    {
        public int mID { get; set; }
        public Point Position { get; set; }
    }
}
