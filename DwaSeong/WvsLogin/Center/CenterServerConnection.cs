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
using System.Net;
using Common;
using System.Threading;
using MapleLib.PacketLib;
using MapleLib.MapleCryptoLib;

namespace WvsLogin.Center
{
    class CenterServerConnection
    {
        private Connector mConnector { get; set; }
        public Session mSession { get; set; }
        public CenterServer mCenterServer { get; set; }

        public bool Connect()
        {
            try
            {
                mConnector = new Connector();
                mConnector.OnClientConnected += mConnector_OnClientConnected;
                mConnector.Connect(mCenterServer.ip, mCenterServer.port);
                return true;
            }
            catch (Exception)
            {
                //Console.WriteLine(e.ToString());
                return false;
            }
        }

        void mConnector_OnClientConnected(Session session)
        {
            mSession = session;
            mSession.OnPacketReceived += mSession_OnPacketReceived;
            mSession.OnClientDisconnected += mSession_OnClientDisconnected;
            mSession.OnInitPacketReceived += mSession_OnInitPacketReceived;
            mSession.RIV = new MapleCrypto(new byte[4], Common.Config.MajorVersion);
            mSession.SIV = new MapleCrypto(new byte[4], Common.Config.MajorVersion);
            

            new Thread(() =>
            {
                Thread.Sleep(1000);
                mSession.SendPacket(CenterServerPacketDefinitions.Identify(Common.Config.CenterServerKey));
                    while (session.Socket.Connected)
                    {
                        mSession.SendPacket(CenterServerPacketDefinitions.ConnectedClients(0xFF, Program.mServer.Clients.Count));
                        mSession.SendPacket(CenterServerPacketDefinitions.PollConnectedClients());
                        Thread.Sleep(5000);
                    }
                }).Start();
        }

        void mSession_OnInitPacketReceived(short major, short minor, byte serverIdentifier)
        {
            
        }

        void mSession_OnClientDisconnected(Session session)
        {
            Logger.Write(Logger.LogTypes.경고, "CenterServer {0} disconnected", mCenterServer.Name);
        }

        void mSession_OnPacketReceived(PacketReader packet)
        {
            short opcode = packet.ReadShort();
            switch ((CenterSendOps)opcode)
            {
                case CenterSendOps.IdentifySuccess: CenterServerPacketHandling.HandleIdentify(packet); break;
                case CenterSendOps.ChannelLoad: CenterServerPacketHandling.HandleChannelLoad(packet, mCenterServer); break;
                case CenterSendOps.Migrate: CenterServerPacketHandling.HandleMigrate(mCenterServer, packet); break;

            }
        }
    }
}
