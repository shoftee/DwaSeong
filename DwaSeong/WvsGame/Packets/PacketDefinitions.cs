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
using MapleLib.PacketLib;
using WvsGame.Field.Entity;
using WvsGame.WZ;
using WvsGame.User;
using System.Drawing;
using Common;

namespace WvsGame.Packets
{
    public static class Global
    {
        public static void AddCharacterData(PacketWriter packet, Character chr)
        {
            packet.WriteInt(chr.mID);
            packet.WriteStringPad(chr.mName, 0, 13);
            packet.WriteByte(chr.mGender);
            packet.WriteByte(chr.mSkin);
            packet.WriteInt(chr.mFace);
            packet.WriteInt(chr.mHair);
            for (int i = 0; i < 3; ++i)
                packet.WriteLong(0); // pet serial numbers
            packet.WriteByte(chr.mPrimaryStats.Level);
            packet.WriteShort(chr.mPrimaryStats.Job);
            packet.WriteShort(chr.mPrimaryStats.Str);
            packet.WriteShort(chr.mPrimaryStats.Dex);
            packet.WriteShort(chr.mPrimaryStats.Int);
            packet.WriteShort(chr.mPrimaryStats.Luk);
            packet.WriteInt(chr.mPrimaryStats.HP);
            packet.WriteInt(chr.mPrimaryStats.MaxHP);
            packet.WriteInt(chr.mPrimaryStats.MP);
            packet.WriteInt(chr.mPrimaryStats.MaxMP);
            packet.WriteShort(chr.mPrimaryStats.AP);
            if (Tools.is_extendsp_job(chr.mPrimaryStats.Job))
            {
                int length = 0;
                for (int i = 0; i < chr.mPrimaryStats.SP.Length; ++i)
                    if (chr.mPrimaryStats.SP[i] > 0)
                        length++;
                packet.WriteByte(length);
                for (int i = 0; i < chr.mPrimaryStats.SP.Length; ++i)
                    if (chr.mPrimaryStats.SP[i] > 0)
                    {
                        packet.WriteByte(i + 1);
                        packet.WriteByte(chr.mPrimaryStats.SP[i]);
                    }
            }
            else
            {
                packet.WriteShort(chr.mPrimaryStats.SP[0]);
            }
            packet.WriteInt(chr.mPrimaryStats.EXP);
            packet.WriteInt(chr.mPrimaryStats.Fame);
            packet.WriteInt(0); // gacha exp according to other sources
            packet.WriteInt(chr.mField);
            packet.WriteByte(chr.mFieldPosition);
            packet.WriteInt(0); // time played in seconds according to other sources
            if (chr.mPrimaryStats.Job >= 430 && chr.mPrimaryStats.Job <= 434) // dual blade
                packet.WriteShort(1);
            else if (chr.mPrimaryStats.Job >= 530 && chr.mPrimaryStats.Job <= 532) // cannoneer
                packet.WriteShort(2);
            else
                packet.WriteShort(0);
            if (chr.mPrimaryStats.Job >= 3001 && chr.mPrimaryStats.Job <= 3112) // demon slayer
                packet.WriteInt(chr.mPrimaryStats.DemonSlayerAccessory);
            packet.WriteByte(chr.mPrimaryStats.Fatigue);
            packet.WriteInt(0); // time (year month day 00) 8 digets
            packet.WriteInt(chr.mTraits.Ambition);
            packet.WriteInt(chr.mTraits.Insight);
            packet.WriteInt(chr.mTraits.Willpower);
            packet.WriteInt(chr.mTraits.Diligence);
            packet.WriteInt(chr.mTraits.Empathy);
            packet.WriteInt(chr.mTraits.Charm);
            packet.WriteShort(chr.mTraits.AmbitionGained);     // buffer of 12 bytes here
            packet.WriteShort(chr.mTraits.InsightGained);      // buffer of 12 bytes here
            packet.WriteShort(chr.mTraits.WillpowerGained);    // buffer of 12 bytes here
            packet.WriteShort(chr.mTraits.DiligenceGained);    // buffer of 12 bytes here
            packet.WriteShort(chr.mTraits.EmpathyGained);      // buffer of 12 bytes here
            packet.WriteShort(chr.mTraits.CharmGained);        // buffer of 12 bytes here
            packet.WriteInt(chr.mPrimaryStats.BattleEXP);
            packet.WriteByte(chr.mPrimaryStats.BattleRank);
            packet.WriteInt(chr.mPrimaryStats.BattlePoints);
            packet.WriteByte(5);
            packet.WriteInt(0);
            packet.WriteInt((int)(DateTime.Now.ToFileTime() >> 32)); // FileTime.dwHighDateTime
            packet.WriteInt((int)(DateTime.Now.ToFileTime() << 32 >> 32)); // FileTime.dwLowDateTime
        }

