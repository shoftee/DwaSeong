using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;

namespace WvsGame.Field.Entity
{
    interface IMovable
    {
        void ParseMovementPath(PacketReader packet);
        PacketWriter GenerateMovementPath();
    }
}
