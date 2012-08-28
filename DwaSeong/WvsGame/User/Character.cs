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
            int portalCount = packet.ReadByte();
            int crc = packet.ReadInt();
            int tickcount = packet.ReadInt();
            mPosition = new System.Drawing.Point(packet.ReadShort(), packet.ReadShort());
            byte[] movement =  packet.ReadBytes(packet.Length - packet.Position);
            packet.ReadBytes(4);



            return Fragments;
        }
        /*
        public List<IMovePath> ParseMovementPath(PacketReader packet)
        {
            int fragments = packet.ReadByte();
            var Fragments = new List<IMovePath>();
            for (int i = 0; i < fragments; ++i)
            {
                int fragmentType = packet.ReadByte();
                int x = 0, y = 0, xWobble = 0, yWobble = 0, unk = 0, fh = 0, xOffset = 0, yOffset = 0, newstate = 0, duration = 0;
                switch (fragmentType)
                {
                    case 0:
                    case 7:
                    case 14:
                        fh = packet.ReadShort();
                        goto case 46;
                    case 16:
                    case 44:
                        xOffset = packet.ReadShort();
                        yOffset = packet.ReadShort();
                        goto case 46;
                    case 45:
                    case 46:
                        x = packet.ReadShort();
                        y = packet.ReadShort();
                        xWobble = packet.ReadShort();
                        yWobble = packet.ReadShort();
                        unk = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        Fragments.Add(new AbsoluteLifeMovePath(fragmentType, new Point(x, y), duration, newstate, unk, fh, new Point(xWobble, yWobble), new Point(xOffset, yOffset)));
                        break;
                    case 1:
                    case 2:
                    case 15:
                    case 18:
                        unk = packet.ReadShort();
                        goto case 43;
                    case 19:
                        unk = packet.ReadShort();
                        goto case 43;
                    case 21:
                    case 40:
                    case 41:
                    case 42:
                    case 43:
                        x = packet.ReadShort();
                        y = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        //RelativeLifeMovement
                        break;
                    case 17:
                    case 22:
                    case 23:
                    case 25:
                    case 26:
                    case 27:
                    case 28:
                    case 29:
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                    case 34:
                    case 35:
                    case 36:
                    case 37:
                    case 38:
                    case 39:
                        newstate = packet.ReadByte();
                        unk = packet.ReadShort();
                        //GroundMovement
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 8:
                    case 9:
                    case 10:
                    case 12:
                    case 13:
                        x = packet.ReadShort();
                        y = packet.ReadShort();
                        fh = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        //TeleportMovement
                        break;
                    case 20:
                        x = packet.ReadShort();
                        y = packet.ReadShort();
                        xOffset = packet.ReadShort();
                        yOffset = packet.ReadShort();
                        newstate = packet.ReadByte();
                        duration = packet.ReadShort();
                        //KnockbackMovement
                        break;
                    case 11:
                        //EquipChangeMovement(readbyte)
                        break;
                    default:
                        Logger.Write(Logger.LogTypes.경고, "NEW MOVEMENT {0} : {1}", fragmentType, /*packet.ToArray().ToString2s()"ㅋㅋ");
                        break;
                }
            }
            return Fragments;
        }
*/
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