        public static void AddAvatarData(PacketWriter packet, Character chr, bool IsMegaphone = false)
        {
            packet.WriteByte(chr.mGender);
            packet.WriteByte(chr.mSkin);
            packet.WriteInt(chr.mFace);
            packet.WriteInt(chr.mPrimaryStats.Job);
            packet.WriteBool(IsMegaphone);
            packet.WriteInt(chr.mHair);
            AddEquipData(packet, chr);
            if (chr.mPrimaryStats.Job >= 3001 && chr.mPrimaryStats.Job <= 3112) // demon slayer
                packet.WriteInt(chr.mPrimaryStats.DemonSlayerAccessory);
        }

        public static void AddEquipData(PacketWriter packet, Character chr)
        {
            foreach (KeyValuePair<sbyte, AbstractItem> pair in chr.mInventory[0].Where(pair => pair.Key >= -100))
            {
                packet.WriteByte(Math.Abs(pair.Key));
                packet.WriteInt(pair.Value.ItemId);
            }
            packet.WriteByte(-1);
            foreach (KeyValuePair<sbyte, AbstractItem> pair in chr.mInventory[0].Where(pair => pair.Key <= -100))
            {
                packet.WriteByte(Math.Abs(pair.Key));
                packet.WriteInt(pair.Value.ItemId);
            }
            packet.WriteByte(-1);
            packet.WriteInt(0);
            packet.WriteBool(chr.mPrimaryStats.Job / 100 == 23); // mercedes
            for (int i = 0; i < 3; ++i)
                packet.WriteInt(0); // pet itemid
        }

        public static void AddInventoryData(PacketWriter packet, Character chr)
        {
            packet.WriteInt(chr.mMeso);
            packet.WriteInt(0);
            packet.WriteByte(96); // equip slots
            packet.WriteByte(96); // consume slot cap
            packet.WriteByte(96); // install slot cap
            packet.WriteByte(96); // etc slot cap
            packet.WriteByte(96); // cash slot cap
            packet.WriteInt(0);
            packet.WriteInt(0);
            foreach (AbstractItem item in chr.mInventory[0].Values.Where(item => item.Position > -100))
                AddItemData(packet, item);
            packet.WriteShort(0); // end equipped
            foreach (AbstractItem item in chr.mInventory[0].Values.Where(item => item.Position < -100))
                AddItemData(packet, item);
            packet.WriteShort(0); // end cash equipped
            foreach (AbstractItem item in chr.mInventory[1].Values)
                AddItemData(packet, item);
            packet.WriteShort(0); // end equip
            packet.WriteShort(0); // end evan equipped
            packet.WriteShort(0); // end mech equipped
            packet.WriteShort(0); // end android equipped
            foreach (AbstractItem item in chr.mInventory[2].Values)
                AddItemData(packet, item);
            packet.WriteByte(0); // end consume
            foreach (AbstractItem item in chr.mInventory[3].Values)
                AddItemData(packet, item);
            packet.WriteByte(0); // end install
            foreach (AbstractItem item in chr.mInventory[4].Values)
                AddItemData(packet, item);
            packet.WriteByte(0); // end etc
            foreach (AbstractItem item in chr.mInventory[5].Values)
                AddItemData(packet, item);
            packet.WriteByte(0); // end cash
            packet.WriteInt(-1);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(0);
            //05 00 01 15 DF 0F 00 00 00 80 05 BB 46 E6 17 02 FF FF FF FF 07 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 40 E0 FD 3B 37 4F 01 FF FF FF FF 
            //06 00 01 2D 2D 10 00 00 00 80 05 BB 46 E6 17 02 FF FF FF FF 07 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 40 E0 FD 3B 37 4F 01 FF FF FF FF 
            //07 00 01 A6 5B 10 00 00 00 80 05 BB 46 E6 17 02 FF FF FF FF 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 40 E0 FD 3B 37 4F 01 FF FF FF FF 
            //0B 00 01 F0 DD 13 00 00 00 80 05 BB 46 E6 17 02 FF FF FF FF 07 00 00 00 00 00 00 00 00 00 00 00 00 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 40 E0 FD 3B 37 4F 01 FF FF FF FF 
            //37 00 01 20 E2 11 00 00 00 80 05 BB 46 E6 17 02 FF FF FF FF 00 00 01 00 01 00 01 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 FF FF FF FF FF FF FF FF FF FF FF FF FF FF 00 40 E0 FD 3B 37 4F 01 FF FF FF FF 

        }

