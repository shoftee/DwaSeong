using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsGame.User;

namespace WvsGame.Field.Entity
{
    public class FieldItem : FieldObject
    {
        public AbstractItem mItem { get; set; }
        public int mMeso { get; set; }
    }
}
