using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WvsCenter
{
    public struct Guild
    {
        public string Name;
        public int Point;
        public int MemberCap;
        public GuildMember[] Members;
    }

    public struct GuildMember
    {
        public int CharacterID;
        public int Grade;
    }
}
