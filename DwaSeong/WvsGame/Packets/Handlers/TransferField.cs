﻿/*
	This file is part of the DwaSeong MapleStory Server
    Copyright (C) 2012 "XiaoKia" <xiaokia@naver.com> 

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License version 3
    as published by the Free Software Foundation. You may not use, modify
    or distribute this program under any other version of the
    GNU Affero General Public License.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WvsGame.User;
using MapleLib.PacketLib;
using WvsGame.Center;

namespace WvsGame.Packets.Handlers
{
    class TransferField : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            packet.ReadByte();
            int portalId = packet.ReadInt();
            int timestamp = packet.ReadInt();
        }
    }
}
