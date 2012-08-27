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
using Common;
using WvsLogin.User;
using System.Drawing;
using WvsLogin.WZ;

namespace WvsLogin.Packets
{
    class PacketDefinitions
    {
        public static byte[] Blocked(byte reason, long time)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckPassword);
            packet.WriteByte(2);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteByte(reason);
            packet.WriteLong(time);
            return packet.ToArray();
        }

        public static byte[] LoginFailed(byte reason)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckPassword);
            packet.WriteByte(reason);
            packet.WriteByte(0);
            packet.WriteInt(0);
            return packet.ToArray();
        }

        public static byte[] LoginSuccess(Client c)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckPassword);
            packet.WriteByte(0);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteInt(c.AccountId);
            packet.WriteByte(0); // Gender
            packet.WriteByte(c.Admin);
            packet.WriteShort(0); // GM Mask           
            packet.WriteByte(0);
            packet.WriteMapleString(c.Username);
            packet.WriteByte(3);
            packet.WriteByte(c.TradeBlock);
            packet.WriteLong(c.TradeBlockExpiration);
            packet.WriteByte(1);
            packet.WriteLong(0); // Creation date. That's not important!
            packet.WriteInt(0x0C); // Tooltip bubble at World/Channel select
            packet.WriteByte(1); // Use SessionID
            packet.WriteByte(1);
            packet.WriteLong(c.SessionID);
            return packet.ToArray();
        }

        public static byte[] WorldInformation(byte serverId, string worldName, byte flag, string eventmessage, short expPercent, short dropPercent, Dictionary<int, int> channels)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.WorldInformation);
            packet.WriteByte(serverId);
            packet.WriteMapleString(worldName);
            packet.WriteByte(flag); // 1 = event, 2 = new, 3 = hot time
            packet.WriteMapleString(eventmessage);
            packet.WriteShort(expPercent);
            packet.WriteShort(dropPercent);
            packet.WriteByte(0);
            packet.WriteByte(channels.Count);

            foreach (KeyValuePair<int, int> channel in channels)
            {
                packet.WriteMapleString(worldName + "-" + (channel.Key + 1));
                packet.WriteInt(channel.Value * 100); // max is 1000
                packet.WriteByte(0);
                packet.WriteByte(channel.Key);
                packet.WriteByte(0);
            }

            Dictionary<Point, string> chatBubbles = new Dictionary<Point, string>();
            chatBubbles.Add(new Point(0, 265), "왛라오! ㅇㅇ");
            chatBubbles.Add(new Point(500, 370), "ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ");
            packet.WriteShort(chatBubbles.Count);

            foreach (KeyValuePair<Point, string> pair in chatBubbles)
            {
                packet.WriteShort(pair.Key.X);
                packet.WriteShort(pair.Key.Y);
                packet.WriteMapleString(pair.Value);
            }

            packet.WriteInt(0);
            return packet.ToArray();
        }

        public static byte[] WorldInformationEnd()
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.WorldInformation);
            packet.WriteByte(0xFF);
            return packet.ToArray();
        }

        public static byte[] RecommendWorldMessage()
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.RecommendWorldMessage);
            Dictionary<int, string> messages = new Dictionary<int, string>();
            messages.Add(0, "There is only one world, so I'm not sure why you are clicking this button.\r\n\r\n설치 겐투♥");

            packet.WriteByte(messages.Count);
            foreach (KeyValuePair<int, string> pair in messages)
            {
                packet.WriteInt(pair.Key);
                packet.WriteMapleString(pair.Value);
            }
            return packet.ToArray();
        }

        public static byte[] LastConnectedWorld(int world)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.LatestConnectedWorld);
            packet.WriteInt(world);
            return packet.ToArray();
        }
        
        public static byte[] CheckUserLimit(int mod0, int mod1)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckUserLimit);
            packet.WriteByte(mod0); // popup         0 = none, 1 = high, 2 =max
            packet.WriteByte(mod1);
            return packet.ToArray();
        }

        public static byte[] CharacterLoadout(User.Client client)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.WorldSelect); //  0B 00 00 00 01 00 03 00 00 00 00 00 00 00 D0 5E CD 01 D0 CF E2 DD
            packet.WriteByte(0);
            packet.WriteByte(client.Characters.Count);
            foreach (Character chr in client.Characters)
            {
               AddCharacterEntry(packet, chr);
            }
            //packet.WriteHexString("6E B9 79 00 6D 65 72 63 65 64 65 73 65 77 00 00 00 00 0C 4D 50 00 00 AD 82 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 D2 07 0C 00 05 00 04 00 04 00 32 00 00 00 32 00 00 00 05 00 00 00 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 70 C9 3F 36 00 00 00 00 00 00 00 00 05 A2 ED 77 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 68 95 CC 02 F0 FB D7 0D 0B 3B 37 4F 01 00 40 E0 FD 00 0C 4D 50 00 00 D2 07 00 00 00 AD 82 00 00 05 50 06 10 00 07 87 5D 10 00 0B 76 39 17 00 37 20 E2 11 00 FF FF 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 01 22 7F 27 00 BB 05 00 00 94 4E 00 00 39 00 00 00 6F B9 79 00 6D 65 72 63 65 64 65 73 65 75 00 00 00 00 0C 45 50 00 00 AD 82 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 D2 07 0C 00 05 00 04 00 04 00 32 00 00 00 32 00 00 00 05 00 00 00 05 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 70 C9 3F 36 00 DB 01 00 00 00 00 00 05 A2 ED 77 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 00 00 00 00 00 00 00 58 04 00 00 BD 7D 20 CD 01 D0 54 06 40 00 0C 45 50 00 00 D2 07 00 00 00 AD 82 00 00 05 50 06 10 00 07 87 5D 10 00 0B 76 39 17 00 37 20 E2 11 00 FF FF 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 01 22 7F 27 00 BB 05 00 00 94 4E 00 00 39 00 00 00");
            packet.WriteByte(1); // second password set
            packet.WriteByte(0); // ?
            packet.WriteInt(3); // character slots
            packet.WriteInt(0); // ``click-here'' cash shop slots. overwrites free character slots, however the location vector is wrong.
            return packet.ToArray();
        }

        public static byte[] NewCharacter(Character chr)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CreateNewCharacter);
            packet.WriteByte(0);
            AddCharacterEntry(packet, chr, true, false);
            return packet.ToArray();
        }

        public static void AddCharacterEntry(PacketWriter packet, Character chr, bool IsNew = false, bool VAC = false)
        {
            AddPlayerStats(packet, chr);
            AddAvatarData(packet, chr);
            if (!VAC)
                packet.WriteByte(0); // >.<
            packet.WriteBool(!IsNew);
            if (!IsNew)
            {
                packet.WriteInt(chr.mWorldPos + 5);
                packet.WriteInt(chr.mWorldPos + 5 - chr.mWorldOldPos);
                packet.WriteInt(chr.mJobPos + 5);
                packet.WriteInt(chr.mJobPos + 5 - chr.mJobOldPos);
            }
        }

        public static byte[] DeleteCharacter(int cid, int mode)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.DeleteCharacter);
            packet.WriteInt(cid);
            packet.WriteByte(mode); // 0 = okay, 12 = invalid birthday, 14 = invalid pic
            return packet.ToArray();
        }

        public static void AddPlayerStats(PacketWriter packet, Character chr)
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
            packet.WriteInt(chr.mMap);
            packet.WriteByte(chr.mMapPosition);
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
            packet.WriteShort(chr.mTraits.AmbitionGained);
            packet.WriteShort(chr.mTraits.InsightGained);
            packet.WriteShort(chr.mTraits.WillpowerGained);
            packet.WriteShort(chr.mTraits.DiligenceGained);
            packet.WriteShort(chr.mTraits.EmpathyGained);
            packet.WriteShort(chr.mTraits.CharmGained);

            packet.WriteInt(chr.mPrimaryStats.BattleEXP);
            packet.WriteByte(chr.mPrimaryStats.BattleRank);
            packet.WriteInt(chr.mPrimaryStats.BattlePoints);
            packet.WriteByte(5);
            packet.WriteInt(0);
            // could always just write a buffer of 8, but oh well lol
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
            foreach (KeyValuePair<sbyte, AbstractItem> pair in chr.mInventory[0])
            {
                if (pair.Key < -100)
                    continue;
                packet.WriteByte(Math.Abs(pair.Key));
                packet.WriteInt(pair.Value.ItemId);
            }
            packet.WriteByte(-1);
            foreach (KeyValuePair<sbyte, AbstractItem> pair in chr.mInventory[0])
            {
                if (pair.Key > -100)
                    continue;
                packet.WriteByte(Math.Abs(pair.Key));
                packet.WriteInt(pair.Value.ItemId);
            }
            packet.WriteByte(-1);
            packet.WriteInt(0);
            packet.WriteBool(chr.mPrimaryStats.Job / 100 == 23); // mercedes
            for (int i = 0; i < 3; ++i)
                packet.WriteInt(0); // pet itemid
        }

        public static byte[] CheckDuplicatedID(string name, byte valid)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckDuplicatedID);
            packet.WriteMapleString(name);
            packet.WriteByte(valid);
            return packet.ToArray();
        }

        public static byte[] ViewAllCharacters(List<Character> characters)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.ViewAllChar);

            packet.WriteByte(0x03);
            packet.WriteBool(false);
            return packet.ToArray();
            /*
            if (characters.Count < 1)
            {
                packet.WriteByte(0x04);
                return packet.ToArray();
            }
            packet.WriteByte(0x00);
            packet.WriteByte(0x00); // world id
            packet.WriteByte(characters.Count);
            for (int i = 0; i < characters.Count; ++i)
            {
                AddCharacterEntry(packet, characters[i], false, true);
            }
            packet.WriteByte(0x01); // pic
            return packet.ToArray();*/
        }

        public static byte[] BadPic()
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.CheckSPW);
            packet.WriteByte(0);
            return packet.ToArray();
        }

        public static byte[] SelectCharacter(byte primary, byte secondary, byte[] IP = null, ushort port = 0, int clientID = 0)
        {
            PacketWriter packet = new PacketWriter();
            packet.WriteOpcode(SendOps.SelectCharacter);
            packet.WriteByte(primary);
            packet.WriteByte(secondary);
            packet.WriteBytes(IP);
            packet.WriteShort(port);
            packet.WriteInt(clientID);
            packet.WriteByte(0);
            packet.WriteInt(0);
            packet.WriteByte(0);
            packet.WriteShort(0);
            packet.WriteShort(0);
            return packet.ToArray();
        }

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

        public static byte[] KeepAlive()
        {
            var packet = new PacketWriter();
            packet.WriteOpcode(SendOps.AliveReq);
            return packet.ToArray();
        }
    }
}
