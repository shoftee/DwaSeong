using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WvsGame.WZ
{
    public interface ICopyable<T>
    {
        T Copy();
    }
}
