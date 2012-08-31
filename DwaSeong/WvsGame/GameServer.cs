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
using WvsGame.WZ;
using WvsGame.Center;
using WvsGame.User;
using WvsGame.Packets;
using WvsGame.Networking;
using Common;

namespace WvsGame
{
    using CField = WvsGame.Field.Field;
    class GameServer
    {
        private delegate bool _Delegate();

        public int ServerId { get; set; }
        public List<Client> Clients { get; set; }
        public ClientAcceptor Acceptor { get; set; }
        public Dictionary<int, List<CField>> Fields { get; set; }

        #region Game IMG Data
        public ushort port { get; set; }
        public int gameWorldId { get; set; }

        public string PublicIP { get; set; }
        public string PrivateIP { get; set; }

        public Dictionary<int, int> LogCharacter { get; set; } // CharacterIDs to log

        public CenterServer center { get; set; }
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
            Fields = new Dictionary<int, List<CField>>();
            var time = DateTime.Now;
            Console.WriteLine();
            Console.WriteLine("\tDwaSeong(돠성) Maple Gulobal Emulator Verson " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine();
            Logger.LogInit("Loading config from Login IMG", new _Delegate(LoadGameIMGConfig));
            Logger.LogInit("Loading config from Database IMG", new _Delegate(LoadDBIMGConfig));
            Logger.LogInit("Establishing database connection", new _Delegate(SetDBCon));
            Logger.LogInit("Initializing packet handlers", new _Delegate(RegisterHandlers));
            Logger.LogInit("Caching WZ Equip Data", new _Delegate(CacheWZCharacterData));
            Logger.LogInit("Caching WZ Item Data", new _Delegate(CacheWZItemData));
            Logger.LogInit("Caching WZ Field Data", new _Delegate(CacheWZFieldData));
            Logger.LogInit("Caching WZ Skill Data", new _Delegate(CacheWZSkillData));
            Logger.LogInit("Initializing field data", new _Delegate(InitializeFieldData));
            Logger.LogInit("Cleaning up", new _Delegate(CleanUp));
            center.mCenterConnection = new CenterServerConnection {mCenterServer = center};
            Logger.LogInit("Establishing CenterServer connection for world " + center.Name, new _Delegate(center.mCenterConnection.Connect));
            Logger.Write(Logger.LogTypes.정보, "Server initialized in {0} milliseconds", DateTime.Now.Subtract(time).TotalMilliseconds);
        }

        private bool LoadGameIMGConfig()
        {
            try
            {
                LogCharacter = new Dictionary<int, int>();
                center = new CenterServer();
                var reader = new ConfigReader(Program.GameIMGPath);

                port = reader.getUShort("", "port");
                gameWorldId = reader.getInt("", "gameWorldId");

                foreach (string s in reader.GetBlocks("logaccount", true))
                {
                    LogCharacter.Add(int.Parse(s), reader.getInt("logcharacter", s));
                }

                center.ip = reader.getString("center", "ip");
                center.port = reader.getUShort("center", "port");
                center.Name = reader.getString("center", "worldName");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool LoadDBIMGConfig()
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

        public Client GetClient(int cid)
        {
            foreach (var client in Clients)
                if (client.mCharacter != null && client.mCharacter.mID == cid)
                    return client;
            return null;
        }

        public bool CacheWZCharacterData()
        {
            try
            {
                NXDataProvider.CacheCharacterData();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        public bool CacheWZItemData()
        {
            try
            {
                NXDataProvider.CacheItemData();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        public bool CacheWZFieldData()
        {
            try
            {
                NXDataProvider.CacheFieldData();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        public bool CacheWZSkillData()
        {
            try
            {
                NXDataProvider.CacheSkillData();
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        private bool InitializeFieldData()
        {
            try
            {
                foreach (KeyValuePair<int, CField> f in NXDataCache.Field)
                {
                    Fields.Add(f.Key, new List<CField>());
                    Fields[f.Key].Add(f.Value.Copy());
                    //Fields[f.Key].Add(ObjectClone.Clone<Field>(f.Value));  // SERIALIZATION = SLOW GARBAGE
                }
                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }

        private bool CleanUp()
        {
            try
            {
                //release players?
                Database.ExecuteQuery("UPDATE account SET Connected = 0 WHERE RecentChannel = {0};", ServerId);

                return true;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                return false;
            }
        }
    }
}
