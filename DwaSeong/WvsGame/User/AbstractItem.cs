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

namespace WvsGame.User
{
    public abstract class AbstractItem
    {
        public int CharacterId { get; set; }
        public int ItemId { get; set; }
        public sbyte Position { get; set; }
        public long Expiration { get; set; }
        public string Owner { get; set; }
        public short Flag { get; set; }
        public string Origin { get; set; }
        public long CreationTime { get; set; }

        /*******************************
         * Item flag mask
         * 0x10 = not for sale
         * 0x20 = expire on logout
         * 0x40 = pick up block
         * 0x80 = only
         * 0x100 = account sharable
         * 0x200 = quest
         * 0x400 = trade block
         * 0x800 = account share tag
         * 0x1000 = mob HP
         ******************************/

        //public abstract AbstractItem Copy();

        #region wzdata
        public int job { get; set; }
        public int level { get; set; }
        public int reqStr { get; set; }
        public int reqDex { get; set; }
        public int reqInt { get; set; }
        public int reqLuk { get; set; }
        public int incStr { get; set; }
        public int incDex { get; set; }
        public int incInt { get; set; }
        public int incLuk { get; set; }
        public int incWatk { get; set; }
        public int incMatk { get; set; }
        public int incWdef { get; set; }
        public int incMdef { get; set; }
        public int incMaxHP { get; set; }
        public int incMaxMP { get; set; }
        public int incAcc { get; set; }
        public int incAvoid { get; set; }
        public int incSpeed { get; set; }
        public int incJump { get; set; }
        public int upgrades { get; set; }
        public int price { get; set; }
        public int attackSpeed { get; set; }
        public int cash { get; set; }
        public int knockback { get; set; }
        public int equipTradeBlock { get; set; }
        public int setItemID { get; set; }
        public int only { get; set; }
        public int slotMax { get; set; }
        public int notSale { get; set; }
        public int specialID { get; set; }
        #endregion
    }
}
