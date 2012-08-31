using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WvsLogin.WZ
{
    public interface ICopyable<T>
    {
        T Copy();
    }
}
