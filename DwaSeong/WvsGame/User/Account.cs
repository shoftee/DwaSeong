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

namespace WvsGame.User
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] MacAddress { get; set; }
        public byte[] HDDSerial { get; set; }
        public string Pin { get; set; }
        public string Pic { get; set; }
        public int Admin { get; set; }
        public int TradeBlock { get; set; }
        public long TradeBlockExpiration { get; set; }
        public int EmailPending { get; set; }
        public int Banned { get; set; }
        public long BanExpiration { get; set; }
        public string BanDescription { get; set; }
        public string CurrentIP { get; set; }
        public string CreateDate { get; set; }
        public long SessionID = -1;
        public bool Migrate = false;

        public void Load(int cid)
        {
            AccountId = Database.GetAccountId(cid);
            Database.SetAccountInfo(this);
        }
    }
}
