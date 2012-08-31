using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public struct Guild
    {
        public int GuildID;
        public string Name;
        public int Point;
        public int MemberCap;
        public long Created;
        public int Emblem;
        public int EmblemColour;
        public int EmblemBG;
        public int EmblemBGColour;
        public GuildMember[] Members;
    }

    public struct GuildMember
    {
        public int GuildID;
        public int CharacterID;
        public int Grade;
    }
}
