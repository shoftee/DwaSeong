using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapleLib.PacketLib;
using System.Drawing;

namespace WvsGame.Movement
{
    public interface IMovePath
    {
        void Encode(PacketWriter packet);
    }
}
