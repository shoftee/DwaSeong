using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;

namespace WvsGame.Field.Entity
{
    public interface ISpawnable
    {
        PacketWriter GenerateSpawnPacket();
        PacketWriter GenerateDespawnPacket();
    }
}
