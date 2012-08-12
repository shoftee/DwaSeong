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

namespace WvsGame.User
{
    public class Equip : AbstractItem, ICopyable<Equip>
    {
        public byte PossibleUpgrades { get; set; }
        public byte Level { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }
        public short IncHP { get; set; }
        public short IncMP { get; set; }
        public short Watk { get; set; }
        public short Wdef { get; set; }
        public short Matk { get; set; }
        public short Mdef { get; set; }
        public short Accuracy { get; set; }
        public short Avoid { get; set; }
        public short Speed { get; set; }
        public short Jump { get; set; }
        public int Durability { get; set; }
        public byte State { get; set; }
        public byte Enhancements { get; set; }
        public short Pot1 { get; set; }
        public short Pot2 { get; set; }
        public short Pot3 { get; set; }
        public short Pot4 { get; set; }
        public short Pot5 { get; set; }
        public short SocketMask { get; set; }
        public short Socket1 { get; set; }
        public short Socket2 { get; set; }
        public short Socket3 { get; set; }

        public Equip()
        {
            CreationTime = DateTime.Now.ToFileTime();
        }

        public Equip(int itemid = 0, string origin = "")
            : base()
        {
            ItemId = itemid;
            Origin = origin;
            Socket1 = -1;
            Socket2 = -1;
            Socket3 = -1;
        }

        public Equip Copy()
        {
            Equip ret = new Equip();
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

            ret.PossibleUpgrades = PossibleUpgrades;
            ret.Level = Level;
            ret.Str = Str;
            ret.Dex = Dex;
            ret.Int = Int;
            ret.Luk = Luk;
            ret.IncHP = IncHP;
            ret.IncMP = IncMP;
            ret.Watk = Watk;
            ret.Wdef = Wdef;
            ret.Matk = Matk;
            ret.Mdef = Mdef;
            ret.Accuracy = Accuracy;
            ret.Avoid = Avoid;
            ret.Speed = Speed;
            ret.Jump = Jump;
            ret.Durability = Durability;
            ret.State = State;
            ret.Enhancements = Enhancements;
            ret.Pot1 = Pot1;
            ret.Pot2 = Pot2;
            ret.Pot3 = Pot3;
            ret.Pot4 = Pot4;
            ret.Pot5 = Pot5;
            ret.SocketMask = SocketMask;
            ret.Socket1 = Socket1;
            ret.Socket2 = Socket2;
            ret.Socket3 = Socket3;
            return ret;
        }
    }
}
