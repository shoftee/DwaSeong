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
    }

    static class CenterServerPacketDefinitions
    {
        public static byte[] IdentifySuccess(int id)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterSendOps.IdentifySuccess);
            packet.WriteInt(id);
            return packet.ToArray();
        }

        public static byte[] ChannelLoad()
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterSendOps.ChannelLoad);
            List<GameServer> chans = new List<GameServer>();
            foreach (GameServer serv in Program.mServer.gameServers.Values)
            {
                if (serv.ServerType == GameServerType.Game)
                    chans.Add(serv);
            }
            packet.WriteByte(chans.Count);
            foreach (GameServer serv in chans)
            {
                packet.WriteByte(serv.ID);
                packet.WriteInt(serv.ClientCount);
            }
            return packet.ToArray();
        }

        public static byte[] Migrate(int accid, int cid, byte[] ip, ushort port)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterSendOps.Migrate);
            packet.WriteInt(accid);
            packet.WriteInt(cid);
            packet.WriteBytes(ip);
            packet.WriteShort(port);
            return packet.ToArray();
        }
    }
}
