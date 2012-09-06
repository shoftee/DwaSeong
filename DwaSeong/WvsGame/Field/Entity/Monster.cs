using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;

namespace WvsGame.Field.Entity
{
    public class Monster : FieldObject, IMovable, ISpawnable
    {
        public int mID { get; set; }
        public int mX { get; set; }
        public int mY { get; set; }
        public int mMobTime { get; set; }
        public int mF { get; set; }
        public int mHide { get; set; }
        public int mFh { get; set; }
        public int mCy { get; set; }
        public int mRx0 { get; set; }
        public int mRx1 { get; set; }

        public PacketWriter GenerateMovementPath()
        {
            return new PacketWriter();
        }

        //public List<IMovePath> ParseMovementPath(PacketReader packet)
        //{
        //    return new List<IMovePath>();
        //}

        public PacketWriter GenerateSpawnPacket()
        {
            return new PacketWriter();
        }

        public PacketWriter GenerateDespawnPacket()
        {
            return new PacketWriter();
        }
    }
}
