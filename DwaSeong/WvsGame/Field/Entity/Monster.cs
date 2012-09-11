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
using MapleLib.PacketLib;

namespace WvsGame.Field.Entity
{
    public class Monster : FieldObject, IMovable, ISpawnable
    {
        public int mID { get; set; }
        public int mX { get; set; }
        public int mY { get; set; }
        public int mMobTime { get; set; }
        public int mF { get; set; }
        public int mHide { get; set; }
        public int mFh { get; set; }
        public int mCy { get; set; }
        public int mRx0 { get; set; }
        public int mRx1 { get; set; }

        public PacketWriter GenerateMovementPath()
        {
            return new PacketWriter();
        }

        //public List<IMovePath> ParseMovementPath(PacketReader packet)
        //{
        //    return new List<IMovePath>();
        //}

        public PacketWriter GenerateSpawnPacket()
        {
            return new PacketWriter();
        }

        public PacketWriter GenerateDespawnPacket()
        {
            return new PacketWriter();
        }
    }
}
