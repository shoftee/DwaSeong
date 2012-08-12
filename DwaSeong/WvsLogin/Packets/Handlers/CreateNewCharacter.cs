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
using WvsLogin.User;
using MapleLib.PacketLib;
using Common;
using Common.Constants;
using WvsLogin.Packets;
using WvsLogin.WZ;

namespace WvsLogin.Packets.Handlers
{
    class CreateNewCharacter : IPacketHandler
    {
        //[7/15/2012 3:18:09 PM][대타] 받은 패킷 2A 00 05 00 64 65 73 75 61 02 00 00 00 00
        // 00 00 01 08 25 4E 00 00 CE 77 00 00 07 00 00 00 01 00 00 00 86 DE 0F 00 2E 2D 1
        //0 00 85 5B 10 00 8B DE 13 00
        /*
         * 2A 00 05 00 64 65 73 75 61 02 00 00 00 00 00 00 01 08 25 4E 00 00 CE 77 00 00 07 00 00 00 01 00 00 00 86 DE 0F 00 2E 2D 10 00 85 5B 10 00 8B DE 13 00
         * 2A 00 
         * 05 00 64 65 73 75 61 
         * 02 00 00 00 
         * 00 00 
         * 00 
         * 01 08 
         * 25 4E 00 00 
         * CE 77 00 00 
         * 07 00 00 00 
         * 01 00 00 00 
         * 86 DE 0F 00 
         * 2E 2D 10 00 
         * 85 5B 10 00 
         * 8B DE 13 00
         */
        private static int[] MALE_FACES = {20000, 20001, 20002, 20018, 20004, 20019, 20005, 20010};
        private static int[] FEMALE_FACES = {21000, 21001, 21002, 21003, 21005, 21008, 21009, 21017};
        private static int[] MALE_HAIR = {30030, 30020, 30000, 30050, 30120, 30270, 30310, 30670};
        private static int[] FEMALE_HAIR = {31000, 31040, 31050, 31100, 31010, 31030, 31120, 31240};
        private static int[] MALE_TOPS = {1040002, 1040006, 1040010, 1040149, 1040150, 1040151, 1040152, 1040153};
        private static int[] FEMALE_TOPS = {1041002, 1041006, 1041010, 1041011, 1041151, 1041152, 1041153, 1041154, 1041155};
        private static int[] MALE_BOTTOMS = {1060002, 1060006, 1060140, 1060141, 1060142, 1060143, 1060144};
        private static int[] FEMALE_BOTTOMS = {1061002, 1061008, 1061161, 1061162, 1061163, 1061164, 1061165};
        private static int[] SHOES = {1072001, 1072005, 1072037, 1072038, 1072497, 1072498, 1072499, 1072500, 1072501};
        private static int[] OTHER_MALE_FACES = {20100, 20401, 20402, 20018, 20004, 20019, 20005, 20010};
        private static int[] OTHER_FEMALE_FACES = {21700, 21201, 21002, 21003, 21005, 21008, 21009, 21017};
        private static int[] ARAN_MALE_TOPS = {1042167, 1040149, 1040150, 1040151, 1040152, 1040153};
        private static int[] ARAN_FEMALE_TOPS = {1042167, 1041151, 1041152, 1041153, 1041154, 1041155};
        private static int[] ARAN_MALE_BOTTOMS = {1062115, 1060140, 1060141, 1060142, 1060143, 1060144};
        private static int[] ARAN_FEMALE_BOTTOMS = {1062115, 1061161, 1061162, 1061163, 1061164, 1061165};
        private static int[] ARAN_SHOES = {1072383, 1072497, 1072498, 1072499, 1072500, 1072501};
        private static int[] EVAN_MALE_TOPS = {1042180, 1040149, 1040150, 1040151, 1040152, 1040153};
        private static int[] EVAN_FEMALE_TOPS = {1042180, 1041151, 1041152, 1041153, 1041154, 1041155};
        private static int[] EVAN_MALE_BOTTOMS = {1060138, 1060140, 1060141, 1060142, 1060143, 1060144};
        private static int[] EVAN_FEMALE_BOTTOMS = {1061160, 1061161, 1061162, 1061163, 1061164, 1061165};
        private static int[] EVAN_SHOES = {1072418, 1072497, 1072498, 1072499, 1072500, 1072501};
        private static int[] EVAN_WEAPONS = {1302132, 1302154, 1322092, 1302155, 1302156, 1302157, 1322093};
        private static int[] WEAPONS = {1302000, 1322005, 1312004, 1302154, 1322092, 1302155, 1302156, 1302157, 1322093};
        private static int[] RESISTANCE_MALE_OVERALLS = {1050173, 1050174, 1050175, 1050181, 1050182, 1050183, 1050184, 1050185};
        private static int[] RESISTANCE_FEMALE_OVERALLS = {1051214, 1051215, 1051216, 1051222, 1051223, 1051224, 1051225, 1051226};
        private static int[] HAIR_COLORS = {0, 7, 3, 2};
        private static int[] ACCESORIES = {1012276, 1012277, 1012278, 1012279, 1012280};

