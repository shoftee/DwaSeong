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
using MapleLib.PacketLib;
using WvsGame.User;

namespace WvsGame
{
    class Program
    {
        public static string GameIMGPath { get; set; }
        public static string DBIMGPath { get; set; }
        public static GameServer mServer { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "갬서버 - 한국메이플스토리 버젼 " + Common.Config.MajorVersion;
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length < 2)
                MessageBox.Show("Invalid argument length", "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
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