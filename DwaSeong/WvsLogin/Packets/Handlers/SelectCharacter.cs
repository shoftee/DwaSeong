/*
	This file is part of the RiceMS MapleStory Server
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
using WvsLogin.User;
using MapleLib.PacketLib;
using WvsLogin.Center;

namespace WvsLogin.Packets.Handlers
{
    //33 00 07 00 30 30 30 30 30 30 30 01 00 00 00 37 00 30 32 2D 30 30 2D 34 43 2D 34 46 2D 34 46 2D 35 30 2C 20 31 43 2D 36 35 2D 39 44 2D 39 38 2D 46 36 2D 39 31 2C 20 30 30 2D 43 30 2D 43 41 2D 36 31 2D 46 32 2D 41 30 15 00 36 34 33 31 35 30 31 46 32 35 43 32 5F 43 36 36 32 31 35 46 34
    //3  ?  ?  ?  0  0  0  0  0  0  0  ?  ?  ?  ?  7  ?  0  2  -  0  0  -  4  C  -  4  F  -  4  F  -  5  0  ,     1  C  -  6  5  -  9  D  -  9  8  -  F  6  -  9  1  ,     0  0  -  C  0  -  C  A  -  6  1  -  F  2  -  A  0  ?  ?  6  4  3  1  5  0  1  F  2  5  C  2  _  C  6  6  2  1  5  F  4
    //3???0000000????7?02-00-4C-4F-4F-50, 1C-65-9D-98-F6-91, 00-C0-CA-61-F2-A0??6431501F25C2_C66215F4
    /*[7/25/2012 5:59:23 AM][대타] 받은 패킷 33 00 06 00 30 30 30 30 30 30 21 00 00 00
 24 00 30 32 2D 30 30 2D 34 43 2D 34 46 2D 34 46 2D 35 30 2C 20 30 30 2D 43 30 2
D 43 41 2D 36 31 2D 46 32 2D 41 30 15 00 36 34 33 31 35 30 31 46 32 35 43 32 5F
43 36 36 32 31 35 46 34*/
    class SelectCharacter : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            string pic = Database.MySqlEscape(packet.ReadMapleString());
            int cid = packet.ReadInt();
            string macs = packet.ReadMapleString(); // i should probably do something with these lol
            string HWID = packet.ReadMapleString(); // ?_hdd serial
            if (pic != c.Pic)
                c.SendPacket(PacketDefinitions.BadPic());
            else
            {
                c.Migrate = true;
                Program.mServer.GetCenterServerById(c.RecentWorld).mCenterConnection.mSession.SendPacket(CenterServerPacketDefinitions.RequestMigrate(c.AccountId, cid, c.RecentChannel));
            }
        }
    }
}
