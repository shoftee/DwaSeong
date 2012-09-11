using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;
using WvsGame.User;

namespace WvsGame.Packets.Handlers
{
    class UserMove : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int tickcount = packet.ReadInt();
            c.mCharacter.mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
            packet.ReadBytes(4);
            Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, packet.ReadBytes(packet.Length - packet.Position)));
            /*
            int portalCount = packet.ReadByte();
            int crc = packet.ReadInt();
            int tickcount = packet.ReadInt();
            short x = packet.ReadShort();
            short y = packet.ReadShort();
            c.mCharacter.mPosition = new System.Drawing.Point(x, y);
            byte[] movement = packet.ReadBytes(packet.Length - packet.Position);
            packet.ReadBytes(4);
                         int tickcount = packet.ReadInt();
             c.mCharacter.mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
             packet.ReadBytes(4);
-            c.mCharacter.ParseMovementPath(packet);*/
            //c.mCharacter.ParseMovementPath(packet);
             //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, MovementParser.ParseMovementPath(packet)));
             //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, packet.ReadBytes(packet.Length - packet.Position)));

            //if (x < Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].WzData

            //c.mCharacter.ParseMovementPath(packet);
            //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, MovementParser.ParseMovementPath(packet)));
            //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, movement));
            //Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, c.mCharacter.ParseMovementPath(packet)));
        }
    }
}