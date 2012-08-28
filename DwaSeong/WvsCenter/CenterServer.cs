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

namespace WvsCenter
{
    class CenterServer
    {
        private delegate bool _Delegate();

        public CenterServerAcceptor _Acceptor { get; set; }

        #region Center IMG Data
        public ushort port { get; set; } // Standard Client Socket port
        public ushort adminPort { get; set; } // Admin Client Socket port
        public uint gameWorldId { get; set; } // World Id
        public string gameWorldName { get; set; } // World name

        public string dbCatalogGlobalAccount { get; set; } // Database name of Global Account database
        public string dbCatalogGameWorld { get; set; } // Database name of Game World database
        public string dbCatalogCoupon { get; set; } // Database name of Coupon database
        public KeyValuePair<string, ushort> dbGameWorldSource { get; set; } // IP/Port of Game World data server
        public KeyValuePair<string, ushort> dbGlobalAccountSource { get; set; } // IP/Port of Global Acccount data server
        public KeyValuePair<string, ushort> dbCouponSource { get; set; } // IP/Port of Coupon data server

        public uint userLimitITC { get; set; } // Connection limit to MTS

        public Dictionary<string, GameServer> gameServers { get; set; }
        #endregion

        #region Database IMG Data
        public string dbUsername { get; set; }
        public string dbPassword { get; set; }
        public string dbDatabase { get; set; }
        public string dbHost { get; set; }
        #endregion

        public void Initialize()
        {
            gameServers = new Dictionary<string, GameServer>();
            _Acceptor = new CenterServerAcceptor();
            DateTime time = DateTime.Now;
            Logger.LogInit("Loading config from Center IMG", new _Delegate(LoadCenterIMGConfig));
            Logger.LogInit("Loading config from Database IMG", new _Delegate(LoadDBIMGConfig));
            Logger.LogInit("Establishing database configuration", new _Delegate(SetDBCon));
            Logger.LogInit("Initialize CenterServer acceptor", new _Delegate(_Acceptor.Listen));
            Logger.Write(Logger.LogTypes.정보, "Server initialized in {0} milliseconds", DateTime.Now.Subtract(time).TotalMilliseconds);

            while (true)
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("         ______________________________________________________________");
                  Console.Write("\r                                                                       |");
                  Console.Write("\r                                                            | {0}", "일렬");
                  Console.Write("\r                                                  | {0}", "포트");
                  Console.Write("\r                                  | {0}", "IP주소");
                  Console.Write("\r                   | {0}", "연결");
                  Console.Write("\r         {0}", "타입");
                  Console.Write("\r        |");
                Console.WriteLine();
                Console.WriteLine("        |__________|______________|_______________|_________|__________|");
                foreach (GameServer serv in gameServers.Values)
                {
                    Console.Write("\r                                                                       |");
                    if (serv.Connection == null)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\r                                                            | {0}", serv.Name);
                    Console.Write("\r                                                  | {0}", serv.port);
                    Console.Write("\r                                  | {0}", serv.PublicIP);
                    Console.Write("\r                   | {0}", serv.Connection != null ? serv.ClientCount.ToString() : "뵨굘끊김");
                    Console.Write("\r         {0}", serv.ServerType);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\r        |");
                    Console.WriteLine();
                }
                Console.WriteLine("        |__________|______________|_______________|_________|__________|");
                System.Threading.Thread.Sleep(10000);
            }
        }

        public bool LoadCenterIMGConfig()
        {
            try
            {
                ConfigReader reader = new ConfigReader(Program.CenterIMGPath);
                port = reader.getUShort("", "port");
                adminPort = reader.getUShort("", "adminPort");
                gameWorldId = reader.getUInt("", "gameWorldId");
                gameWorldName = reader.getString("", "gameWorldName");

                dbCatalogGlobalAccount = reader.getString("", "dbCatalogGlobalAccount");
                dbCatalogGameWorld = reader.getString("", "dbCatalogGameWorld");
                dbCatalogCoupon = reader.getString("", "dbCatalogCoupon");

                userLimitITC = reader.getUInt("", "userLimitITC");

                IEnumerable<string> gameServerTypeList = reader.getBlocksFromBlock("", 1);
                GameServerType gameServerType;
                foreach (string s in gameServerTypeList)
                {
                    switch (s)
                    {
                        case "login": gameServerType = GameServerType.Login; break;
                        case "game": gameServerType = GameServerType.Game; break;
                        case "shop": gameServerType = GameServerType.Shop; break;
                        case "mapgen": gameServerType = GameServerType.MapGen; break;
                        case "claim": gameServerType = GameServerType.Claim; break;
                        case "itc": gameServerType = GameServerType.ITC; break;
                        default: gameServerType = GameServerType.UNK; break;
                    }
                    if (gameServerType == GameServerType.UNK)
                    {
                        Logger.Warn("Unknown GameServerType: " + s);
                        continue;
                    }
                    if (gameServerType == GameServerType.Claim)
                    {
                        GameServer server = new GameServer();
                        server.Name = s;
                        server.PublicIP = reader.getString(s, "PublicIP");
                        server.PrivateIP = reader.getString(s, "PrivateIP");
                        server.port = reader.getUShort(s, "port");
                        server.ServerType = gameServerType;
                        server.ID = GetNextAvailableGameServerId();
                        gameServers.Add(s, server);
                    }
                    else
                    {
                        foreach (string node in reader.GetBlocks(s, true))
                        {
                            GameServer server = new GameServer();
                            server.Name = node;
                            server.PublicIP = reader.getString(node, "PublicIP");
                            server.PrivateIP = reader.getString(node, "PrivateIP");
                            server.port = reader.getUShort(node, "port");
                            server.ServerType = gameServerType;
                            server.ID = server.ServerType == GameServerType.Login ? 0xFF : GetNextAvailableGameServerId();
                            gameServers.Add(node, server);
                        }
                    }
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

        public GameServer GetGameServerById(int id)
        {
            foreach (GameServer serv in gameServers.Values)
                if (serv.ID == id)
                    return serv;
            return null;
        }

        public int GetNextAvailableGameServerId()
        {
            for (int i = 0; i < int.MaxValue; ++i)
                if (GetGameServerById(i) == null)
                    return i;
            return 0;
        }
    }
}