using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;
using WvsGame.Movement;

namespace WvsGame.Field.Entity
{
    interface IMovable
    {
        List<IMovePath> ParseMovementPath(PacketReader packet);
        PacketWriter GenerateMovementPath();
    }
}
