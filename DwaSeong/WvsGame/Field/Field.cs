using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Common;
using WvsGame.Field.Entity;
using WvsGame.User;

namespace WvsGame.Field
{
    public class Field : ICopyable<Field>
    {
        public int FieldID { get; set; }
        public int FieldInstance { get; set; }
        public List<FieldObject> mObjects { get; set; }
        public FieldWzData WzData { get; set; }
        public FootholdTree mFootholdTree { get; set; }

        private int FieldObjectCounter { get; set; }

        public Field()
        {
            mObjects = new List<FieldObject>();
            WzData = new FieldWzData();
            mFootholdTree = new FootholdTree();
        }

        public void SendPacket(byte[] packet)
        {
            foreach (
                Client c in
                    Program.mServer.Clients.Where(
                        c => c.mCharacter.mField == FieldID && c.mCharacter.mFieldInstance == FieldInstance))
                c.SendPacket(packet);
        }

        public void SendPacket(Client ignore, byte[] packet)
        {
            foreach (
                Client c in
                    Program.mServer.Clients.Where(
                        c => c.mCharacter.mField == FieldID && c.mCharacter.mFieldInstance == FieldInstance && c != ignore))
                c.SendPacket(packet);
        }


        public void AddFieldObject(FieldObject obj)
        {
            obj.mMap = this;
            obj.mObjectID = FieldObjectCounter;
            mObjects.Add(obj);
            FieldObjectCounter++;
        }

        public void AddCharacter(Character chr)
        {
            SendPacket(chr.GenerateSpawnPacket().ToArray());
            foreach (var c in Program.mServer.Clients.Where(c => c.mCharacter.mField == FieldID && c.mCharacter.mFieldInstance == FieldInstance))
                chr.mClient.SendPacket(c.mCharacter.GenerateSpawnPacket().ToArray());
            foreach (var obj in mObjects.OfType<ISpawnable>())
            {
                if (obj is Npc)
                    if (((Npc) obj).mID == 9010041 || ((Npc) obj).mID == 9010040)
                        continue;
                chr.mClient.SendPacket((obj).GenerateSpawnPacket().ToArray());
                //Console.WriteLine(obj.ToString());
            }
        }

        public void RemoveCharacter(Character chr)
        {
            SendPacket(chr.GenerateDespawnPacket().ToArray());
            foreach (Client c in Program.mServer.Clients)
                if (c.mCharacter.mField == FieldID && c.mCharacter.mFieldInstance == FieldInstance)
                    chr.mClient.SendPacket(c.mCharacter.GenerateDespawnPacket().ToArray());
        }

        public Portal GetPortal(int id)
        {
            foreach (Portal p in mObjects.OfType<Portal>())
                if (p.mPortalID == id)
                    return p;
            return null;
        }
        
        public Field Copy()
        {
            Field ret = new Field();
            ret.FieldID = FieldID;
            ret.FieldInstance = FieldInstance;
            ret.mObjects = mObjects;
            ret.mFootholdTree = mFootholdTree;
            ret.WzData = new FieldWzData();
            ret.WzData.bgm = WzData.bgm;
            ret.WzData.MapMark = WzData.MapMark;
            ret.WzData.fieldLimit = WzData.fieldLimit;
            ret.WzData.returnMap = WzData.returnMap;
            ret.WzData.forcedReturn = WzData.forcedReturn;
            ret.WzData.cloud = WzData.cloud;
            ret.WzData.swim = WzData.swim;
            ret.WzData.hideMinimap = WzData.hideMinimap;
            ret.WzData.town = WzData.town;
            ret.WzData.mobRate = WzData.mobRate;

            ret.WzData.timeLimit = WzData.timeLimit;
            ret.WzData.lvLimit = WzData.lvLimit;
            ret.WzData.fieldType = WzData.fieldType;
            ret.WzData.onFirstUserEnter = WzData.onFirstUserEnter;
            ret.WzData.onUserEnter = WzData.onUserEnter;
            ret.WzData.fly = WzData.fly;
            ret.WzData.noMapCmd = WzData.noMapCmd;
            ret.WzData.partyOnly = WzData.partyOnly;
            ret.WzData.reactorShuffle = WzData.reactorShuffle;
            ret.WzData.reactorShuffleName = WzData.reactorShuffleName;
            ret.WzData.personalShop = WzData.personalShop;
            ret.WzData.entrustedShop = WzData.entrustedShop;
            ret.WzData.effect = WzData.effect;
            ret.WzData.lvForceMove = WzData.lvForceMove;
            ret.WzData.timeMob = WzData.timeMob;
            ret.WzData.help = WzData.help;
            ret.WzData.snow = WzData.snow;
            ret.WzData.rain = WzData.rain;
            ret.WzData.dropExpire = WzData.dropExpire;
            ret.WzData.decHP = WzData.decHP;
            ret.WzData.decInterval = WzData.decInterval;
            ret.WzData.autoLieDetector = WzData.autoLieDetector;
            ret.WzData.expeditionOnly = WzData.expeditionOnly;
            ret.WzData.fs = WzData.fs;
            ret.WzData.protectItem = WzData.protectItem;
            ret.WzData.createMobInterval = WzData.createMobInterval;
            ret.WzData.fixedMobCapacity = WzData.fixedMobCapacity;

            ret.WzData.startHour = WzData.startHour;
            ret.WzData.endHour = WzData.endHour;
            ret.WzData.moveLimit = WzData.moveLimit;
            ret.WzData.mapDesc = WzData.mapDesc;
            ret.WzData.mapName = WzData.mapName;
            ret.WzData.streetName = WzData.streetName;
            ret.WzData.miniMapOnOff = WzData.miniMapOnOff;
            ret.WzData.noRegenMap = WzData.noRegenMap;
            ret.WzData.recovery = WzData.recovery;
            ret.WzData.blockPBossChange = WzData.blockPBossChange;
            ret.WzData.everlast = WzData.everlast;
            ret.WzData.damageCheckFree = WzData.damageCheckFree;
            ret.WzData.dropRate = WzData.dropRate;
            ret.WzData.scrollDisable = WzData.scrollDisable;
            ret.WzData.needSkillForFly = WzData.needSkillForFly;
            ret.WzData.zakum2Hack = WzData.zakum2Hack;
            ret.WzData.allMoveCheck = WzData.allMoveCheck;
            ret.WzData.VRLimit = WzData.VRLimit;
            ret.WzData.consumeItemCoolTime = WzData.consumeItemCoolTime;

            ret.WzData.VR = WzData.VR;
            ret.WzData.strMapName = WzData.strMapName;
            ret.WzData.strStreetName = WzData.strStreetName;

            return ret;
        }
    }

