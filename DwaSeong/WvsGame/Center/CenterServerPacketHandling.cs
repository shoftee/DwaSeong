/*
	This file is part of the DwaSeong MapleStory Server
    Copyright (C) 2012 "XiaoKia" <xiaokia@naver.com> 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation. You may not use, modify
    or distribute this program under any other version of the
    GNU Affero General Public License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsGame.Networking;
using MapleLib.PacketLib;
using Common;

namespace WvsGame.Center
{
    class CenterServerPacketHandling
    {
        public static void HandleIdentify(PacketReader packet)
        {
            int id = packet.ReadInt();
            Program.mServer.ServerId = id;
            Program.mServer.Acceptor = new ClientAcceptor();
            Program.mServer.Acceptor.mGameServer = Program.mServer;
            Program.mServer.Acceptor.Listen();
        }

        public static void HandleGuildOperation(PacketReader packet)
        {
            int cid = packet.ReadInt();
            int gid = packet.ReadInt();
            string name = packet.ReadMapleString();
            int point = packet.ReadInt();
            int membercap = packet.ReadInt();
            int emblemBG = packet.ReadShort();
            int emblemBGC = packet.ReadShort();
            int emblem = packet.ReadShort();
            int emblemC = packet.ReadShort();
            int membercount = packet.ReadInt();
            List<GuildMember> mems = new List<GuildMember>();
            for (int i = 0; i < membercount; ++i)
            {
                var member = new GuildMember();
                member.CharacterID = packet.ReadInt();
                member.Grade = packet.ReadInt();
                member.GuildID = gid;
                mems.Add(member);
            }

            if (membercap == 0)
                ++membercap;
            var guild = new Guild();
            guild.Name = name;
            guild.GuildID = gid;
            guild.Point = point;
            guild.MemberCap = membercap;
            guild.EmblemBG = emblemBG;
            guild.EmblemBGColour = emblemBGC;
            guild.Emblem = emblem;
            guild.EmblemColour = emblemC;
            guild.Members = mems.ToArray();
            Program.mServer.GetClient(cid).mCharacter.mGuild = guild;
        }
    }
}
