/*
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
using System.Windows.Forms;

namespace WvsCenter
{
    class Program
    {
        public static string CenterIMGPath { get; set; }
        public static string DBIMGPath { get; set; }
        public static CenterServer mServer { get; set; }

        static void Main(string[] args)
        {
            Console.Title = "센터서버 - 한국메이플스토리 버젼 " + Common.Config.MajorVersion;
            Console.ForegroundColor = ConsoleColor.White;
            if (args.Length < 2)
            {
                MessageBox.Show("Invalid argument length", "Launch error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                CenterIMGPath = args[0];
                DBIMGPath = args[1];
                try
                {
                    mServer = new CenterServer();
                    mServer.Initialize();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                }
                Console.ReadKey();
            }
            while (true) ;
        }
    }
}