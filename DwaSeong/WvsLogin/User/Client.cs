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
using Common;
using WvsLogin.Packets;
using MapleLib.PacketLib;

namespace WvsLogin.User
{
    public class Client
    {
        public Session mSession { get; set; }

        public bool validated = false;
        public long lastKeepAlive = 0;
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public long SessionID = -1;
        public int RecentWorld { get; set; }
        public int RecentChannel { get; set; }
        public byte[] MacAddress { get; set; }
        public byte[] HDDSerial { get; set; }
        public string Pin { get; set; }
        public string Pic { get; set; }
        public int Admin { get; set; }
        public int TradeBlock { get; set; }
        public long TradeBlockExpiration { get; set; }

        public long LastKeepAlive { get; set; }
        public bool Migrate = false;
        public int LoginFailCount { get; set; }

        public List<Character> Characters { get; set; }

        public Client(Session pSession)
        {
            Characters = new List<Character>();
            mSession = pSession;
            mSession.OnPacketReceived += mSession_OnPacketReceived;
            mSession.OnClientDisconnected += mSession_OnClientDisconnected;
        }

        public override string ToString()
        {
            return AccountId + "," + MacAddress.ToString2s() + "," + HDDSerial.ToString2s();
        }

        void mSession_OnPacketReceived(PacketReader packet)
        {
            Logger.Write(Logger.LogTypes.대타, "받은 패킷 {0}", packet.ToArray().ToString2s());
            IPacketHandler handler = PacketHandler.getInstance().GetHandler(packet.ReadShort());
            if (handler != null)
                handler.handlePacket(this, packet);
            else
            {
                packet.Reset(0);
                Logger.Write(Logger.LogTypes.경고, "!!: {0}", HexEncoding.ToHex(packet.ReadShort()));
            }
        }

        void mSession_OnClientDisconnected(Session session)
        {
            if (!Migrate)
                Database.ExecuteQuery("UPDATE account SET Connected = 0 WHERE AccountName = '{0}';", Username);
            Logger.Write(Logger.LogTypes.연결, "클라이언트 {0} 끊겨짐", session.Socket.RemoteEndPoint.ToString());
            Program.mServer.Clients.Remove(this);
        }

        public void SendPacket(byte[] packet)
        {
            mSession.SendPacket(packet);
        }

        public void LoadAccountFromDatabase()
        {
            AccountId = Database.GetAccountId(Username);
            Pin = Database.GetPin(Username);
            Pic = Database.GetPic(Username);
            RecentWorld = Database.GetRecentWorld(Username);
            Admin = Database.GetAdmin(Username);
            TradeBlock = Database.GetTradeBlock(Username);
            TradeBlockExpiration = Database.GetTradeBlockExpiration(Username);
        }

        public void SaveAccountToDatabase()
        {
            Database.ExecuteQuery("UPDATE Account SET `SessionID` = {0}, `RecentWorld` = {1}, `MacAddress` = '{2}', `HWID` = '{3}', `Pin` = '{4}', `Pic` = '{5}' WHERE AccountName = '{6}'",
                SessionID,
                RecentWorld,
                HexEncoding.byteArrayToString(MacAddress).Replace(" ", ""),
                HexEncoding.byteArrayToString(HDDSerial).Replace(" ", ""),
                Pin,
                Pic,
                Username);
        }
    }
}
