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
using System.Windows.Forms;
using WvsLogin.User;
using MapleLib.PacketLib;

namespace WvsLogin
{
    class Program
    {
        public static string GameIMGPath { get; set; }
        public static string DBIMGPath { get; set; }
        public static GameServer mServer { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "로그인서버 - 한국메이플스토리 버젼 " + Common.Config.MajorVersion;
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length < 2)
                MessageBox.Show("Invalid argument length", "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                /* *****************************Skill Point*********************************
                 * *************************Data Storage Concept****************************
                 * Skill Point data is stored in MySQL as a long.
                 * Each two bytes represent a job advancement
                 * 
                 * _______________________________________________________________________
                 * |FF      |00      |FF      |00      |FF      |00      |FF      |00      |
                 * |11111111|00000000|11111111|00000000|11111111|00000000|11111111|00000000|
                 * |-----------------|-----------------|-----------------|-----------------| 
                 * |     1st job     |      2nd job    |      3rd job    |     4th job     |
                 * |_________________|_________________|_________________|_________________| 
                 * 
                 * 
                 * short[] SP = new short[4];
                 * ulong data = 0xFF00EE00DD00CC00;
                 * 
                 * //This code will retrieve the bytes
                 * SP[0] = (short)(data >> 48);
                 * SP[1] = (short)(data << 16 >> 48);
                 * SP[2] = (short)(data << 32 >> 48);
                 * SP[3] = (short)(data << 48 >> 48);
                 * Console.WriteLine("{0},{1},{2},{3}", SP[0], SP[1], SP[2], SP[3]);
                 * 
                 * //This code will set the bytes
                 * data = (data & 0x0000FFFFFFFFFFFF) | ((ulong)1 << 48); // 1st job
                 * data = (data & 0xFFFF0000FFFFFFFF) | ((ulong)3 << 32); // 2nd job
                 * data = (data & 0xFFFFFFFF0000FFFF) | ((ulong)3 << 16); // 3rd job
                 * data = (data & 0xFFFFFFFFFFFF0000) | ((ulong)7);       // 4th job
                 */

                GameIMGPath = args[0];
                DBIMGPath = args[1];
                try
                {
                    mServer = new GameServer();
                    mServer.Initialize();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                while (true)
                {
                    string[] str = Console.ReadLine().Split(" "[0]);
                    switch (str[0])
                    {
                        case "packetout":
                            foreach (Client c in mServer.Clients)
                                c.SendPacket(HexEncoding.GetBytes(string.Join(" ", str, 1, str.Length - 1)));
                            break;
                    }
                }
            }
        }
    }
}