    public class FieldWzData
    {
        //Must have
        public string bgm = "Bgm00/GoPicnic";
        public string MapMark = "None";
        public FieldLimit fieldLimit = FieldLimit.FIELDOPT_NONE;
        public int returnMap = 999999999;
        public int forcedReturn = 999999999;
        public int cloud = 0;
        public int swim = 0;
        public int hideMinimap = 0;
        public int town = 0;
        public float mobRate = 1.5f;

        //Optional
        public int? timeLimit = null;
        public int? lvLimit = null;
        public FieldType? fieldType = null;
        public string onFirstUserEnter = null;
        public string onUserEnter = null;
        public int? fly = null;
        public int? noMapCmd = null;
        public int? partyOnly = null;
        public int? reactorShuffle = null;
        public string reactorShuffleName = null;
        public int? personalShop = null;
        public int? entrustedShop = null;
        public string effect = null; //Bubbling; 610030550 and many others
        public int? lvForceMove = null; //limit FROM value
        public TimeMob? timeMob = null;
        public string help = null; //help string
        public int? snow = null;
        public int? rain = null;
        public int? dropExpire = null; //in seconds
        public int? decHP = null;
        public int? decInterval = null;
        public AutoLieDetector? autoLieDetector = null;
        public int? expeditionOnly = null;
        public float? fs = null; //slip on ice speed, default 0.2
        public int? protectItem = null; //ID, item protecting from cold
        public int? createMobInterval = null; //used for massacre pqs
        public int? fixedMobCapacity = null; //mob capacity to target (used for massacre pqs)

        //Unknown optional
        public int? startHour = null;
        public int? endHour = null;
        public int? moveLimit = null;
        public string mapDesc = null;
        public string mapName = null;
        public string streetName = null;
        public int? miniMapOnOff = null;
        public int? noRegenMap = null; //610030400
        public float? recovery = null; //recovery rate, like in sauna (3)
        public int? blockPBossChange = null; //something with monster carnival
        public int? everlast = null; //something with bonus stages of PQs
        public int? damageCheckFree = null; //something with fishing event
        public float? dropRate = null;
        public int? scrollDisable = null;
        public int? needSkillForFly = null;
        public int? zakum2Hack = null; //JQ hack protection
        public int? allMoveCheck = null; //another JQ hack protection
        public int? VRLimit = null; //use vr's as limits?
        public int? consumeItemCoolTime = null; //cool time of consume item

        //Special
        public Rectangle? VR = null;
        public string strMapName = "<Untitled>";
        public string strStreetName = "<Untitled>";

        public struct TimeMob
        {
            public int? startHour, endHour;
            public int id;
            public string message;

            public TimeMob(int? startHour, int? endHour, int id, string message)
            {
                this.startHour = startHour;
                this.endHour = endHour;
                this.id = id;
                this.message = message;
            }
        }

        public struct AutoLieDetector
        {
            public int startHour, endHour, interval, prop; //interval in mins, prop default = 80

            public AutoLieDetector(int startHour, int endHour, int interval, int prop)
            {
                this.startHour = startHour;
                this.endHour = endHour;
                this.interval = interval;
                this.prop = prop;
            }
        }
    }

