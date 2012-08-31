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
using Common;
using MapleLib.PacketLib;
using MapleLib.MapleCryptoLib;

namespace WvsCenter
{
    static class CenterServerPacketHandling
    {
        public static void Identify(GameServerConnection con, PacketReader packet)
        {
            byte serverType = packet.ReadByte();
            byte[] key = packet.ReadBytes(128);
            con.Verified = true;

            for (int i = 0; i < 128; i++)
                if (key[i] != Config.CenterServerKey[i])
                {
                    Console.WriteLine("byte{0} != {1} position: {2}", key[i], Config.CenterServerKey[i], i);
                    con.Verified = false;
                }
            if (con.Verified == false)
            {
                Logger.Write(Logger.LogTypes.오류, "Server at " + con.mSession.Socket.RemoteEndPoint.ToString() + " failed hash check.");
                return;
            }
            con.ServerType = serverType;
            con.Verified = true;
            switch (serverType)
            {
                case 0x01:
                    foreach (GameServer serv in Program.mServer.gameServers.Values)
                        if (serv.Connection == null && serv.ServerType == GameServerType.Login)
                        {
                            con.mGameServer = serv;
                            serv.Connection = con;
                        }
                    break;
                case 0x02:
                    foreach (GameServer serv in Program.mServer.gameServers.Values)
                        if (serv.Connection == null && serv.ServerType == GameServerType.Game)
                        {
                            con.mGameServer = serv;
                            serv.Connection = con;
                            break;
                        }
                    break;
            }
            con.mSession.SendPacket(CenterServerPacketDefinitions.IdentifySuccess(con.mGameServer.ID));
        }

        public static void ClientCount(GameServerConnection con, PacketReader packet)
        {
            byte serverid = packet.ReadByte();
            int count = packet.ReadInt();

            foreach (GameServer serv in Program.mServer.gameServers.Values)
                if (serv.ID == serverid)
                    serv.ClientCount = count;
        }
        

        public static void ChannelLoad(GameServerConnection con, PacketReader packet)
        {
            con.mSession.SendPacket(CenterServerPacketDefinitions.ChannelLoad());
        }

        public static void Migrate(GameServerConnection con, PacketReader packet)
        {
            int accountid = packet.ReadInt();
            int cid = packet.ReadInt();
            int channel = packet.ReadInt();
            GameServer serv = Program.mServer.GetGameServerById(channel);
            if (serv == null)
                con.mSession.SendPacket(CenterServerPacketDefinitions.Migrate(accountid, cid, new byte[4], 0));
            else
                con.mSession.SendPacket(CenterServerPacketDefinitions.Migrate(accountid, cid, System.Net.IPAddress.Parse(serv.PublicIP).GetAddressBytes(), serv.port));
        }

        public static void GuildOperation(GameServerConnection con, PacketReader packet)
        {
            int cid = packet.ReadInt();
            int gid = packet.ReadInt();
            var guild = Database.GetGuild(gid);
            con.mSession.SendPacket(CenterServerPacketDefinitions.GuildOperation(cid, guild));
        }
    }
}
