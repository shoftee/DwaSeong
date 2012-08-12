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
using MapleLib.PacketLib;
using MapleLib.MapleCryptoLib;

namespace WvsCenter
{
    class CenterServerAcceptor
    {
        private Acceptor _Acceptor { get; set; }
        public List<GameServerConnection> _ConnectedClients { get; set; }

        public bool Listen()
        {
            try
            {
                _Acceptor = new Acceptor();
                _ConnectedClients = new List<GameServerConnection>();

                _Acceptor.StartListening((int)Program.mServer.port);
                _Acceptor.OnClientConnected += _Acceptor_OnClientConnected;
                return true;
            }
            catch
            {
                return false;
            }
        }

        void _Acceptor_OnClientConnected(Session session)
        {
            Logger.Write(Logger.LogTypes.연결, "opened connection with {0}", session.Socket.RemoteEndPoint.ToString());

            session.RIV = new MapleCrypto(new byte[4], Common.Config.MajorVersion);
            session.SIV = new MapleCrypto(new byte[4], Common.Config.MajorVersion);

            PacketWriter packet = new PacketWriter();
            packet.WriteShort(0);
            packet.WriteShort(Common.Config.MajorVersion);
            packet.WriteShort(1);
            packet.WriteByte(0x31);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(0x08);
            packet.SetShort(0, packet.Length - 2);
            session.SendRawPacket(packet.ToArray());


            GameServerConnection con = new GameServerConnection(session);
            _ConnectedClients.Add(con);
            
        }
    }
}