    [Flags]
    public enum FieldLimit //Credits to Koolk, LightPepsi, Bui
    {
        FIELDOPT_NONE = 0,
        FIELDOPT_MOVELIMIT = 1,
        FIELDOPT_SKILLLIMIT = 2,
        FIELDOPT_SUMMONLIMIT = 4,
        FIELDOPT_MYSTICDOORLIMIT = 8,
        FIELDOPT_MIGRATELIMIT = 0x10,
        FIELDOPT_PORTALSCROLLLIMIT = 0x20,
        FIELDOPT_TELEPORTITEMLIMIT = 0x40,
        FIELDOPT_MINIGAMELIMIT = 0x80,
        FIELDOPT_SPECIFICPORTALSCROLLLIMIT = 0x100,
        FIELDOPT_TAMINGMOBLIMIT = 0x200,
        FIELDOPT_STATCHANGEITEMCONSUMELIMIT = 0x400,
        FIELDOPT_PARTYBOSSCHANGELIMIT = 0x800,
        FIELDOPT_NOMOBCAPACITYLIMIT = 0x1000,
        FIELDOPT_WEDDINGINVITATIONLIMIT = 0x2000,
        FIELDOPT_CASHWEATHERCONSUMELIMIT = 0x4000,
        FIELDOPT_NOPET = 0x8000,
        FIELDOPT_ANTIMACROLIMIT = 0x10000,
        FIELDOPT_FALLDOWNLIMIT = 0x20000,
        FIELDOPT_SUMMONNPCLIMIT = 0x40000,
        FIELDOPT_NOEXPDECREASE = 0x80000,
        FIELDOPT_NODAMAGEONFALLING = 0x100000,
        FIELDOPT_PARCELOPENLIMIT = 0x200000,
        FIELDOPT_DROPLIMIT = 0x400000
    }

    public enum FieldType //Credits to Koolk for about half of them and me for the rest
    {
        FIELDTYPE_DEFAULT = 0,
        FIELDTYPE_SNOWBALL = 1,
        FIELDTYPE_CONTIMOVE = 2,
        FIELDTYPE_TOURNAMENT = 3,
        FIELDTYPE_COCONUT = 4,
        FIELDTYPE_OXQUIZ = 5,
        FIELDTYPE_PERSONALTIMELIMIT = 6,
        FIELDTYPE_WAITINGROOM = 7,
        FIELDTYPE_GUILDBOSS = 8,
        FIELDTYPE_LIMITEDVIEW = 9,
        FIELDTYPE_MONSTERCARNIVAL = 0xA,
        FIELDTYPE_MONSTERCARNIVALREVIVE = 0xB,
        FIELDTYPE_ZAKUM = 0xC,
        FIELDTYPE_ARIANTARENA = 0xD,
        FIELDTYPE_DOJANG = 0xE,
        FIELDTYPE_MONSTERCARNIVAL_S2 = 0xF,
        FIELDTYPE_MONSTERCARNIVALWAITINGROOM = 0x10,
        FIELDTYPE_COOKIEHOUSE = 0x11,
        FIELDTYPE_BALROG = 0x12,
        FIELDTYPE_BATTLEFIELD = 0x13,
        FIELDTYPE_SPACEGAGA = 0x14,
        FIELDTYPE_WITCHTOWER = 0x15,
        FIELDTYPE_ARANTUTORIAL = 0x16,
        FIELDTYPE_MASSACRE = 0x17,
        FIELDTYPE_MASSACRE_RESULT = 0x18,
        FIELDTYPE_PARTYRAID = 0x19,
        FIELDTYPE_PARTYRAID_BOSS = 0x1A,
        FIELDTYPE_PARTYRAID_RESULT = 0x1B,
        FIELDTYPE_NODRAGON = 0x1C,
        FIELDTYPE_DYNAMICFOOTHOLD = 0x1D,
        FIELDTYPE_ESCORT = 0x1E,
        FIELDTYPE_ESCORT_RESULT = 0x1F,
        FIELDTYPE_HUNTINGADBALLOON = 0x20,
        FIELDTYPE_CHAOSZAKUM = 0x21,
        FIELDTYPE_KILLCOUNT = 0x22,
        FIELDTYPE_WEDDING = 0x3C,
        FIELDTYPE_WEDDINGPHOTO = 0x3D,
        FIELDTYPE_FISHINGKING = 0x4A,
        FIELDTYPE_SHOWABATH = 0x51,
        FIELDTYPE_BEGINNERCAMP = 0x52,
        FIELDTYPE_SNOWMAN = 1000,
        FIELDTYPE_SHOWASPA = 1001,
        FIELDTYPE_HORNTAILPQ = 1013,
        FIELDTYPE_CRIMSONWOODPQ = 1014
    }

    public enum PortalType //Credits to me and BluePoop
    {
        Startpoint = 0x0, //sp
        Invisible = 0x1, //pi
        Visible = 0x2, //pv
        Collision = 0x3, //pc
        Changable = 0x4, //pg
        ChangableInvisible = 0x5, //pgi
        TownportalPoint = 0x6, //tp
        Script = 0x7, //ps
        ScriptInvisible = 0x8, //psi
        CollisionScript = 0x9, //pcs
        Hidden = 0xA, //ph
        ScriptHidden = 0xB, //psh
        CollisionVerticalJump = 0xC, //pcj
        CollisionCustomImpact = 0xD, //pci
        CollisionUnknownPcig = 0xE //pcig
    }
}
