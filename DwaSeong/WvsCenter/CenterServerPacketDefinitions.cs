using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using MapleLib.PacketLib;
using MapleLib.MapleCryptoLib;

namespace WvsCenter
{
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

        public static byte[] GuildOperation(int cid, Guild guild)
        {
            var packet = new PacketWriter();
            packet.WriteCenterServerOpcode(CenterSendOps.GuildOperation);
            packet.WriteInt(cid);
            packet.WriteInt(guild.GuildID);
            packet.WriteMapleString(guild.Name ?? string.Empty);
            packet.WriteInt(guild.Point);
            packet.WriteInt(guild.MemberCap);
            packet.WriteShort(guild.EmblemBG);
            packet.WriteShort(guild.EmblemBGColour);
            packet.WriteShort(guild.Emblem);
            packet.WriteShort(guild.EmblemColour);
            packet.WriteInt(guild.Members.Length);
            foreach (var member in guild.Members)
            {
                packet.WriteInt(member.CharacterID);
                packet.WriteInt(member.Grade);
            }
            return packet.ToArray();
        }
    }
}
