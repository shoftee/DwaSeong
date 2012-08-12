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

namespace WvsLogin.WZ
{
    public class Item : AbstractItem
    {
        public short Quantity { get; set; }

        public Item()
        {
            CreationTime = DateTime.Now.ToFileTime();
        }

        public Item(int itemid = 0, string origin = "")
            : base()
        {
            ItemId = itemid;
            Origin = origin;
        }

        public Item Copy()
        {
            Item ret = new Item();
            ret.CharacterId = CharacterId;
            ret.ItemId = ItemId;
            ret.Position = Position;
            ret.Expiration = Expiration;
            ret.Owner = Owner;
            ret.Flag = Flag;
            ret.Origin = Origin;
            ret.CreationTime = CreationTime;

            ret.job = job;
            ret.level = level;
            ret.reqStr = reqStr;
            ret.reqDex = reqDex;
            ret.reqInt = reqInt;
            ret.reqLuk = reqLuk;
            ret.incStr = incStr;
            ret.incDex = incDex;
            ret.incInt = incInt;
            ret.incLuk = incLuk;
            ret.incWatk = incWatk;
            ret.incMatk = incMatk;
            ret.incWdef = incWdef;
            ret.incMdef = incMdef;
            ret.incMaxHP = incMaxHP;
            ret.incMaxMP = incMaxMP;
            ret.incAcc = incAcc;
            ret.incAvoid = incAvoid;
            ret.incSpeed = incSpeed;
            ret.incJump = incJump;
            ret.upgrades = upgrades;
            ret.price = price;
            ret.attackSpeed = attackSpeed;
            ret.cash = cash;
            ret.knockback = knockback;
            ret.equipTradeBlock = equipTradeBlock;
            ret.setItemID = setItemID;
            ret.only = only;
            ret.slotMax = slotMax;
            ret.notSale = notSale;
            ret.specialID = specialID;

            ret.Quantity = Quantity;
            return ret;
        }
    }
}
