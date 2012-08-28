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
using WvsGame.User;

namespace WvsGame.WZ
{
    using CField = Field.Field;
    public class NXDataCache
    {
        public static Dictionary<int, Equip> Equip = new Dictionary<int, Equip>();
        public static Dictionary<int, Item> Item = new Dictionary<int, Item>();
        public static Dictionary<int, CField> Field = new Dictionary<int, CField>();
        public static Dictionary<int, Skill> Skill = new Dictionary<int, Skill>();
    }

    //public static class ObjectClone
    //{
    //    public static T Clone<T>(T RealObject)
    //    {
    //        using (Stream objectStream = new MemoryStream())
    //        {
    //            IFormatter formatter = new BinaryFormatter();
    //            formatter.Serialize(objectStream, RealObject);
    //            objectStream.Seek(0, SeekOrigin.Begin);
    //            return (T)formatter.Deserialize(objectStream);
    //        }
    //    }
    //}
}
