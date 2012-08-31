using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MapleLib.PacketLib;

namespace WvsGame.Movement
{
    class AbsoluteLifeMovePath : IMovePath
    {
        public int Type, Duration, Newstate, Unk, Fh;
        public Point Position, PixelsPerSecond, Offset;

        public AbsoluteLifeMovePath(int type, Point position, int duration, int newstate, int unk = 0, int fh = 0, Point pps = new Point(), Point offset = new Point())
        {
            Type = type;
            Position = position;
            Duration = duration;
            Newstate = newstate;
            Unk = unk;
            Fh = fh;
            PixelsPerSecond = pps;
            Offset = offset;
        }

        public void Encode(PacketWriter packet)
        {
            packet.WriteByte(Type);
            packet.WritePos(Position);
            packet.WritePos(PixelsPerSecond);
            packet.WriteShort(Unk);
            if (Type == 14)
                packet.WriteShort(Fh);
            if (Type != 44)
                packet.WritePos(Offset);
            packet.WriteByte(Newstate);
            packet.WriteShort(Duration);
        }
    }
}
