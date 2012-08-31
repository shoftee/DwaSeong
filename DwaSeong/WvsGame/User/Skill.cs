using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common;

namespace WvsGame.User
{
    public class Skill : ICopyable<Skill>
    {
        public int SkillID { get; set; }
        public int SkillLevel { get; set; }
        public int SkillMastery { get; set; }
        public long Expiration { get; set; }
        public long Cooldown { get; set; }

        public List<Level> Levels { get; set; }

        public Dictionary<int, int> Req { get; set; }
        public int MasterLevel { get; set; }
        public int FinalAttack { get; set; }
        public int CombatOrders { get; set; }

        //common
        public int MaxLevel { get; set; }
        public string Cooltime { get; set; }           // formulas
        public string MpCon { get; set; }              // formulas
        public string Time { get; set; }               // formulas
        public string Mastery { get; set; }            // formulas
        public string X { get; set; }                  // formulas
        public string Y { get; set; }                  // formulas
        public string CriticaldamageMin { get; set; }  // formulas

        //info
        public int RapidAttack { get; set; }
        public int MagicSteal { get; set; }
        public int MassSpell { get; set; }

        public Skill(int skillid = 0)
        {
            SkillID = skillid;
            Levels = new List<Level>();
            Req = new Dictionary<int, int>();
        }

        public struct Level
        {
            public int SkillLevel;
            public int Time;
            public int Speed;
            public int Jump;
            public Point BoundsLT;
            public Point BoundsRB;
            public int Range;
            public int MobCount;
            public int Damage;
            public int PhysicalDamage;
            public int PhysicalDefense;
            public int MagicalDamage;
            public int MagicalDefense;
        }

        public Skill Copy()
        {
            Skill ret = new Skill();
            ret.SkillID = SkillID;
            ret.Levels = new List<Level>(Levels);
            Req = new Dictionary<int,int>(Req);
            MasterLevel = MasterLevel;
            FinalAttack = FinalAttack;
            CombatOrders = CombatOrders;
            MaxLevel = MaxLevel;
            Cooltime = Cooltime;
            MpCon = MpCon;
            Time = Time;
            Mastery = Mastery;
            X = X;
            Y = Y;
            CriticaldamageMin = CriticaldamageMin;
            RapidAttack = RapidAttack;
            MagicSteal = MagicSteal;
            MassSpell = MassSpell;
            return ret;
        }
    }
}
