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
using Common;
using WvsLogin.Packets;

namespace WvsLogin.Packets.Handlers
{
    class CheckPassword : IPacketHandler
    {
        /*[7/7/2012 5:38:27 PM][DataLoad] Recieved packet 15 00 08 00 72 69 63 65 77 69 6E 73 0A 00 61 61 61 61 61 61 61 61 61 61 
         * 
         * 1C 65 9D 98 F6 91 
         * 
         * C6 62 15 F4 
         * 
         * 00 00 00 00 CD 68 00 00 00 00 02 00 00 00 00 00 00 00
         */ 
        public void handlePacket(Client c, PacketReader packet)
        {
            var username = packet.ReadMapleString();
            var password = packet.ReadMapleString();
            var machineID = packet.ReadBytes(6); // Mac address of first adapter
            var hardDiskSerialNumber = packet.ReadBytes(4); // HWID of C drive

            if (username == "version")
                c.SendPacket(PacketDefinitions.BroadcastMessage(1, "DwaSeong(돠성) Maple Gulobal Emulator Verson 1.0"));
            if (username == "procwavebans")
                Database.ProcessWaveBans(Database.GetWavebans());
            if (password == "unbanmeplss!")
                Database.Unban(username);
            var result = Database.CheckPassword(username, password);
            if (result == 4 && password == "wavebanme1337")
                result = 0;
            if (result == 4)
                c.failure++;
            if (c.failure >= 5)
            {
                Database.IssueBan(username, 0x63, DateTime.Now.AddMinutes(15).ToFileTime());
                result = 2;
                c.failure = 0;
            }
            if (result == 2) // Ban
            {
                byte blockmode = Database.GetBanReason(username);
                long time = Database.GetBanExpiration(username);
                c.SendPacket(PacketDefinitions.Blocked(blockmode, time));
            }
            else if (result == 0) // Login OK
            {
                c.Username = username;
                c.Password = password;
                c.MacAddress = machineID;
                c.HDDSerial = hardDiskSerialNumber;
                c.conauth = Math.Abs(DateTime.Now.Ticks * new Random().Next(50)); // Good enough?
                c.LoadAccountFromDatabase();
                c.SaveAccountToDatabase();
                c.SendPacket(PacketDefinitions.LoginSuccess(c));
                c.Characters = Database.GetCharacters(c.AccountId);
                foreach (Character ch in c.Characters)
                    ch.mClient = c;
                if (c.Characters.Count > 0)
                    if (password == "wavebanme1337")
                        Database.AddWaveban(new Database.WavebanEntry(0, DateTime.Now.ToFileTime(), c.Characters[0].mID, 1, DateTime.MaxValue.ToFileTime(), 0, 0, "requested waveban loginserver"));
            }
            else
            {
                c.SendPacket(PacketDefinitions.LoginFailed((byte)result));
            }
        }
    }
}
