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

namespace WvsCenter
{
    enum GameServerType
    {
        Login,
        Game,
        Shop,
        MapGen,
        Claim,
        ITC,
        UNK
    }

    class GameServer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string PublicIP { get; set; }
        public string PrivateIP { get; set; }
        public ushort port { get; set; }
        public GameServerType ServerType { get; set; }
        public GameServerConnection Connection { get; set; }
        public int ClientCount { get; set; }

        public override string ToString()
        {
            return "ClientCount:" + ClientCount + ",Name:" + Name + ",PublicIP:" + PublicIP + ",Port:" + port + ",ServerType:" + ServerType;
        }
    }
}
