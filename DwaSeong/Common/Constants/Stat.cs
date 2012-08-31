using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Constants
{
    public enum Stat : ulong
    {
        Skin = 0x01,
        Face = 0x02,
        Hair = 0x04,
        Level = 0x10,
        Job = 0x20,
        Str = 0x40,
        Dex = 0x80,
        Int = 0x100,
        Luk = 0x200,
        Hp = 0x400,
        MaxHp = 0x800,
        Mp = 0x1000,
        MaxMp = 0x2000,
        Ap = 0x4000,
        Sp = 0x8000,
        Exp = 0x10000,
        Fame = 0x20000,
        Meso = 0x40000,
        Pet = 0x180008,
        GachaponExp = 0x200000,
        Fatigue = 0x400000,
        Charisma = 0x800000,
        Insight = 0x1000000,
        Will = 0x2000000,
        Dilligence = 0x4000000,
        Empathy = 0x8000000,
        Charm = 0x10000000,
        TraitLimit = 0x20000000,
        BattleExp = 0x40000000,
        BattleRank = 0x80000000,
        BattlePoints = 0x100000000,
        IceGauge = 0x200000000,
        Virtue = 0x400000000
    }
}
