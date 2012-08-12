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
using Common;

namespace WvsGame.Packets.Handlers
{
    internal class PublicChat : IPacketHandler
    {
        public void handlePacket(Client c, PacketReader packet)
        {
            packet.ReadInt(); // timestamp...a client sided timestamp.
            string message = packet.ReadMapleString();
            if (message.StartsWith("@"))
            {
                string[] splitted = message.Replace("@", "").Split(' ');
                int status = CommandProcessing.HandlePlayerCommand(c, splitted);
                if (status == 0)
                    return;
                    //c.SendPacket(CField.ChatMessage(0x0C, "Success! Execute Player Command " + message));
                else if (status == 1)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! NOT EXIST Execute Player Command " + message));
                else if (status == 2)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! EXCEPTION Execute Player Command " + message));
            }
            else if (message.StartsWith("!") && c.mAccount.Admin == 1)
            {
                string[] splitted = message.Replace("!", "").Split(' ');
                int status = CommandProcessing.HandleMasterCommand(c, splitted);
                if (status == 0)
                    c.SendPacket(CField.ChatMessage(0x0C, "Success! Execute Master Command " + message));
                else if (status == 1)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! NOT EXIST Execute Master Command " + message));
                else if (status == 2)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! EXCEPTION Execute Master Command " + message));
            }
            else if (message.StartsWith("/") && c.mAccount.Admin == 1)
            {
                string[] splitted = message.Replace("/", "").Split(' ');
                int status = CommandProcessing.HandleAdminCommand(c, splitted);
                if (status == 0)
                    c.SendPacket(CField.ChatMessage(0x0C, "Success! Execute Admin Command " + message));
                else if (status == 1)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! NOT EXIST Execute Admin Ccommand " + message));
                else if (status == 2)
                    c.SendPacket(CField.ChatMessage(0x0C, "Fail! EXCEPTION Execute Admin Command " + message));
            }
            else
            {
                Program.mServer.Fields[c.mCharacter.mField][0].SendPacket(
                    CUserPool.PublicChatMessage(c.mCharacter.mID,
                                                message,
                                                c.mAccount.Admin == 1,
                                                true));
                Program.mServer.Fields[c.mCharacter.mField][0].SendPacket(
                    CField.ChatMessage(
                        (short) (c.mChatColor >= 0 ? c.mChatColor : c.mAccount.Admin == 1 ? 0x0B : 0x00),
                        "★" +
                        c.mCharacter.
                            mName +
                        "★ : " + message));
            }
        }

        private static class CommandProcessing
        {
            public static int HandlePlayerCommand(Client c, string[] cmd)
            {
                try
                {
                    switch (cmd[0].ToLower())
                    {
                        case "help":
                        case "commands":
                            c.SendPacket(CField.ChatMessage(0x0C, "@help"));
                            c.SendPacket(CField.ChatMessage(0x0C, "@version"));
                            c.SendPacket(CField.ChatMessage(0x0C, "@chatcolor"));
                            break;
                        case "version":
                            c.SendPacket(CField.ChatMessage(0x07,
                                                            "DwaSeong(돠성) Maple Gulobal Emulator Verson " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version));
                            break;
                        case "chatcolor":
                            if (cmd.Length < 2)
                            {
                                c.SendPacket(CField.ChatMessage(0x0C, "0 = Public"));
                                c.SendPacket(CField.ChatMessage(0x0C, "1 = Lime"));
                                c.SendPacket(CField.ChatMessage(0x0C, "2 = Pink"));
                                c.SendPacket(CField.ChatMessage(0x0C, "3 = Orange"));
                                c.SendPacket(CField.ChatMessage(0x0C, "4 = Purple"));
                                c.SendPacket(CField.ChatMessage(0x0C, "5 = Green"));
                                c.SendPacket(CField.ChatMessage(0x0C, "6 = Magenta"));
                                c.SendPacket(CField.ChatMessage(0x0C, "7 = Gray"));
                                c.SendPacket(CField.ChatMessage(0x0C, "8 = Yellow"));
                                c.SendPacket(CField.ChatMessage(0x0C, "9 = Light Yellow"));
                                c.SendPacket(CField.ChatMessage(0x0C, "10 = Cyan"));
                                c.SendPacket(CField.ChatMessage(0x0C, "11 = White Background"));
                                c.SendPacket(CField.ChatMessage(0x0C, "12 = Red"));
                                c.SendPacket(CField.ChatMessage(0x0C, "13 = Ligt Blue Background"));
                            }
                            else
                            {
                                short chatto = short.Parse(cmd[1]);
                                if (chatto >= 0 && chatto <= 0x0D)
                                {
                                    c.SendPacket(CField.ChatMessage(0x0C, "Chat Colour = " + chatto));
                                    c.mChatColor = chatto;
                                }
                                else
                                {
                                    c.SendPacket(CField.ChatMessage(0x0C, "Chose Selection >= 0 While <= 13"));
                                }
                            }
                            break;
                        default:
                            return 1;
                    }
                }
                catch
                {
                    return 2;
                }
                return 0;
            }