        public void handlePacket(Client c, PacketReader packet)
        {
            string charname = packet.ReadMapleString();
            int jobtype = packet.ReadInt();
            short specialjobtype = packet.ReadShort();
            byte gender = packet.ReadByte();
            packet.Skip(2);
            int face = packet.ReadInt();
            int hair = packet.ReadInt();
            int haircolor = packet.ReadInt();
            int skin = packet.ReadInt();
            int top = packet.ReadInt();
            int bottom = 0;
            if (jobtype < 5)
                bottom = packet.ReadInt();
            int shoes = packet.ReadInt();
            int weapon = packet.ReadInt();
            int shield = 0;
            if (jobtype == 6)
                shield = packet.ReadInt();
            Character newchr = new Character();
            newchr.mName = charname;
            if (jobtype == 0)
            {
                newchr.mPrimaryStats.Job = (short)Job.Citizen;
            }
            else if (jobtype == 1)
            {
                newchr.mPrimaryStats.Job = (short)Job.Beginner;
            }
            else if (jobtype == 2)
            {
                newchr.mPrimaryStats.Job = (short)Job.Noblesse;
            }
            else if (jobtype == 3)
            {
                newchr.mPrimaryStats.Job = (short)Job.Legend;
            }
            else if (jobtype == 4)
            {
                newchr.mPrimaryStats.Job = (short)Job.Evan1;
            }
            else if (jobtype == 5)
            {
                newchr.mPrimaryStats.Job = (short)Job.Mercedes;
            }
            else if (jobtype == 6)
            {
                newchr.mPrimaryStats.Job = (short)Job.DemonSlayer;
            }
            newchr.mMap = 100000000;
            newchr.mClient = c;
            newchr.mHair = hair + haircolor;
            newchr.mFace = face;
            newchr.mSkin = (byte)skin;
            newchr.mGender = gender;
            
            newchr.mPrimaryStats.Level = 1;
            newchr.mPrimaryStats.HP = 50;
            newchr.mPrimaryStats.MaxHP = 50;
            newchr.mPrimaryStats.MP = 50;
            newchr.mPrimaryStats.MaxMP = 50;
            newchr.mPrimaryStats.Str = 4;
            newchr.mPrimaryStats.Dex = 4;
            newchr.mPrimaryStats.Int = 4;
            newchr.mPrimaryStats.Luk = 4;

            Equip eweapon = new Equip(weapon, "Character creation (JobId " + jobtype + ")");
            eweapon.Watk = 17;
            eweapon.Position = -11;
            newchr.mInventory[0].Add(eweapon.Position, eweapon);

            if (shield > 0)
            {
                Equip eshield = new Equip(shield, "Character creation (JobId " + jobtype + ")");
                eshield.Position = -10;
                newchr.mInventory[0].Add(eshield.Position, eshield);
            }

            Equip etop = new Equip(top, "Character creation (JobId " + jobtype + ")");
            etop.Position = -5;
            newchr.mInventory[0].Add(etop.Position, etop);

            if (bottom > 0)
            {
                Equip ebottom = new Equip(bottom, "Character creation (JobId " + jobtype + ")");
                ebottom.Position = -6;
                newchr.mInventory[0].Add(ebottom.Position, ebottom);
            }
            Equip eshoes = new Equip(shoes, "Character creation (JobId " + jobtype + ")");
            eshoes.Position = -7;
            newchr.mInventory[0].Add(eshoes.Position, eshoes);

            Database.SaveCharacter(newchr, true);
            c.SendPacket(PacketDefinitions.NewCharacter(newchr));
        }
    }
}