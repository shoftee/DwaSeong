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
using System.Drawing;
using WvsGame.Field;
using WvsGame.Field.Entity;
using WvsGame.User;
using reNX;
using reNX.NXProperties;

namespace WvsGame.WZ
{
    using CS = Skill;
    using CField = Field.Field;

    public class NXDataProvider
    {
        private static readonly NXFile Character = new NXFile(@"C:\nx\Character.nx");
        private static readonly NXFile Item = new NXFile(@"C:\nx\Item.nx");
        private static readonly NXFile Map = new NXFile(@"C:\nx\Map.nx");
        private static readonly NXFile Skill = new NXFile(@"C:\nx\Skill.nx");

        #region Character

        public static int CacheCharacterData()
        {
            int ret = 0;
            foreach (NXNode basenode in Character.BaseNode)
            {
                if (basenode.Name.StartsWith("0") || basenode.Name == "Hair" || basenode.Name == "Face" ||
                    basenode.Name == "Afterimage")
                    continue;
                foreach (NXNode imgnode in basenode)
                {
                    int itemid = int.Parse(imgnode.Name.Replace(".img", ""));
                    if (NXDataCache.Equip.ContainsKey(itemid))
                        continue;
                    var equip = new Equip(itemid, "WZData");
                    NXNode info = imgnode.GetChild("info");
                    equip.job = GetIntFromChild(info, "reqJob");
                    equip.level = GetIntFromChild(info, "reqLevel");
                    equip.reqStr = GetIntFromChild(info, "reqSTR");
                    equip.reqDex = GetIntFromChild(info, "reqDEX");
                    equip.reqInt = GetIntFromChild(info, "reqINT");
                    equip.reqLuk = GetIntFromChild(info, "reqLUK");
                    equip.incStr = GetIntFromChild(info, "incSTR");
                    equip.incDex = GetIntFromChild(info, "incDEX");
                    equip.incInt = GetIntFromChild(info, "incINT");
                    equip.incLuk = GetIntFromChild(info, "incLUK");
                    equip.incWatk = GetIntFromChild(info, "incPAD");
                    equip.incMatk = GetIntFromChild(info, "incMAD");
                    equip.incWdef = GetIntFromChild(info, "incPDD");
                    equip.incMdef = GetIntFromChild(info, "incMDD");
                    equip.incMaxHP = GetIntFromChild(info, "incMHP");
                    equip.incMaxMP = GetIntFromChild(info, "incMMP");
                    equip.incAcc = GetIntFromChild(info, "incACC");
                    equip.incAvoid = GetIntFromChild(info, "incEVA");
                    equip.incSpeed = GetIntFromChild(info, "incSpeed");
                    equip.incJump = GetIntFromChild(info, "incJump");
                    equip.upgrades = GetIntFromChild(info, "tuc");
                    equip.price = GetIntFromChild(info, "price");
                    equip.attackSpeed = GetIntFromChild(info, "attackSpeed");
                    equip.cash = GetIntFromChild(info, "cash");
                    equip.knockback = GetIntFromChild(info, "knockback");
                    equip.equipTradeBlock = GetIntFromChild(info, "equipTradeBlock");
                    equip.setItemID = GetIntFromChild(info, "setItemID");
                    equip.only = GetIntFromChild(info, "only");
                    equip.notSale = GetIntFromChild(info, "notSale");
                    equip.specialID = GetIntFromChild(info, "specialID");
                    equip.slotMax = GetIntFromChild(info, "slotMax");
                    NXDataCache.Equip.Add(itemid, equip);
                    ++ret;
                }
            }
            return ret;
        }

        #endregion

        #region Item