        //0005 01 000FDF15 00 00-00-00-00-00-00-00-00 FFFFFFFF 07 00 0000 0000 0000 0000 0000 0000 0000 0000 0003 0000 0000 0000 0000 0000 0000 "Ij#z쨳" 0000 
        //00 01 00000000 FFFFFFFF 00000000 0000 00 00 0000 0000 0000 0000 0000 0000 FFFF FFFF FFFF 00-00-00-00-00-00-00-00 00-40-E0-FD-3B-37-4F-01 FFFFFFFF 
        public static void AddItemData(PacketWriter packet, AbstractItem aitem)
        {
            Equip eitem = null;
            Item item = null;
            if (aitem.Position < 0)
                eitem = (Equip)aitem;
            else
                item = (Item)aitem;
            if (eitem == null)
                packet.WriteByte(item.Position);
            else
                packet.WriteShort(Math.Abs(eitem.Position));
            packet.WriteByte(eitem == null ? 2 : 1);
            packet.WriteInt(aitem.ItemId);
            packet.WriteByte(aitem.cash);
            if (aitem.cash > 0)
                packet.WriteLong(aitem.specialID);
            packet.WriteLong(aitem.Expiration);
            packet.WriteInt(-1);
            if (eitem != null)
            {
                packet.WriteByte(eitem.PossibleUpgrades);
                packet.WriteByte(0); // level
                packet.WriteShort(eitem.Str);
                packet.WriteShort(eitem.Dex);
                packet.WriteShort(eitem.Int);
                packet.WriteShort(eitem.Luk);
                packet.WriteShort(eitem.IncHP);
                packet.WriteShort(eitem.IncMP);
                packet.WriteShort(eitem.Watk);
                packet.WriteShort(eitem.Matk);
                packet.WriteShort(eitem.Wdef);
                packet.WriteShort(eitem.Mdef);
                packet.WriteShort(eitem.Accuracy);
                packet.WriteShort(eitem.Avoid);
                packet.WriteShort(0); // hands
                packet.WriteShort(eitem.Speed);
                packet.WriteShort(eitem.Jump);
                packet.WriteMapleString(eitem.Owner);
                packet.WriteShort(eitem.Flag);
                packet.WriteByte(0); // skill
                packet.WriteByte(1); // base level
                packet.WriteInt(0); // item exp
                packet.WriteInt(-1); // eitem.Durability 
                packet.WriteInt(0); // vicious
                packet.WriteShort(0); // pvp damage
                packet.WriteByte(eitem.State);
                packet.WriteByte(eitem.Enhancements);
                packet.WriteShort(eitem.Pot1);
                if (eitem.specialID == 0)
                {
                    packet.WriteShort(eitem.Pot2);
                    packet.WriteShort(eitem.Pot3);
                    packet.WriteShort(eitem.Pot4);
                    packet.WriteShort(eitem.Pot5);
                }
                packet.WriteShort(eitem.SocketMask);
                packet.WriteShort(-1); // eitem.Socket1
                packet.WriteShort(-1); // eitem.Socket2
                packet.WriteShort(-1); // eitem.Socket3
                packet.WriteLong(0); // inventory id
                packet.WriteLong(0x217E57D909BC000);
                packet.WriteInt(-1);
            }
            else
            {
                packet.WriteShort(item.Quantity);
                packet.WriteMapleString(item.Owner);
                packet.WriteShort(item.Flag);
                //packet.WriteLong(-1); // if is bullet/star
            }
        }

