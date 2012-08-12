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
using MapleLib.PacketLib;
using MapleLib.MapleCryptoLib;
using WvsGame.Networking;
using WvsGame.User;

namespace WvsGame.Networking
{
    class ClientAcceptor
    {
        public GameServer mGameServer { get; set; }
        public Acceptor acceptor { get; set; }

        public void Listen()
        {
            acceptor = new Acceptor();
            acceptor.OnClientConnected += acceptor_OnClientConnected;
            acceptor.StartListening(mGameServer.port);
            Logger.Write(Logger.LogTypes.정보, "포트{0} 기다리는중…", mGameServer.port);
        }

        void acceptor_OnClientConnected(Session session)
        {
            Logger.Write(Logger.LogTypes.연결, "클라이언트 {0} 연결됨", session.Socket.RemoteEndPoint.ToString());

            Random rand = new Random();
            int riv = rand.Next();
            int siv = rand.Next();

            session.RIV = new MapleCrypto(BitConverter.GetBytes(riv), Config.MajorVersion);
            session.SIV = new MapleCrypto(BitConverter.GetBytes(siv), Config.MajorVersion);

            PacketWriter packet = new PacketWriter();
            packet.WriteShort(0);
            packet.WriteShort(Config.MajorVersion);
            packet.WriteShort(1);
            packet.WriteByte(0x31);
            packet.WriteInt(riv);
            packet.WriteInt(siv);
            packet.WriteByte(0x08);
            packet.SetShort(0, packet.Length - 2);
            session.SendRawPacket(packet.ToArray());

            Client c = new Client(session);
            mGameServer.Clients.Add(c);
        }
    }
}
