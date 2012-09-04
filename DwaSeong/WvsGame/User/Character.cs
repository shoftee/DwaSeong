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
using WvsGame.Packets;
using WvsGame.Movement;
using System.Drawing;
using MapleLib.PacketLib;
using WvsGame.Field.Entity;
using WvsGame.WZ;
using Common;

namespace WvsGame.User
{
    public class Character : ISpawnable, IMovable
    {
        public int mID { get; set; }
        public string mName { get; set; }
        public byte mGender { get; set; }
        public byte mSkin { get; set; }
        public int mHair { get; set; }
        public int mFace { get; set; }

        public int mField { get; set; }
        public byte mFieldPosition { get; set; }
        public int mFieldInstance { get; set; }

        public int mMeso { get; set; }

        public int mWorldPos { get; set; }
        public int mWorldOldPos { get; set; }
        public int mJobPos { get; set; }
        public int mJobOldPos { get; set; }

        public Dictionary<sbyte, AbstractItem>[] mInventory { get; set; }

        public List<Skill> mSkills { get; set; }
        public List<Buff> mBuffs { get; set; }

        public PrimaryStats mPrimaryStats { get; set; }
        public Traits mTraits { get; set; }

        public Client mClient { get; set; }

        public Point mPosition { get; set; }
        public int mStance { get; set; }
        public int mFoothold { get; set; }

        public Guild mGuild { get; set; }

        public Character()
        {
            mPrimaryStats = new PrimaryStats();
            mTraits = new Traits();
            mInventory = new Dictionary<sbyte, AbstractItem>[6];
            for (int i = 0; i < 6; i++)
                mInventory[i] = new Dictionary<sbyte, AbstractItem>();
            mSkills = new List<Skill>();
            mBuffs = new List<Buff>();
            mPrimaryStats.SP = new short[4];
            mStance = 4;   
        }

        public PacketWriter GenerateSpawnPacket()
        {
            return new PacketWriter(CUserPool.UserEnterField(this));
        }

        public PacketWriter GenerateDespawnPacket()
        {
            return new PacketWriter(CUserPool.PlayerLeaveField(mID));
        }

        public List<IMovePath> ParseMovementPath(PacketReader packet)
        {
            var Fragments = new List<IMovePath>();
            //int portalCount = packet.ReadByte();
            //int crc = packet.ReadInt();
            //int tickcount = packet.ReadInt();
            //mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
            //packet.ReadBytes(4);

            int fragments = packet.ReadByte();
            for (int i = 0; i < fragments; ++i)
            {
                int fragmentType = packet.ReadByte();
                int x = 0, y = 0, xWobble = 0, yWobble = 0, unk = 0, fh = 0, xOffset = 0, yOffset = 0, newstate = 0, duration = 0;
                switch (fragmentType)
                {
                    case 0:
                        xOffset = packet.ReadShort();
                        yOffset = packet.ReadShort();
                        x = packet.ReadShort();
                        y = packet.ReadShort();
                        xWobble = packet.ReadShort();
                        yWobble = packet.ReadShort();
                        unk = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        Fragments.Add(new AbsoluteLifeMovePath(fragmentType, new Point(x, y), duration, newstate, unk, fh, new Point(xWobble, yWobble), new Point(xOffset, yOffset)));
                        break;
                    /*case 1:
                        x = packet.ReadShort();                                   //[9/1/2012 3:23:42 PM][대타] 받은 패킷 49 00 
                        y = packet.ReadShort();                                   //                                     01 
                        newstate = packet.ReadByte();                             //                                     AC 7E BC BC 
                        duration = packet.ReadShort();                            //                                     3A 7D 89 03 
                        //RelativeLifeMovement                                    //                                     BA 02 
                        break;                                                    //                                     E3 00 
                    case 3:                                                       //                                     00 00 00 00 
                        x = packet.ReadShort();                                   //                                     03
                        y = packet.ReadShort();                                   //                                     00 BA 02 0F 01 00 00 A4 01 00 00 00 00 00 00 06 D2 00
                        fh = packet.ReadShort();                                  //                                     00 BA 02 12 01 00 00 00 00 00 00 00 00 00 00 06 06 00
                        newstate = packet.ReadByte();                             //                                     00 BA 02 12 01 00 00 00 00 61 00 00 00 00 00 04 26 01
                        duration = packet.ReadShort();                            //                                     11 00 00 00 00 00 00 00 00 00 BA 02 E3 00 BA 02 12 01
                        //TeleportMovement                                        //                                     11 
                        break;                                                    //                                     00 00 00 00 00 00 00 00 00 
                    case 20:                                                      //                                     BA 02 
                        x = packet.ReadShort();                                   //                                     E3 00 
                        y = packet.ReadShort();                                   //                                     BA 02 
                        xOffset = packet.ReadShort();                             //                                     12 01
                        yOffset = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        //KnockbackMovement
                        break;
                    case 11:
                        //EquipChangeMovement(readbyte)
                        break;*/
                    default:
                        Logger.Write(Logger.LogTypes.경고, "NEW MOVEMENT {0} : {1}", fragmentType, /*packet.ToArray().ToString2s()*/"ㅋㅋ");
                        //break;
                        return Fragments;
                }
            }
            int read = packet.ReadByte();
            byte buffer = 0;
            byte lol = 0;
            List<byte> derp = new List<byte>();
            if (read > 0)
                for (int i = 0; i < read; ++i)
                {
                    if (i % 2 != 0)
                        buffer >>= 4;
                    else
                        buffer = packet.ReadByte();
                    lol = (byte)(buffer & (byte)0x0F);
                    derp.Add(lol);
                }

            packet.ReadByte(); // x old
            packet.ReadByte(); // y old
            packet.ReadByte(); // x new
            packet.ReadByte(); // y new

            return Fragments;
        }

        public PacketWriter GenerateMovementPath()
        {
            return new PacketWriter();
        }
    }

    public class PrimaryStats
    {
        public byte Level { get; set; }
        public short Job { get; set; }
        public short Str { get; set; }
        public short Dex { get; set; }
        public short Int { get; set; }
        public short Luk { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public short AP { get; set; }
        public short[] SP { get; set; }
        public int EXP { get; set; }
        public int Fame { get; set; }
        public int DemonSlayerAccessory { get; set; }
        public int Fatigue { get; set; }
        public byte BattleRank { get; set; }
        public int BattlePoints { get; set; }
        public int BattleEXP { get; set; }
    }

    public class Traits
    {
        public int Ambition { get; set; }
        public int Insight { get; set; }
        public int Willpower{ get; set; }
        public int Diligence { get; set; }
        public int Empathy { get; set; }
        public int Charm { get; set; }
        public short AmbitionGained { get; set; }
        public short InsightGained { get; set; }
        public short WillpowerGained { get; set; }
        public short DiligenceGained { get; set; }
        public short EmpathyGained { get; set; }
        public short CharmGained { get; set; }
    }
}