        public static void AddSkillData(PacketWriter packet, Character chr)
        {
            /*
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);*/
            List<Skill> cooldowns = new List<Skill>();
            packet.WriteByte(1);
            packet.WriteShort(chr.mSkills.Count);
            foreach (Skill s in chr.mSkills)
            {
                if (s.Cooldown > DateTime.Now.ToFileTime())
                    cooldowns.Add(s);
                packet.WriteInt(s.SkillID);
                packet.WriteInt(s.SkillLevel);
                packet.WriteLong(s.Expiration);
                if (s.MasterLevel != 0)
                    packet.WriteInt(s.SkillMastery);
            }
            packet.WriteShort(cooldowns.Count);
            foreach (Skill s in cooldowns)
            {
                packet.WriteInt(s.SkillID);
                packet.WriteShort(DateTime.FromFileTime(s.Cooldown).Subtract(DateTime.Now).Seconds);
            }
        }

        public static void AddQuestData(PacketWriter packet, Character chr)
        {
            packet.WriteByte(1);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteByte(1);
            packet.WriteShort(0);
        }

        public static void AddRingData(PacketWriter packet, Character chr)
        {
            packet.WriteByte(0); // PartnerRing - long(ringId) long(partnerRingId) int(itemid)
            packet.WriteByte(0); // PartnerRing - long(ringId) long(partnerRingId) int(itemid)
            packet.WriteByte(0); // MarrageRing - int(cid) int(parnterCId) int(itemid)
        }

        public static void AddRockData(PacketWriter packet, Character chr)
        {
            for (int i = 0; i < 5; ++i)
                packet.WriteInt(999999999);
            for (int i = 0; i < 10; ++i)
                packet.WriteInt(999999999);
            for (int i = 0; i < 13; ++i)
                packet.WriteInt(999999999);
            for (int i = 0; i < 13; ++i)
                packet.WriteInt(999999999);
        }

        public static void AddMonsterBookData(PacketWriter packet, Character chr)
        {
            packet.WriteInt(0);
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteInt(-1);
        }

        public static void AddPartyQuestData(PacketWriter packet, Character chr)
        {
            packet.WriteShort(0);
        }
    }