            internal static int HandleMasterCommand(Client c, string[] cmd)
            {
                try
                {
                    switch (cmd[0].ToLower())
                    {
                        case "help":
                        case "commands":
                            c.SendPacket(CField.ChatMessage(0x09, "!help"));
                            c.SendPacket(CField.ChatMessage(0x09, "!say [message]"));
                            c.SendPacket(CField.ChatMessage(0x09, "!notice [message]"));
                            c.SendPacket(CField.ChatMessage(0x09, "!clients"));
                            c.SendPacket(CField.ChatMessage(0x09, "!fieldinfo"));
                            c.SendPacket(CField.ChatMessage(0x09, "!gmboard [name|map|all] [url]"));
                            c.SendPacket(CField.ChatMessage(0x09, "!dc [name]"));
                            c.SendPacket(CField.ChatMessage(0x09, "!ban"));
                            c.SendPacket(CField.ChatMessage(0x09, "!waveban"));
                            c.SendPacket(CField.ChatMessage(0x09, "!unban [name]"));
                            break;
                        case "notice":
                            foreach (var cl in Program.mServer.Clients)
                                cl.SendPacket(CWvsContext.BroadcastMessage(0,
                                                                           string.Join(" ", cmd, 1, cmd.Length - 1)));
                            break;
                        case "say":
                            foreach (var cl in Program.mServer.Clients)
                                cl.SendPacket(CField.ChatMessage(0x0A,
                                                                 "[" + c.mCharacter.mName + "] " +
                                                                 string.Join(" ", cmd, 1, cmd.Length - 1)));
                            break;
                        case "clients":
                            var clients = Program.mServer.Clients.Aggregate(string.Empty,
                                                                            (current, cl) =>
                                                                            current + (cl.mCharacter.mName + ","));
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "Server (" + clients.Split(',').Length + "):"));
                            c.SendPacket(CField.ChatMessage(0x09, clients));
                            clients =
                                Program.mServer.Clients.Where(cl => cl.mCharacter.mField == c.mCharacter.mField).
                                    Aggregate(string.Empty, (current, cl) => current + (cl.mCharacter.mName + ","));
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "Field (" + clients.Split(',').Length + "):"));
                            c.SendPacket(CField.ChatMessage(0x09, clients));
                            break;
                        case "mapinfo":
                        case "fieldinfo":
                            c.SendPacket(CField.ChatMessage(0x08, "Field info:"));
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "    Instances: " +
                                                            Program.mServer.Fields[c.mCharacter.mField].Count));
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "    Field Limit: " +
                                                            Program.mServer.Fields[c.mCharacter.mField][0].
                                                                WzData.fieldLimit));
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "    Footholds: " +
                                                            Program.mServer.Fields[c.mCharacter.mField][0].
                                                                mFootholdTree.mFootholds.Count));
                            //var ou = Program.mServer.Fields[c.mCharacter.mField][0].mObjects.Aggregate(string.Empty, (current, o) => current + ("{" + o + "},"));
                            var ou = string.Empty;
                            c.SendPacket(CField.ChatMessage(0x09,
                                                            "    Field Objects: " +
                                                            Program.mServer.Fields[c.mCharacter.mField][0].
                                                                mObjects.Count + " " + ou));
                            break;
                        case "gmboard":
                            foreach (var cl in Program.mServer.Clients)
                                switch (cmd[1])
                                {
                                    case "all":
                                        cl.SendPacket(CWvsContext.GMBoard(cmd[2]));
                                        break;
                                    case "map":
                                        if (cl.mCharacter.mField == c.mCharacter.mField)
                                            cl.SendPacket(CWvsContext.GMBoard(cmd[2]));
                                        else continue;
                                        break;
                                    default:
                                        if (cl.mCharacter.mName == cmd[1])
                                            cl.SendPacket(CWvsContext.GMBoard(cmd[2]));
                                        break;
                                }
                            break;
                        case "disconnect":
                        case "dc":
                            var chrname = cmd[1];
                            foreach (var cl in Program.mServer.Clients.Where(cl => cl.mCharacter.mName == chrname))
                            {
                                cl.mSession.Socket.Disconnect(true);
                                c.SendPacket(CField.ChatMessage(0x0A,
                                                                "User " + chrname +
                                                                " was online, disconnect success"));
                                return 0;
                            }
                            c.SendPacket(CField.ChatMessage(0x0A, "User " + chrname + " was not found"));
                            break;
                        case "block":
                        case "ban":
                            if (cmd.Length != 8)
                            {
                                c.SendPacket(CField.ChatMessage(0x08,
                                                                "Usage: !ban [name] [banreason] [days] [hours] [minutes] [seconds] [boolean:disconnect]"));
                                for (var i = 1; i <= 0x11; ++i)
                                    c.SendPacket(CField.ChatMessage(0x09,
                                                                    "    " + i + " (" +
                                                                    Enum.GetName(typeof (BlockReason), i) +
                                                                    ") : " +
                                                                    BlockReasonOperation.GetReason(
                                                                        (BlockReason) i)));
                            }
                            else
                            {
                                var user = cmd[1];
                                var acc = Database.GetAccountName(user);
                                var reason = int.Parse(cmd[2]);
                                var days = int.Parse(cmd[3]);
                                var hours = int.Parse(cmd[4]);
                                var minutes = int.Parse(cmd[5]);
                                var seconds = int.Parse(cmd[6]);
                                var disconnect = bool.Parse(cmd[7]);
                                Database.IssueBan(acc, (byte) reason,
                                                  DateTime.Now.AddDays(days).AddHours(hours).AddMinutes(minutes).
                                                      AddSeconds(seconds).ToFileTime(),
                                                  "Manual ban issued by " + c.mCharacter.mName + " issued for " + acc +
                                                  "/" + user + " Disconnect(" +
                                                  disconnect + ")");
                                foreach (var cl in Program.mServer.Clients.Where(cl => cl.mAccount.Admin == 1))
                                    cl.SendPacket(CField.ChatMessage(0x0A,
                                                                     "[GM NOTICE] Manual ban(" + reason +
                                                                     ") issued by " +
                                                                     c.mCharacter.mName + " issued for " + acc + "/" +
                                                                     user + " Disconnect(" +
                                                                     disconnect + ")"));
                                if (disconnect)
                                    foreach (var cl in Program.mServer.Clients.Where(cl => cl.mCharacter.mName == user))
                                    {
                                        cl.mSession.Socket.Disconnect(true);
                                        c.SendPacket(CField.ChatMessage(0x0A,
                                                                        "User " + user +
                                                                        " was online, disconnect success"));
                                    }
                            }
                            break;
                        case "waveblock":
                        case "waveban":
                            if (cmd.Length != 8)
                            {
                                c.SendPacket(CField.ChatMessage(0x08,
                                                                "Usage: !waveban [name] [banreason] [days] [hours] [minutes] [seconds] [boolean:disconnect]"));
                                for (var i = 1; i <= 0x11; ++i)
                                    c.SendPacket(CField.ChatMessage(0x09,
                                                                    "    " + i + " (" +
                                                                    Enum.GetName(typeof (BlockReason), i) +
                                                                    ") : " +
                                                                    BlockReasonOperation.GetReason(
                                                                        (BlockReason) i)));
                            }
                            else
                            {
                                var cidissued = c.mCharacter.mID;
                                var timeissued = DateTime.Now.ToFileTime();
                                var cidtarget = Database.GetCID(cmd[1]);
                                var bantype = int.Parse(cmd[2]);
                                var expiredays = int.Parse(cmd[3]);
                                var expirehours = int.Parse(cmd[4]);
                                var expireminutes = int.Parse(cmd[5]);
                                var expireseconds = int.Parse(cmd[6]);
                                var disconnect = bool.Parse(cmd[7]);
                                const int processed = 0;
                                const int timeProcessed = 0;
                                var comment = "Wave ban(" + bantype + ") issued by " + c.mCharacter.mName;
                                var wavebanEntry = new Database.WavebanEntry(cidissued, timeissued,
                                                                             cidtarget, bantype,
                                                                             DateTime.Now.AddDays(
                                                                                 expiredays).AddHours(
                                                                                     expirehours).
                                                                                 AddMinutes(expireminutes)
                                                                                 .AddSeconds(expireseconds)
                                                                                 .ToFileTime(), processed,
                                                                             timeProcessed, comment);
                                Database.AddWaveban(wavebanEntry);
                                foreach (var cl in Program.mServer.Clients.Where(cl => cl.mAccount.Admin == 1))
                                    cl.SendPacket(CField.ChatMessage(0x0A,
                                                                     "[GM NOTICE] Wave ban(" + bantype +
                                                                     ") issued by " +
                                                                     c.mCharacter.mName));
                                if (disconnect)
                                    foreach (
                                        var cl in Program.mServer.Clients.Where(cl => cl.mCharacter.mName == cmd[1]))
                                    {
                                        cl.mSession.Socket.Disconnect(true);
                                        c.SendPacket(CField.ChatMessage(0x0A,
                                                                        "User " + cmd[1] +
                                                                        " was online, disconnect success"));
                                    }
                            }
                            break;
                        case "unblock":
                        case "unban":
                            Database.Unban(Database.GetAccountName(cmd[1]));
                            break;
                        default:
                            return 1;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return 2;
                }
                return 0;
            }

            internal static int HandleAdminCommand(Client c, IList<string> cmd)
            {
                try
                {
                    switch (cmd[0].ToLower())
                    {
                        case "help":
                            c.SendPacket(CField.ChatMessage(0x09, "/processwavebans"));
                            break;
                        case "processwavebans":
                            Database.ProcessWaveBans(Database.GetWavebans());
                            break;
                        default:
                            return 1;
                    }
                }
                catch
                {
                    return 2;
                }
                return 0;
            }
        }
    }
}