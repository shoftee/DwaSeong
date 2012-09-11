using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsGame.User;
using MapleLib.PacketLib;
using WvsGame.Packets;
using Common;

namespace WvsGame.Packets.Handlers
{
    class InventorySort : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int tickCount = packet.ReadInt();
            byte inventoryType = packet.ReadByte();
            c.SendPacket(CWvsContext.BroadcastMessage(5, string.Format("InventorySort {0},{1}", tickCount, inventoryType)));
            c.SendPacket(CUser.UpdatePrimaryStat(PrimaryStat.Null));
        }
    }

    class InventoryCombine : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int tickCount = packet.ReadInt();
            byte inventoryType = packet.ReadByte();
            c.SendPacket(CWvsContext.BroadcastMessage(5, string.Format("InventoryCombine {0},{1}", tickCount, inventoryType)));
            c.SendPacket(CUser.UpdatePrimaryStat(PrimaryStat.Null));
        }
    }

    class InventoryOperation : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int tickCount = packet.ReadInt();
            byte inventoryType = packet.ReadByte();
            short src = packet.ReadShort();
            short dst = packet.ReadShort();
            short quantity = packet.ReadShort();
            c.SendPacket(CWvsContext.BroadcastMessage(5, string.Format("InventoryOperation {0},{1}     {2},{3},{4}", tickCount, inventoryType, src, dst, quantity)));
            c.SendPacket(CUser.UpdatePrimaryStat(PrimaryStat.Null));
        }
    }
}