        public static int CacheItemData()
        {
            int ret = 0;
            foreach (NXNode basenode in Item.BaseNode)
            {
                if (basenode.Name == "ItemOption.img" || basenode.Name == "Special")
                    continue;
                foreach (NXNode imgnode in basenode)
                {
                    if (basenode.Name == "Pet")
                    {
                        int itemid = int.Parse(imgnode.Name.Replace(".img", ""));
                        if (NXDataCache.Item.ContainsKey(itemid))
                            continue;
                        var item = new Item(itemid, "WZData");
                        NXNode info = imgnode.GetChild("info");
                        item.price = GetIntFromChild(info, "price");
                        item.cash = GetIntFromChild(info, "cash");
                        item.only = GetIntFromChild(info, "only");
                        item.notSale = GetIntFromChild(info, "notSale");
                        item.slotMax = GetIntFromChild(info, "slotMax");
                        NXDataCache.Item.Add(itemid, item);
                        ++ret;
                    }
                    else
                    {
                        foreach (NXNode subnode in imgnode)
                        {
                            int itemid = int.Parse(subnode.Name);
                            if (NXDataCache.Item.ContainsKey(itemid))
                                continue;
                            var item = new Item(itemid, "WZData");
                            NXNode info = subnode.GetChild("info");
                            item.price = GetIntFromChild(info, "price");
                            item.cash = GetIntFromChild(info, "cash");
                            item.only = GetIntFromChild(info, "only");
                            item.notSale = GetIntFromChild(info, "notSale");
                            item.slotMax = GetIntFromChild(info, "slotMax");
                            NXDataCache.Item.Add(itemid, item);
                            ++ret;
                        }
                    }
                }
            }
            return ret;
        }

        #endregion

        #region Map

