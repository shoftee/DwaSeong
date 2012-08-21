/*
	This file is part of the RiceMS MapleStory Server
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
using Common;
using MapleLib.PacketLib;

namespace WvsGame.Center
{
    class CenterServerPacketDefinitions
    {
        public static byte[] Identify(byte[] key)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterRecvOps.Identify);
            packet.WriteByte(2);
            packet.WriteBytes(key);
            return packet.ToArray();
        }

        public static byte[] ConnectedClients(byte server, int count)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterRecvOps.ClientCount);
            packet.WriteByte(server);
            packet.WriteInt(count);
            return packet.ToArray();
        }

        public static byte[] PollConnectedClients()
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterRecvOps.PollChannelLoad);
            return packet.ToArray();
        }

        public static byte[] GuildInfo(int cid, int gid)
        {
            var packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterRecvOps.GuildOperation);
            packet.WriteInt(cid);
            packet.WriteInt(gid);
            return packet.ToArray();
        }
    }
}
