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
using Common;
using WvsGame.Packets;
using MapleLib.PacketLib;

namespace WvsGame.User
{
    public class Client
    {
        public Session mSession { get; set; }
        public Account mAccount { get; set; }
        public Character mCharacter { get; set; }

        public bool validated = false;
        public long LastKeepAlive { get; set; }
        public short mChatColor = -1;

        public Client(Session pSession)
        {
            mSession = pSession;
            mSession.OnPacketReceived += mSession_OnPacketReceived;
            mSession.OnClientDisconnected += mSession_OnClientDisconnected;
            LastKeepAlive = DateTime.Now.ToFileTime();
            new System.Threading.Thread(() =>
                {
                    while (mSession.Socket.Connected)
                    {
                        if (DateTime.Now.ToFileTime() - LastKeepAlive > DateTime.FromFileTime(0).AddSeconds(15).ToFileTime())
                        {
                            mSession.Socket.Close();
                            Logger.Write(Logger.LogTypes.연결, "KeepAlive timeout {0}, {1}, {2}", DateTime.Now.ToFileTime(), LastKeepAlive, DateTime.FromFileTime(0).AddSeconds(15).ToFileTime());
                        }
                        SendPacket(CClientSocket.KeepAlive());
                        System.Threading.Thread.Sleep(1000);
                    }
                }).Start();
        }

        public override string ToString()
        {
            return mSession.Socket + "," + mAccount.AccountId + "," + mAccount.MacAddress.ToString2s() + "," + mAccount.HDDSerial.ToString2s();
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
                Logger.Write(Logger.LogTypes.경고, "!!!: {0}", HexEncoding.ToHex(packet.ReadShort()));
            }
        }

        void mSession_OnClientDisconnected(Session session)
        {
            if (!mAccount.Migrate)
                Database.ExecuteQuery("UPDATE account SET Connected = 0 WHERE AccountName = '{0}';", mAccount.Username);
            Logger.Write(Logger.LogTypes.연결, "클라이언트 {0} 끊겨짐", session.Socket.RemoteEndPoint.ToString());
            Program.mServer.Clients.Remove(this);
        }

        public void SendPacket(byte[] packet)
        {
            Logger.Write(Logger.LogTypes.대타, "보낸 패킷 {0}", packet.ToString2s());
            mSession.SendPacket(new PacketWriter(packet));
        }
    }
}
