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
            int portalCount = packet.ReadByte();
            int crc = packet.ReadInt();
            int tickcount = packet.ReadInt();
            c.mCharacter.mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
            Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].SendPacket(c, CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, packet.ReadBytes(packet.Length - packet.Position)));
            //Console.WriteLine("UH....PACKET?\r\n{0}", CField.UserMove(c.mCharacter.mID, c.mCharacter.mPosition, packet.ReadBytes(packet.Length - packet.Position)).ToString2s());
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
            /*[9/12/2012 12:40:34 AM][대타] 받은 패킷 
             * 49 00 
             * 01 AC 7E BC BC 80 15 FC 05 BA 02 12 01 00 00 00 00 03 00 BA 02 12 01 00 00 00 00 61 00 00 00 00 00 
             * 04 4A 01 00 C8 02 12 01 96 00 00 00 61 00 00 00 00 00 02 96 00 00 CC 02 12 01 7E 00 00 00 61 00 00 
             * 00 00 00 04 1E 00 11 00 00 00 00 00 40 44 44 00 BA 02 12 01 CC 02 12 01
             * 
             */
        }
    }
}