        public static int CacheFieldData()
        {
            int ret = 0;
            foreach (NXNode mapnode in Map.BaseNode.GetChild("Map"))
            {
                if (!mapnode.Name.StartsWith("Map"))
                    continue;
                foreach (NXNode mapimg in mapnode)
                {
                    CField field = new CField();
                    field.FieldID = int.Parse(mapimg.Name.Replace(".img", ""));
                    NXNode node = mapimg.GetChild("info");
                    foreach (NXNode childnode in node)
                        switch (childnode.Name)
                        {
                            case "bgm":
                                field.WzData.bgm = GetStringFromChild(node, childnode.Name);
                                break;
                            case "cloud":
                                field.WzData.cloud = GetIntFromChild(node, childnode.Name);
                                break;
                            case "swim":
                                field.WzData.swim = GetIntFromChild(node, childnode.Name);
                                break;
                            case "forcedReturn":
                                field.WzData.forcedReturn = GetIntFromChild(node, childnode.Name);
                                break;
                            case "hideMinimap":
                                field.WzData.hideMinimap = GetIntFromChild(node, childnode.Name);
                                break;
                            case "mapDesc":
                                field.WzData.mapDesc = GetStringFromChild(node, childnode.Name);
                                break;
                            case "mapMark":
                                field.WzData.MapMark = GetStringFromChild(node, childnode.Name);
                                break;
                            case "mobRate":
                                field.WzData.mobRate = GetFloatFromChild(node, childnode.Name);
                                break;
                            case "moveLimit":
                                field.WzData.moveLimit = GetIntFromChild(node, childnode.Name);
                                break;
                            case "returnMap":
                                field.WzData.returnMap = GetIntFromChild(node, childnode.Name);
                                break;
                            case "town":
                                field.WzData.town = GetIntFromChild(node, childnode.Name);
                                break;
                            case "fieldLimit":
                                int fl = GetIntFromChild(node, childnode.Name);
                                if (fl >= (int) Math.Pow(2, 23))
                                    fl = fl & ((int) Math.Pow(2, 23) - 1);
                                field.WzData.fieldLimit = (FieldLimit) fl;
                                break;
                            case "VRTop":
                            case "VRBottom":
                            case "VRLeft":
                            case "VRRight":
                                break;
                            case "timeLimit":
                                field.WzData.timeLimit = GetIntFromChild(node, childnode.Name);
                                break;
                            case "lvLimit":
                                field.WzData.lvLimit = GetIntFromChild(node, childnode.Name);
                                break;
                            case "onFirstUserEnter":
                                field.WzData.onFirstUserEnter = GetStringFromChild(node, childnode.Name);
                                break;
                            case "onUserEnter":
                                field.WzData.onUserEnter = GetStringFromChild(node, childnode.Name);
                                break;
                            case "fly":
                                field.WzData.fly = GetIntFromChild(node, childnode.Name);
                                break;
                            case "noMapCmd":
                                field.WzData.noMapCmd = GetIntFromChild(node, childnode.Name);
                                break;
                            case "partyOnly":
                                field.WzData.partyOnly = GetIntFromChild(node, childnode.Name);
                                break;
                            case "fieldType":
                                int ft = GetIntFromChild(node, childnode.Name);
                                if (!Enum.IsDefined(typeof (FieldType), ft))
                                    ft = 0;
                                field.WzData.fieldType = (FieldType) ft;
                                break;
                            case "miniMapOnOff":
                                field.WzData.miniMapOnOff = GetIntFromChild(node, childnode.Name);
                                break;
                            case "reactorShuffle":
                                field.WzData.reactorShuffle = GetIntFromChild(node, childnode.Name);
                                break;
                            case "reactorShuffleName":
                                field.WzData.reactorShuffleName = GetStringFromChild(node, childnode.Name);
                                break;
                            case "personalShop":
                                field.WzData.personalShop = GetIntFromChild(node, childnode.Name);
                                break;
                            case "entrustedShop":
                                field.WzData.entrustedShop = GetIntFromChild(node, childnode.Name);
                                break;
                            case "effect":
                                field.WzData.effect = GetStringFromChild(node, childnode.Name);
                                break;
                            case "lvForceMove":
                                field.WzData.lvForceMove = GetIntFromChild(node, childnode.Name);
                                break;
                            case "timeMob":
                                field.WzData.startHour = GetIntFromChild(node, "startHour");
                                field.WzData.endHour = GetIntFromChild(node, "endHour");
                                int? id = GetIntFromChild(node, "id");
                                string message = GetStringFromChild(node, "message");
                                if (id == null || message == null ||
                                    (field.WzData.startHour == null ^ field.WzData.endHour == null))
                                    break;
                                else
                                    field.WzData.timeMob = new FieldWzData.TimeMob(field.WzData.startHour,
                                                                                   field.WzData.endHour, (int) id,
                                                                                   message);
                                break;
                            case "help":
                                field.WzData.help = GetStringFromChild(node, childnode.Name);
                                break;
                            case "snow":
                                field.WzData.snow = GetIntFromChild(node, childnode.Name);
                                break;
                            case "rain":
                                field.WzData.rain = GetIntFromChild(node, childnode.Name);
                                break;
                            case "dropExpire":
                                field.WzData.dropExpire = GetIntFromChild(node, childnode.Name);
                                break;
                            case "decHP":
                                field.WzData.decHP = GetIntFromChild(node, childnode.Name);
                                break;
                            case "decInterval":
                                field.WzData.decInterval = GetIntFromChild(node, childnode.Name);
                                break;
                            case "autoLieDetector":
                                field.WzData.startHour = GetIntFromChild(node, "startHour");
                                field.WzData.endHour = GetIntFromChild(node, "endHour");
                                int? interval = GetIntFromChild(node, "interval");
                                int? propInt = GetIntFromChild(node, "prop");
                                if (field.WzData.startHour == null || field.WzData.endHour == null || interval == null ||
                                    propInt == null)
                                {
                                    break;
                                }
                                else
                                    field.WzData.autoLieDetector =
                                        new FieldWzData.AutoLieDetector((int) field.WzData.startHour,
                                                                        (int) field.WzData.endHour, (int) interval,
                                                                        (int) propInt);
                                break;
                            case "expeditionOnly":
                                field.WzData.expeditionOnly = GetIntFromChild(node, childnode.Name);
                                break;
                            case "fs":
                                field.WzData.fs = GetFloatFromChild(node, childnode.Name);
                                break;
                            case "protectItem":
                                field.WzData.protectItem = GetIntFromChild(node, childnode.Name);
                                break;
                            case "createMobInterval":
                                field.WzData.createMobInterval = GetIntFromChild(node, childnode.Name);
                                break;
                            case "fixedMobCapacity":
                                field.WzData.fixedMobCapacity = GetIntFromChild(node, childnode.Name);
                                break;
                            case "streetName":
                                field.WzData.streetName = GetStringFromChild(node, childnode.Name);
                                break;
                            case "mapName":
                                field.WzData.mapName = GetStringFromChild(node, childnode.Name);
                                break;
                            case "noRegenMap":
                                field.WzData.noRegenMap = GetIntFromChild(node, childnode.Name);
                                break;
                            case "recovery":
                                field.WzData.recovery = GetFloatFromChild(node, childnode.Name);
                                break;
                            case "blockPBossChange":
                                field.WzData.blockPBossChange = GetIntFromChild(node, childnode.Name);
                                break;
                            case "everlast":
                                field.WzData.everlast = GetIntFromChild(node, childnode.Name);
                                break;
                            case "damageCheckFree":
                                field.WzData.damageCheckFree = GetIntFromChild(node, childnode.Name);
                                break;
                            case "dropRate":
                                field.WzData.dropRate = GetFloatFromChild(node, childnode.Name);
                                break;
                            case "scrollDisable":
                                field.WzData.scrollDisable = GetIntFromChild(node, childnode.Name);
                                break;
                            case "needSkillForFly":
                                field.WzData.needSkillForFly = GetIntFromChild(node, childnode.Name);
                                break;
                            case "zakum2Hack":
                                field.WzData.zakum2Hack = GetIntFromChild(node, childnode.Name);
                                break;
                            case "allMoveCheck":
                                field.WzData.allMoveCheck = GetIntFromChild(node, childnode.Name);
                                break;
                            case "VRLimit":
                                field.WzData.VRLimit = GetIntFromChild(node, childnode.Name);
                                break;
                            case "consumeItemCoolTime":
                                field.WzData.consumeItemCoolTime = GetIntFromChild(node, childnode.Name);
                                break;
                        }
                    if (mapimg.ContainsChild("life"))
                    {
                        node = mapimg.GetChild("life");
                        foreach (NXNode childnode in node)
                        {
                            string type = GetStringFromChild(childnode, "type");
                            if (type == "m")
                            {
                                var monster = new Monster();
                                monster.mID = GetIntFromChild(childnode, "id");
                                monster.mX = GetIntFromChild(childnode, "x");
                                monster.mY = GetIntFromChild(childnode, "y");
                                monster.mMobTime = GetIntFromChild(childnode, "mobTime");
                                monster.mF = GetIntFromChild(childnode, "f");
                                monster.mHide = GetIntFromChild(childnode, "hide");
                                monster.mFh = GetIntFromChild(childnode, "fh");
                                monster.mCy = GetIntFromChild(childnode, "cy");
                                monster.mRx0 = GetIntFromChild(childnode, "rx0");
                                monster.mRx1 = GetIntFromChild(childnode, "rx1");
                                field.AddFieldObject(monster);
                            }
                            else if (type == "n")
                            {
                                var npc = new Npc();
                                npc.mID = GetIntFromChild(childnode, "id");
                                npc.mX = GetIntFromChild(childnode, "x");
                                npc.mY = GetIntFromChild(childnode, "y");
                                npc.mF = GetIntFromChild(childnode, "f");
                                npc.mFh = GetIntFromChild(childnode, "fh");
                                npc.mCy = GetIntFromChild(childnode, "cy");
                                npc.mRx0 = GetIntFromChild(childnode, "rx0");
                                npc.mRx1 = GetIntFromChild(childnode, "rx1");
                                field.AddFieldObject(npc);
                            }
                        }
                    }
                    /*if (mapimg.ContainsChild("reactor"))
                    {
                        node = mapimg.GetChild("reactor");
                        foreach (NXNode childnode in node)
                            switch (childnode.Name)
                            {
                            }
                    }
                    if (mapimg.ContainsChild("foothold"))
                    {
                        node = mapimg.GetChild("foothold");
                        foreach (NXNode childnode in node)
                            switch (childnode.Name)
                            {
                            }
                    }
                    if (mapimg.ContainsChild("ladderRope"))
                    {
                        node = mapimg.GetChild("ladderRope");
                        foreach (NXNode childnode in node)
                            switch (childnode.Name)
                            {
                            }
                    }*/
                    if (mapimg.ContainsChild("foothold"))
                    {
                        node = mapimg.GetChild("foothold");
                        field.mFootholdTree.mField = field;
                        foreach (NXNode layernode in node)
                        {
                            foreach (NXNode groupnode in node)
                            {
                                foreach (NXNode footholdnode in node)
                                {
                                    Foothold foothold = new Foothold();
                                    foothold.mFootholdTree = field.mFootholdTree;
                                    foothold.mLayerID = int.Parse(layernode.Name);
                                    foothold.mGroupID = int.Parse(groupnode.Name);
                                    foothold.mID = int.Parse(footholdnode.Name);
                                    foothold.mStart = new Point(GetIntFromChild(footholdnode, "x1"), GetIntFromChild(footholdnode, "y1"));
                                    foothold.mEnd = new Point(GetIntFromChild(footholdnode, "x2"), GetIntFromChild(footholdnode, "y2"));
                                    foothold.mPrevious = GetIntFromChild(footholdnode, "prev");
                                    foothold.mNext = GetIntFromChild(footholdnode, "next");
                                    foothold.mForce = GetIntFromChild(footholdnode, "force");
                                    foothold.mForbidFallDown = GetIntFromChild(footholdnode, "forbidFallDown");
                                    foothold.mPiece = GetIntFromChild(footholdnode, "piece");
                                    field.mFootholdTree.mFootholds.Add(foothold);
                                }
                            }
                        }
                    }
                    if (mapimg.ContainsChild("seat"))
                    {
                        node = mapimg.GetChild("seat");
                        foreach (NXNode childnode in node)
                        {
                            Seat seat = new Seat();
                            int id = int.Parse(childnode.Name);
                            seat.mID = id;
                            seat.mPosition = GetPointFromChild(node, childnode.Name);
                            field.AddFieldObject(seat);
                        }
                    }
                    if (mapimg.ContainsChild("portal"))
                    {
                        node = mapimg.GetChild("portal");
                        foreach (NXNode childnode in node)
                        {
                            Portal portal = new Portal();
                            portal.mPortalID = int.Parse(childnode.Name);
                            portal.mName = GetStringFromChild(childnode, "pn");
                            portal.mPosition = new Point(GetIntFromChild(childnode, "x"), GetIntFromChild(childnode, "y"));
                            portal.mDestinationMapID = GetIntFromChild(childnode, "tm");
                            portal.mDestinationName = GetStringFromChild(childnode, "tn");
                            portal.mOnce = GetIntFromChild(childnode, "onlyOnce");

                            switch (portal.mName)
                            {
                                case "sp":
                                    portal.mType = PortalType.Startpoint;
                                    break;
                                case "pi":
                                    portal.mType = PortalType.Invisible;
                                    break;
                                case "pv":
                                    portal.mType = PortalType.Visible;
                                    break;
                                case "pc":
                                    portal.mType = PortalType.Collision;
                                    break;
                                case "pg":
                                    portal.mType = PortalType.Changable;
                                    break;
                                case "pgi":
                                    portal.mType = PortalType.ChangableInvisible;
                                    break;
                                case "tp":
                                    portal.mType = PortalType.TownportalPoint;
                                    break;
                                case "ps":
                                    portal.mType = PortalType.Script;
                                    break;
                                case "psi":
                                    portal.mType = PortalType.ScriptInvisible;
                                    break;
                                case "pcs":
                                    portal.mType = PortalType.CollisionScript;
                                    break;
                                case "ph":
                                    portal.mType = PortalType.Hidden;
                                    break;
                                case "psh":
                                    portal.mType = PortalType.ScriptHidden;
                                    break;
                                case "pcj":
                                    portal.mType = PortalType.CollisionVerticalJump;
                                    break;
                                case "pci":
                                    portal.mType = PortalType.CollisionCustomImpact;
                                    break;
                                case "pcig":
                                    portal.mType = PortalType.CollisionUnknownPcig;
                                    break;
                            }
                            field.AddFieldObject(portal);
                        }
                    }
                    NXDataCache.Field.Add(field.FieldID, field);
                    ret++;
                }
            }
            return ret;
        }

