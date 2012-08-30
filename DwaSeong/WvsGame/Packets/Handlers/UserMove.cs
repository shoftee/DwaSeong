using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;
using WvsGame.Movement;
using WvsGame.User;

namespace WvsGame.Packets.Handlers
{
    class UserMove : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int portalCount = packet.ReadByte();
            int crc = packet.ReadInt();
            int tickcount = packet.ReadInt();
            c.mCharacter.mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
            packet.ReadBytes(4);
            //c.mCharacter.ParseMovementPath(packet);
            //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, MovementParser.ParseMovementPath(packet)));
            Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, packet.ReadBytes(packet.Length - packet.Position)));
        }
    }
}
