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

namespace WvsCenter
{
    class GameServerConnection
    {
        public GameServer mGameServer { get; set; }
        public Session mSession { get; set; }
        public byte ServerType { get; set; }
        public bool Verified { get; set; }

        public GameServerConnection(Session pSession)
        {
            mSession = pSession;
            mSession.OnClientDisconnected += _Session_OnClientDisconnected;
            mSession.OnPacketReceived += _Session_OnPacketReceived;
        }

        void _Session_OnPacketReceived(PacketReader packet)
        {
            short opcode = packet.ReadShort();
            switch ((CenterRecvOps)opcode)
            {
                case CenterRecvOps.Identify: CenterServerPacketHandling.Identify(this, packet); break;
                case CenterRecvOps.ClientCount: CenterServerPacketHandling.ClientCount(this, packet); break;
                case CenterRecvOps.PollChannelLoad: CenterServerPacketHandling.ChannelLoad(this, packet); break;
                case CenterRecvOps.Migrate: CenterServerPacketHandling.Migrate(this, packet); break;
            }
        }

        void _Session_OnClientDisconnected(Session session)
        {
            mGameServer.Connection = null;
            Program.mServer._Acceptor._ConnectedClients.Remove(this);
        }
    }
}
