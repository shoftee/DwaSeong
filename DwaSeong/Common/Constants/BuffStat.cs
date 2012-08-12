using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    enum BuffStat
    {
        WAtk = 0X01,
        WDef = 0x02,
        MAtk = 0x04,
        MDef = 0x08,
        Accuracy = 0x10,
        Avoid = 0x20,
        Hands = 0x40,
        Speed = 0x80,
        Jump = 0x100,

        MagicGuard = 0x200,
        DarkSight = 0x400,
        MaxHp = 0x2000,
        MaxMp = 0x8000,
    }
}
