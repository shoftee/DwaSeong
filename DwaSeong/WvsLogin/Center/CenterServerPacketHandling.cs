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
using WvsLogin.Packets;
using WvsLogin.Networking;
using MapleLib.PacketLib;

namespace WvsLogin.Center
{
    class CenterServerPacketHandling
    {
        public static void HandleIdentify(PacketReader packet)
        {
            Program.mServer.Acceptor = new ClientAcceptor();
            Program.mServer.Acceptor.mGameServer = Program.mServer;
            Program.mServer.Acceptor.Listen();
        }

        public static void HandleChannelLoad(PacketReader packet, CenterServer serv)
        {
            byte channels = packet.ReadByte();
            var ret = new Dictionary<int, int>();
            for (int i = 0; i < channels; ++i)
            {
                ret.Add(packet.ReadByte(), packet.ReadInt());
            }
            serv.channels = ret;
        }

        public static void HandleMigrate(CenterServer serv, PacketReader packet)
        {
            int accid = packet.ReadInt();
            int cid = packet.ReadInt();
            byte[] ip = packet.ReadBytes(4);
            ushort port = (ushort)packet.ReadShort();
            if (port == 0)
                Program.mServer.GetClientByAccountId(accid).SendPacket(PacketDefinitions.SelectCharacter(0x0C, 0x1F, ip, port, cid));
            else
                Program.mServer.GetClientByAccountId(accid).SendPacket(PacketDefinitions.SelectCharacter(0x00, 0x00, ip, port, cid));
        }
    }
}