        #endregion

        #region String

        #endregion

        #region Skill

        public static int CacheSkillData()
        {
            int ret = 0;
            foreach (NXNode dirnode in Skill.BaseNode)
            {
                int oi = 0;
                if (!int.TryParse(dirnode.Name[0].ToString(), out oi))
                    continue;
                NXNode skills = dirnode["skill"];
                foreach (NXNode skillnode in skills)
                {
                    var skill = new Skill(int.Parse(skillnode.Name));
                    if (skillnode.ContainsChild("level"))
                        foreach (NXNode levelnode in skillnode["level"])
                        {
                            var level = new Skill.Level();
                            level.SkillLevel = int.Parse(levelnode.Name);
                            level.Time = GetIntFromChild(levelnode, "time");
                            level.Speed = GetIntFromChild(levelnode, "speed");
                            level.Jump = GetIntFromChild(levelnode, "jump");
                            level.BoundsLT = GetPointFromChild(levelnode, "lt");
                            level.BoundsRB = GetPointFromChild(levelnode, "rb");
                            level.Range = GetIntFromChild(levelnode, "range");
                            level.MobCount = GetIntFromChild(levelnode, "mobCount");
                            level.Damage = GetIntFromChild(levelnode, "damage");
                            level.PhysicalDamage = GetIntFromChild(levelnode, "pad");
                            level.PhysicalDefense = GetIntFromChild(levelnode, "pdd");
                            level.MagicalDamage = GetIntFromChild(levelnode, "mad");
                            level.MagicalDefense = GetIntFromChild(levelnode, "mdd");
                            skill.Levels.Add(level);
                        }
                    if (skillnode.ContainsChild("req"))
                        foreach (NXNode reqnode in skillnode["req"])
                            skill.Req.Add(int.Parse(reqnode.Name), GetIntFromChild(skillnode["req"], reqnode.Name));
                    skill.MasterLevel = GetIntFromChild(skillnode, "masterLevel");
                    if (skillnode.ContainsChild("finalAttack"))
                        if (skillnode["finalAttack"].ChildCount > 0)
                            foreach (NXNode n in skillnode["finalAttack"])
                                skill.FinalAttack = int.Parse(n.Name);
                        else
                            skill.FinalAttack = GetIntFromChild(skillnode, "finalAttack");
                    skill.CombatOrders = GetIntFromChild(skillnode, "combatOrders");

                    if (skillnode.ContainsChild("info"))
                    {
                        skill.RapidAttack = GetIntFromChild(skillnode["info"], "rapidAttack");
                        skill.MagicSteal = GetIntFromChild(skillnode["info"], "MagicSteal");
                        skill.MassSpell = GetIntFromChild(skillnode["info"], "MassSpell");
                    }
                    if (skillnode.ContainsChild("common"))
                    {
                        skill.Cooltime = GetStringFromChild(skillnode["common"], "cooltime");
                        skill.MpCon = GetStringFromChild(skillnode["common"], "MpCon");
                        skill.Time = GetStringFromChild(skillnode["common"], "Time");
                        skill.Mastery = GetStringFromChild(skillnode["common"], "Mastery");
                        skill.X = GetStringFromChild(skillnode["common"], "X");
                        skill.Y = GetStringFromChild(skillnode["common"], "Y");
                        skill.CriticaldamageMin = GetStringFromChild(skillnode["common"], "criticaldamageMin");
                    }
                    NXDataCache.Skill.Add(skill.SkillID, skill);
                }
            }
            return ret;
        }

        #endregion

        private static int GetIntFromChild(NXNode node, string get)
        {
            if (!node.ContainsChild(get))
                return 0;
            try
            {
                return node.GetChild(get).ValueOrDie<int>();
            }
            catch (InvalidCastException) // see Character.wz/Weapon/01482146.img/info/incSTR for more details T_T
            {
                return int.Parse(node.GetChild(get).ValueOrDie<string>());
            }
        }

        private static float GetFloatFromChild(NXNode node, string get)
        {
            if (!node.ContainsChild(get))
                return 0;
            return (float) node.GetChild(get).ValueOrDie<double>();
        }

        private static Point GetPointFromChild(NXNode node, string get)
        {
            if (!node.ContainsChild(get))
                return new Point();
            return node.GetChild(get).ValueOrDie<Point>();
        }

        private static string GetStringFromChild(NXNode node, string get)
        {
            if (!node.ContainsChild(get))
                return "";
            return node.GetChild(get).ValueOrDie<string>();
        }
    }
}