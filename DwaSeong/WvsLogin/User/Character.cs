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
using Common;
using WvsLogin.WZ;

namespace WvsLogin.User
{
    public class Character
    {
        public int mID { get; set; }
        public string mName { get; set; }
        public byte mGender { get; set; }
        public byte mSkin { get; set; }
        public int mHair { get; set; }
        public int mFace { get; set; }

        public int mMap { get; set; }
        public byte mMapPosition { get; set; }

        public int mMeso { get; set; }

        public int mWorldPos { get; set; }
        public int mWorldOldPos { get; set; }
        public int mJobPos { get; set; }
        public int mJobOldPos { get; set; }

        public Dictionary<sbyte, AbstractItem>[] mInventory { get; set; }

        public PrimaryStats mPrimaryStats { get; set; }
        public Traits mTraits { get; set; }

        public Client mClient { get; set; }

        public Character()
        {
            mPrimaryStats = new PrimaryStats();
            mTraits = new Traits();
            mInventory = new Dictionary<sbyte, AbstractItem>[6];
            for (int i = 0; i < 6; i++)
                mInventory[i] = new Dictionary<sbyte, AbstractItem>();
            mPrimaryStats.SP = new short[4];
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
