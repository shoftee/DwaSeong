using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;
using WvsGame.Packets;
using WvsGame.Movement;

namespace WvsGame.Field.Entity
{
    public class Npc : FieldObject, IMovable, ISpawnable
    {
        public int mID { get; set; }
        public int mX { get; set; }
        public int mY { get; set; }
        public int mF { get; set; }
        public int mFh { get; set; }
        public int mCy { get; set; }
        public int mRx0 { get; set; }
        public int mRx1 { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(mID + ",");
            sb.Append(mX  + ",");
            sb.Append(mY  + ",");
            sb.Append(mF  + ",");
            sb.Append(mFh + ",");
            sb.Append(mCy + ",");
            sb.Append(mRx0+ ",");
            sb.Append(mRx1+ ",");
            return sb.ToString();
        }

        public PacketWriter GenerateMovementPath()
        {
            return new PacketWriter();
        }

        public List<IMovePath> ParseMovementPath(PacketReader packet)
        {
            return new List<IMovePath>();
        }

        public PacketWriter GenerateSpawnPacket()
        {
            return new PacketWriter(CNpcPool.NpcEnterField(this));
        }

        public PacketWriter GenerateDespawnPacket()
        {
            return new PacketWriter(CNpcPool.NpcLeaveField(this));
        }
    }
}