    public static class CWvsContext
    {
        public static byte[] GMBoard(string url)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.GMBoard);
            packet.WriteInt(Environment.TickCount); // actually object id of some sort
            packet.WriteMapleString(url);
            return packet.ToArray();
        }

        // 0: [Notice] <Msg>
        // 1: Popup <Msg>
        // 2: Megaphone
        // 3: Super Megaphone 
        // 4: Server Message
        // 5: Pink Text
        // 6: LightBlue Text ({} as Item)
        // 7: [int] -> Keep Wz Error
        // 8: Item Megaphone
        // 9: Item Megaphone
        // 10: Three Line Megaphone
        // 11: Item Megaphone
        // 12: Weather Effect
        // 13: Green Gachapon Message
        // 14: Orange Text
        // 15: Twin Dragon's Egg (got)
        // 16: Twin Dragon's Egg (duplicated)
        // 17: Lightblue Text
        // 18: Lightblue Text
        // 20: LightBlue Text ({} as Item)
        // 22: Skull Megaphone
        // 23: Ani Message
        // 25: Cake Pink Message
        // 26: Pie Yellow Message
        public static byte[] BroadcastMessage(byte type, string message)
        {
            int channel = 0;
            bool megaEar = true;
            var packet = new PacketWriter();
            packet.WriteOpcode(SendOps.BroadcastMsg);
            packet.WriteByte(type);
            if (type == 4)
                packet.WriteByte(1);
            if (type != 23 && type != 24)
                packet.WriteMapleString(message);
            switch (type)
            {
                case 3: // Super Megaphone
                case 22: // Skull Megaphone
                case 25:
                case 26:
                    packet.WriteByte(channel - 1);
                    packet.WriteByte(megaEar ? 1 : 0);
                    break;
                case 9: // Like Item Megaphone (Without Item)
                    packet.WriteByte(channel - 1);
                    break;
                case 12: // Weather Effect
                    packet.WriteInt(channel); // item id
                    break;
                case 6:
                case 11:
                case 20:
                    packet.WriteInt(channel >= 1000000 && channel < 6000000 ? channel : 0); // Item Id
                    //E.G. All new EXP coupon {Ruby EXP Coupon} is now available in the Cash Shop!
                    break;
                case 24:
                    packet.WriteShort(0); // ?
                    break;
            }
            return packet.ToArray();
        }
    }

    public static class CUserPool
    {
        public static byte[] UserEnterField(Character chr)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.UserEnterField);
            packet.WriteInt(chr.mID);
            packet.WriteByte(chr.mPrimaryStats.Level);
            packet.WriteMapleString(chr.mName);
            packet.WriteMapleString(""); // ultimate explourer
            packet.WriteMapleString("와라오~"); // guild
            packet.WriteShort(0x3FE); // guild
            packet.WriteByte(0x10); // guild
            packet.WriteShort(0xFAC); // guild
            packet.WriteByte(0x10); // guild
            int[] buffs = new int[8];
            foreach (Buff buff in chr.mBuffs)
                buffs[buff.Position] |= buff.BuffID; 
            //foreach (int i in buffs)
            //    packet.WriteInt(i); // buffer 32
            packet.WriteHexString("00 00 00 FE 00 00 A0 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ");
            //buff values

            packet.WriteInt(-1); // end buff values reading ?

            //packet.WriteByte(0);
            //packet.WriteByte(0);
            //packet.WriteByte(0);
            //packet.WriteByte(0); 

            //packet.WriteBytes(new byte[4]); 
            //packet.WriteBytes(new byte[4]);
            //packet.WriteByte(1);

            packet.WriteHexString("00 00 00 00 00 00 00 00 00 00 00 00 01");

            packet.WriteInt(Environment.TickCount);
            packet.WriteShort(0); 
            packet.WriteBytes(new byte[4]);
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteShort(0); 
            packet.WriteBytes(new byte[4]);
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteShort(0); 
            packet.WriteBytes(new byte[4]); 
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteBytes(new byte[4]);
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteByte(1);
            packet.WriteInt(0);
            packet.WriteShort(0);
            packet.WriteBytes(new byte[4]);
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteBytes(new byte[4]);
            packet.WriteBytes(new byte[4]);
            packet.WriteByte(1);
            packet.WriteInt(Environment.TickCount);
            packet.WriteShort(0);

            packet.WriteShort(chr.mPrimaryStats.Job);
            packet.WriteShort(0);
            Global.AddAvatarData(packet, chr);
            packet.WriteInt(0);
            packet.WriteBytes(new byte[12]);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);

            packet.WriteShort(chr.mPosition.X);
            packet.WriteShort(chr.mPosition.Y);
            packet.WriteByte(0); // chr.mStance
            packet.WriteShort(chr.mFoothold);

            packet.WriteByte(0);
            packet.WriteByte(0);

            packet.WriteByte(1);
            packet.WriteByte(0);

            packet.WriteInt(1); // mount level
            packet.WriteInt(0); // mount exp
            packet.WriteInt(0); // mount fatigue

            packet.WriteByte(0); // ``Announce Box''

            packet.WriteByte(0); // Chalk Board

            Global.AddRingData(packet, chr);
            Global.AddRingData(packet, chr);
            Global.AddRingData(packet, chr);

            packet.WriteByte(0); // berserk
            packet.WriteInt(0);
            packet.WriteByte(0); // new year cards
            packet.WriteInt(0);
            return packet.ToArray();
        }

        public static byte[] PlayerLeaveField(int cid)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.PlayerLeaveField);
            packet.WriteInt(cid);
            return packet.ToArray();
        }

        public static byte[] PublicChatMessage(int cidfrom, string msg, bool whitebg, bool hide)
        {
            var packet = new PacketWriter();
            packet.WriteOpcode(SendOps.PublicChatMsg);
            packet.WriteInt(cidfrom);
            packet.WriteBool(whitebg);
            packet.WriteMapleString(msg);
            packet.WriteBool(hide);
            return packet.ToArray();
        }
    }

    public static class CField
    {
        // 0 = public
        // 1 = lime
        // 2 = pink
        // 3 = orange
        // 4 = purple
        // 5 = green
        // 6 = magenta
        // 7 = gray
        // 8 = yellow
        // 9 = light yellow
        // A = cyan
        // B = white background
        // C = red
        // D = ligt blue background
        public static byte[] ChatMessage(short type, string message)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.ChatMessage);
            packet.WriteShort(type);
            packet.WriteMapleString(message);
            return packet.ToArray();
        }
    }

    public static class CStage
    {
        public static byte[] SetField(Client c, bool connecting = false)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.SetField);
            packet.WriteShort(2);
            packet.WriteInt(1);
            packet.WriteInt(0);
            packet.WriteInt(2);
            packet.WriteInt(0);
            packet.WriteInt(Program.mServer.ServerId);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteByte(1);
            packet.WriteInt(0);
            packet.WriteBool(connecting);
            if (!connecting)
            {
                packet.WriteInt(c.mCharacter.mField);
                packet.WriteByte(c.mCharacter.mFieldPosition);
                packet.WriteInt(c.mCharacter.mPrimaryStats.HP);
                packet.WriteByte(0);
                packet.WriteLong(c.mAccount.conauth);
                packet.WriteInt(100);
                packet.WriteByte(0);
                packet.WriteByte(0);
                return packet.ToArray();
            }
            packet.WriteShort(0);
            packet.WriteInt(unchecked((int)(0xF05A5CB3))); // rand
            packet.WriteInt(unchecked((int)(0xF05A5CB3))); // rand
            packet.WriteInt(unchecked((int)(0xF05A5CB3))); // rand

            ulong flagmask = (unchecked((ulong)-1));
            flagmask = (flagmask & 0xFFFFFFFFF0FFFFFF) | ((ulong)0x01 << 24);
            flagmask = (flagmask & 0xFFFFFFF0FFFFFFFF) | ((ulong)0x0D << 32);
            flagmask = (flagmask & 0x0FFFFFFFFFFFFFFF) | ((ulong)0x01 << 60);
            packet.WriteLong(unchecked((long)flagmask)); // flags?

            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteByte(0);
            
            Global.AddCharacterData(packet, c.mCharacter);
            packet.WriteByte(0x0A); // buddylist cap
            packet.WriteByte(0); // blessing of fairy
            packet.WriteByte(0); // blessing of empress
            packet.WriteByte(0); // ultimate explorer
            Global.AddInventoryData(packet, c.mCharacter);
            Global.AddSkillData(packet, c.mCharacter);
            //packet.WriteHexString("01 07 00 0C 00 00 00 00 00 00 00 00 80 05 BB 46 E6 17 02 49 00 00 00 00 00 00 00 00 80 05 BB 46 E6 17 02 10 2D 31 01 FF FF FF FF 00 80 05 BB 46 E6 17 02 0F 2D 31 01 FF FF FF FF 00 80 05 BB 46 E6 17 02 0E 2D 31 01 FF FF FF FF 00 80 05 BB 46 E6 17 02 12 2D 31 01 FF FF FF FF 00 80 05 BB 46 E6 17 02 11 2D 31 01 FF FF FF FF 00 80 05 BB 46 E6 17 02 00 00 01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B 00 00 00 00 00 00 00 FF FF FF FF 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 70 53 A7 D3 06 6E CD 01 64 00 00 00 00 01");

            Global.AddQuestData(packet, c.mCharacter);
            //Global.AddRingData(packet, c.mCharacter);//01 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 
            packet.WriteShort(0); // rings...something like that ?
            packet.WriteShort(0); // rings...something like that ?
            packet.WriteShort(0); // rings...something like that ?
            packet.WriteShort(0); // rings...something like that ?
            //FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B FF C9 9A 3B 
            Global.AddRockData(packet, c.mCharacter);
            //00 00 00 00 00 00 00 FF FF FF FF 
            Global.AddMonsterBookData(packet, c.mCharacter);
            packet.WriteShort(0);
            packet.WriteShort(0);
            //AddPartyQuestData(packet, c.mCharacter);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            for (int i = 0; i < 17; ++i)
                packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteByte(1);
            packet.WriteShort(0);
            packet.WriteShort(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteInt(0);
            packet.WriteLong(0x1CD6E06D3A75370);
            packet.WriteInt(100);
            packet.WriteByte(0);
            packet.WriteByte(1);

            return packet.ToArray();
        }
    }

    public static class CNpcPool
    {
        public static byte[] NpcEnterField(Npc npc)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.NpcEnterField);
            packet.WriteInt(npc.mObjectID);
            packet.WriteInt(npc.mID);
            packet.WriteShort(npc.mX);
            packet.WriteShort(npc.mCy);
            packet.WriteByte(npc.mF);
            packet.WriteShort(npc.mFh);
            packet.WriteShort(npc.mRx0);
            packet.WriteShort(npc.mRx1);
            packet.WriteBool(true); // visible
            return packet.ToArray();
        }

        public static byte[] NpcLeaveField(Npc npc)
        {
            var packet = new PacketWriter();
            packet.WriteOpcode(SendOps.NpcLeaveField);
            packet.WriteInt(npc.mObjectID);
            return packet.ToArray();
        }
    }
}
