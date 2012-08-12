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
using WvsGame.User;
using MapleLib.PacketLib;
using WvsGame.Center;

namespace WvsGame.Packets.Handlers
{
    //28 00 01 00 00 00 1C 65 9D 98 F6 91 C6 62 15 F4 00 00 00 00 CD 68 00 00 F1 16 B7 E8 D4 F6 92 73
    class EnterServer : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            int cid = packet.ReadInt();
            byte[] mac = packet.ReadBytes(6);
            byte[] hwid = packet.ReadBytes(4);
            packet.Skip(8);
            long conauth = packet.ReadLong();
            c.mAccount = new Account();
            c.mAccount.Load(cid);
            Logger.Write(Logger.LogTypes.정보,
                         "CONAUTH;\r\n\t CID={0},\r\n\t CMAC={1},\r\n\t CHWID={2},\r\n\t CCONAUTH={3},\r\n\t SMAC={4},\r\n\t SHWID={5},\r\n\t SCONAUTH={6}",
                         cid, mac.ToString2s(), 
                         hwid.ToString2s(), 
                         conauth, 
                         c.mAccount.MacAddress.ToString2s(),
                         c.mAccount.HDDSerial.ToString2s(), 
                         c.mAccount.conauth);
            if (c.mAccount.conauth != conauth)
            {
                Logger.Write(Logger.LogTypes.경고, "UNEQUAL CONAUTH !");
                return; // TODO: Autoban
            }
            c.mCharacter = Database.GetCharacter(cid);
            c.mCharacter.mClient = c;
            Console.WriteLine("Character: {0}:{1}", c.mCharacter.mID, c.mCharacter.mName);
            c.SendPacket(CStage.SetField(c, true));
            Program.mServer.Fields[c.mCharacter.mField][c.mCharacter.mFieldInstance].AddCharacter(c.mCharacter);
        }
    }
}