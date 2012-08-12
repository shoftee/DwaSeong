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
using WvsLogin.User;
using WvsLogin.Center;
using WvsLogin.Networking;
using WvsLogin.Packets;

namespace WvsLogin
{
    class GameServer
    {
        private delegate bool _Delegate();

        public List<Client> Clients { get; set; }
        public ClientAcceptor Acceptor { get; set; }

        #region Login IMG Data
        public ushort port { get; set; } // Standard Client Socket port
        public ushort adminPort { get; set; } // Admin Client Socket port
        public string dbCatalogGlobalAccount { get; set; } // Database name of Global Account database
        public string dbCatalogGameWorld { get; set; } // Database name of Game World database

        public string PrivateIP { get; set; } // IP for internal connections
        public string PublicIP { get; set; } // IP for outside connections
        
        public Dictionary<int, int> LogAccounts { get; set; } // AccountIDs to log

        public Dictionary<string, CenterServer> centerServers { get; set; }
        #endregion

        #region Database IMG Data
        public string dbUsername { get; set; }
        public string dbPassword { get; set; }
        public string dbDatabase { get; set; }
        public string dbHost { get; set; }
        #endregion

        public void Initialize()
        {
            Clients = new List<Client>();
            DateTime time = DateTime.Now;
            Logger.LogInit("Loading config from Login IMG", new _Delegate(LoadGameIMGConfig));
            Logger.LogInit("Loading config from Database IMG", new _Delegate(LoadDBIMGConfig));
            Logger.LogInit("Establishing database connection", new _Delegate(SetDBCon));
            Logger.LogInit("Initializing packet handlers", new _Delegate(RegisterHandlers));
            foreach (KeyValuePair<string, CenterServer> cserv in centerServers)
            {
                cserv.Value.mCenterConnection = new CenterServerConnection();
                cserv.Value.mCenterConnection.mCenterServer = cserv.Value;
                Logger.LogInit("Establishing CenterServer connection for world " + cserv.Key, new _Delegate(cserv.Value.mCenterConnection.Connect));
            }
            Logger.Write(Logger.LogTypes.정보, "Server initialized in {0} milliseconds", DateTime.Now.Subtract(time).TotalMilliseconds);
        }

        public bool LoadGameIMGConfig()
        {
            try
            {
                LogAccounts = new Dictionary<int, int>();
                centerServers = new Dictionary<string, CenterServer>();
                ConfigReader reader = new ConfigReader(Program.GameIMGPath);

                port = reader.getUShort("", "port");
                adminPort = reader.getUShort("", "adminPort");

                dbCatalogGlobalAccount = reader.getString("", "dbCatalogGlobalAccount");
                dbCatalogGameWorld = reader.getString("", "dbCatalogGameWorld");

                foreach (string s in reader.GetBlocks("logaccount", true))
                {
                    LogAccounts.Add(int.Parse(s), reader.getInt("logaccount", s));
                }

                foreach (string s in reader.GetBlocks("center", true))
                {
                    CenterServer centerServ = new CenterServer();
                    centerServ.ip = reader.getString(s, "ip");
                    centerServ.port = reader.getUShort(s, "port");
                    centerServ.world = reader.getInt(s, "world");
                    centerServ.channelNo = reader.getInt(s, "channelNo");
                    centerServers.Add(s, centerServ);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool LoadDBIMGConfig()
        {
            try
            {
                ConfigReader reader = new ConfigReader(Program.DBIMGPath);
                dbUsername = reader.getString("", "dbUsername");
                dbPassword = reader.getString("", "dbPassword");
                dbDatabase = reader.getString("", "dbDatabase");
                dbHost = reader.getString("", "dbHost");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetDBCon()
        {
            return Database.EstablishConnection(dbUsername, dbPassword, dbDatabase, dbHost);
        }

        public bool RegisterHandlers()
        {
            try
            {
                PacketHandler.getInstance();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        public CenterServer GetCenterServerById(int id)
        {
            foreach (CenterServer serv in centerServers.Values)
                if (serv.world == id)
                    return serv;
            return null;
        }

        public Client GetClientByAccountId(int accid)
        {
            foreach (Client c in Clients)
                if (c.AccountId == accid) 
                    return c;
            return null;
        }
    }
}
