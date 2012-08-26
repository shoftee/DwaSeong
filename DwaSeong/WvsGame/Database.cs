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
using MySql.Data.MySqlClient;
using System.Reflection;
using WvsGame.User;
using WvsGame.WZ;
using MapleLib.PacketLib;
using Common;

namespace WvsGame
{
    public class Database
    {
        public static MySqlConnection connection; // Should I pool connections? <_<

        public static bool EstablishConnection(string user, string pass, string db, string host)
        {
            try
            {
                connection = new MySqlConnection("data source=" + host + ";database=" + db + ";user id=" + user + ";Pwd=" + pass);
                connection.Open();
                return true;
            }
            catch
            {
                connection = null;
                return false;
            }
        }

        public static void ExecuteQuery(string query, params object[] objects)
        {
            query = string.Format(query, objects);
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public static MySqlDataReader ExecuteDataQuery(string query, params object[] objects)
        {
            query = string.Format(query, objects);
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            return command.ExecuteReader();
        }

        /// <summary>
        /// Checks the login status of the account
        /// </summary>
        /// <param name="username">Username of the account</param>
        /// <param name="password">Password of the account</param>
        /// <returns>Check Password result function
        /// 0x00 = Ok
        /// 0x02 = Ban
        /// 0x03 = Blocked or deleted
        /// 0x04 = Invalid Password
        /// 0x05 = Invalid Username
        /// 0x10 = Account not verified via email
        /// </returns>
        public static int CheckPassword(string username, string password)
        {
            int retval = -1;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                string pass = commandreader["PasswordHash"].ToString();
                int ban = Convert.ToInt32(commandreader["IsBanned"]);
                int email = Convert.ToInt32(commandreader["IsEmailPending"]);
                long banexpire = Convert.ToInt64(commandreader["BanExpiration"]);
                if (ban > 0 && banexpire > DateTime.Now.ToFileTime())
                    retval = 2;
                else if (pass == password)
                {
                    if (email == 1)
                        retval = 0x10;
                    retval = 0;
                }
                else
                    retval = 4;
            }
            if (commandreader.FieldCount == 0)
                retval = 5;
            commandreader.Close();
            return retval;
        }

        public static byte GetBanReason(string username)
        {
            byte ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToByte(commandreader["IsBanned"]);
            }
            commandreader.Close();
            return ret;
        }

        public static long GetBanExpiration(string username)
        {
            long ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt64(commandreader["BanExpiration"]);
            }
            commandreader.Close();
            return ret;
        }

        public static int GetAccountId(int cid)
        {
            int ret = 0;
            string query = "SELECT * FROM character_data WHERE id = " + cid + ";";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt32(commandreader["AccountId"]);
            }
            commandreader.Close();
            return ret;
        }

        public static string GetAccountName(string charname)
        {
            string ret = string.Empty;
            string query = "SELECT * FROM character_data WHERE Name = '" + charname + "';";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToString(commandreader["AccountId"]);
            }
            commandreader.Close(); 
            query = "SELECT * FROM account WHERE AccountID = " + ret + ";";
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToString(commandreader["AccountName"]);
            }
            commandreader.Close();
            return ret;
        }

        public static string GetPin(string username)
        {
            string ret = string.Empty;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = commandreader["Pin"].ToString();
            }
            commandreader.Close();
            return ret;
        }

        public static string GetPic(string username)
        {
            string ret = string.Empty;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToString(commandreader["Pic"]);
            }
            commandreader.Close();
            return ret;
        }

        public static int GetRecentWorld(string username)
        {
            int ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt32(commandreader["RecentWorld"]);
            }
            commandreader.Close();
            return ret;
        }

        public static int GetAdmin(string username)
        {
            int ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt32(commandreader["Admin"]);
            }
            commandreader.Close();
            return ret;
        }

        public static int GetTradeBlock(string username)
        {
            int ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt32(commandreader["IsTradeBanned"]);
            }
            commandreader.Close();
            return ret;
        }

        public static long GetTradeBlockExpiration(string username)
        {
            long ret = 0;
            string query = "SELECT * FROM account WHERE AccountName = '" + username + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret = Convert.ToInt64(commandreader["TradeBanExpiration"]);
            }
            commandreader.Close();
            return ret;
        }

        public static byte CheckDuplicatedID(string charname)
        {
            byte ret = 0;
            string query = "SELECT * FROM character_data where Name = '" + charname + "'";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            if (commandreader.HasRows)
                ret = 1;
            return ret;
        }

        public static int GetCID(string charname)
        {
            int ret = 0;
            string query = "SELECT * FROM character_data where Name = '" + charname + "'";
            var command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
                ret = Convert.ToInt32(commandreader["id"]);
            commandreader.Close();
            return ret;
        }

        public static void DeleteCharacter(int cid)
        {
            string query = "DELETE FROM character_data WHERE id = @cid";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@cid", cid);
            command.ExecuteNonQuery();
        }

        public static void IssueBan(string user, byte reason, long length, string desc = "")
        {
            string query = "UPDATE account SET IsBanned = @IsBanned, BanExpiration = @expire, BanDescription = @desc WHERE AccountName = @user";
            MySqlCommand command = new MySqlCommand(query, connection);
            desc += "(" + DateTime.Now + ")";
            command.Parameters.AddWithValue("@IsBanned", reason);
            command.Parameters.AddWithValue("@expire", length);
            command.Parameters.AddWithValue("@desc", desc);
            command.Parameters.AddWithValue("@user", user);
            command.ExecuteNonQuery();
        }

        public static Character GetCharacter(int cid)
        {
            Character ret = new Character();
            string query = "SELECT * FROM character_data WHERE id = " + cid;
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret.mID = Convert.ToInt32(commandreader["id"]);
                ret.mName = commandreader["Name"].ToString();
                ret.mGender = Convert.ToByte(commandreader["Gender"]);
                ret.mSkin = Convert.ToByte(commandreader["Skin"]);
                ret.mHair = Convert.ToInt32(commandreader["Hair"]);
                ret.mFace = Convert.ToInt32(commandreader["Face"]);
                ret.mField = Convert.ToInt32(commandreader["Map"]);
                ret.mFieldPosition = Convert.ToByte(commandreader["MapPosition"]);
                ret.mMeso = Convert.ToInt32(commandreader["Meso"]);
            }
            commandreader.Close();
            query = "SELECT * FROM character_primarystats WHERE CharacterId = " + ret.mID;
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret.mPrimaryStats.Level = Convert.ToByte(commandreader["Level"]);
                ret.mPrimaryStats.Job = Convert.ToInt16(commandreader["Job"]);
                ret.mPrimaryStats.Str = Convert.ToInt16(commandreader["Str"]);
                ret.mPrimaryStats.Dex = Convert.ToInt16(commandreader["Dex"]);
                ret.mPrimaryStats.Int = Convert.ToInt16(commandreader["Int"]);
                ret.mPrimaryStats.Luk = Convert.ToInt16(commandreader["Luk"]);
                ret.mPrimaryStats.HP = Convert.ToInt32(commandreader["Hp"]);
                ret.mPrimaryStats.MaxHP = Convert.ToInt32(commandreader["MaxHp"]);
                ret.mPrimaryStats.MP = Convert.ToInt32(commandreader["Mp"]);
                ret.mPrimaryStats.MaxMP = Convert.ToInt32(commandreader["MaxMp"]);
                ret.mPrimaryStats.AP = Convert.ToInt16(commandreader["Ap"]);
                ulong spData = Convert.ToUInt64(commandreader["Sp"]);
                ret.mPrimaryStats.SP[0] = (short)(spData >> 48);
                ret.mPrimaryStats.SP[1] = (short)(spData << 16 >> 48);
                ret.mPrimaryStats.SP[2] = (short)(spData << 32 >> 48);
                ret.mPrimaryStats.SP[3] = (short)(spData << 48 >> 48);
                ret.mPrimaryStats.EXP = Convert.ToInt32(commandreader["Exp"]);
                ret.mPrimaryStats.Fame = Convert.ToInt32(commandreader["Fame"]);
                ret.mPrimaryStats.DemonSlayerAccessory = Convert.ToInt32(commandreader["DemonSlayerAccessory"]);
                ret.mPrimaryStats.Fatigue = Convert.ToInt32(commandreader["Fatigue"]);
                ret.mPrimaryStats.BattlePoints = Convert.ToInt32(commandreader["BattlePoints"]);
                ret.mPrimaryStats.BattleEXP = Convert.ToInt32(commandreader["BattleExp"]);
            }
            commandreader.Close();
            query = "SELECT * FROM character_traits WHERE CharacterId = " + ret.mID;
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                ret.mTraits.Ambition = Convert.ToInt32(commandreader["Ambition"]);
                ret.mTraits.Insight = Convert.ToInt32(commandreader["Insight"]);
                ret.mTraits.Willpower = Convert.ToInt32(commandreader["Willpower"]);
                ret.mTraits.Diligence = Convert.ToInt32(commandreader["Diligence"]);
                ret.mTraits.Empathy = Convert.ToInt32(commandreader["Empathy"]);
                ret.mTraits.Charm = Convert.ToInt32(commandreader["Charm"]);
                ret.mTraits.AmbitionGained = Convert.ToInt16(commandreader["AmbitionGained"]);
                ret.mTraits.InsightGained = Convert.ToInt16(commandreader["InsightGained"]);
                ret.mTraits.WillpowerGained = Convert.ToInt16(commandreader["WillpowerGained"]);
                ret.mTraits.DiligenceGained = Convert.ToInt16(commandreader["DiligenceGained"]);
                ret.mTraits.EmpathyGained = Convert.ToInt16(commandreader["EmpathyGained"]);
                ret.mTraits.CharmGained = Convert.ToInt16(commandreader["CharmGained"]);
            }
            commandreader.Close();

            ret.mInventory = GetInventoryItems(ret.mID);
            ret.mSkills = GetSkills(ret.mID);
            ret.mGuild = new Guild { GuildID = GetGuildID(ret.mID) };

            commandreader.Close();
            return ret;
        }

        public static void SaveCharacter(Character chr, bool newchr = false)
        {
            string query = "UPDATE `character_data` SET `Name` = @Name, `Gender`= @Gender, `Skin` = @Skin, `Hair` = @Hair, `Face` = @Face, `Map` = @Map, `MapPosition` = @MapPosition, `Meso` = @Meso WHERE id = @CharacterId;";
            if (newchr)
                query = "INSERT `character_data` (AccountId, Name, Gender, Skin, Hair, Face, Map, MapPosition, Meso) VALUES (@AccountId, @Name, @Gender, @Skin, @Hair, @Face, @Map, @MapPosition, @Mesod);";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@AccountId", chr.mClient.mAccount.AccountId);
            command.Parameters.AddWithValue("@Name", chr.mName);
            command.Parameters.AddWithValue("@Gender", chr.mGender);
            command.Parameters.AddWithValue("@Skin", chr.mSkin);
            command.Parameters.AddWithValue("@Hair", chr.mHair);
            command.Parameters.AddWithValue("@Face", chr.mFace);
            command.Parameters.AddWithValue("@Map", chr.mField);
            command.Parameters.AddWithValue("@MapPosition", chr.mFieldPosition);
            command.Parameters.AddWithValue("@Meso", chr.mMeso);
            command.Parameters.AddWithValue("@CharacterId", chr.mID);
            command.ExecuteNonQuery();

            query = "SELECT id FROM character_data WHERE AccountId = " + chr.mClient.mAccount.AccountId;
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                chr.mID = Convert.ToInt32(commandreader["id"]);
            }
            commandreader.Close();

            query = "UPDATE `character_primarystats` SET `Level` = @Level, `Job` = @Job, `Str` = @Str, `Dex` = @Dex, `Int` = @Int, `Luk` = @Luk, `Hp` = @Hp, `MaxHp` = @MaxHp, `Mp` = @Mp, `MaxMp` = @MaxMp, `Ap` = @Ap, `Sp` = @Sp, `Exp` = @Exp, `Fame` = @Fame, `DemonSlayerAccessory` = @DemonSlayerAccessory, `Fatigue` = @Fatigue, `BattlePoints` = @BattlePoints, `BattleExp` = @BattleExp WHERE CharacterId = @CharacterId;";
            if (newchr)
                query = "INSERT `character_primarystats` (CharacterId, Level, Job, Str, Dex, `Int`, Luk, Hp, MaxHp, Mp, MaxMp, Ap, Sp, Exp, Fame, DemonSlayerAccessory, Fatigue, BattlePoints, BattleExp) VALUES (@CharacterId, @Level, @Job, @Str, @Dex, @Int, @Luk, @Hp, @MaxHp, @Mp, @MaxMp, @Ap, @Sp, @Exp, @Fame, @DemonSlayerAccessory, @Fatigue, @BattlePoints, @BattleExp);";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Level", chr.mPrimaryStats.Level);
            command.Parameters.AddWithValue("@Job", chr.mPrimaryStats.Job);
            command.Parameters.AddWithValue("@Str", chr.mPrimaryStats.Str);
            command.Parameters.AddWithValue("@Dex", chr.mPrimaryStats.Dex);
            command.Parameters.AddWithValue("@Int", chr.mPrimaryStats.Int);
            command.Parameters.AddWithValue("@Luk", chr.mPrimaryStats.Luk);
            command.Parameters.AddWithValue("@Hp", chr.mPrimaryStats.HP);
            command.Parameters.AddWithValue("@MaxHp", chr.mPrimaryStats.MaxHP);
            command.Parameters.AddWithValue("@Mp", chr.mPrimaryStats.MP);
            command.Parameters.AddWithValue("@MaxMp", chr.mPrimaryStats.MaxMP);
            command.Parameters.AddWithValue("@Ap", chr.mPrimaryStats.AP);
            ulong SPData = 0x0000000000000000;
            SPData = (SPData & 0x0000FFFFFFFFFFFF) | ((ulong)chr.mPrimaryStats.SP[0] << 48);
            SPData = (SPData & 0xFFFF0000FFFFFFFF) | ((ulong)chr.mPrimaryStats.SP[1] << 32);
            SPData = (SPData & 0xFFFFFFFF0000FFFF) | ((ulong)chr.mPrimaryStats.SP[2] << 16);
            SPData = (SPData & 0xFFFFFFFFFFFF0000) | (ulong)((ulong)chr.mPrimaryStats.SP[3]);
            command.Parameters.AddWithValue("@Sp", SPData);
            command.Parameters.AddWithValue("@Exp", chr.mPrimaryStats.EXP);
            command.Parameters.AddWithValue("@Fame", chr.mPrimaryStats.Fame);
            command.Parameters.AddWithValue("@DemonSlayerAccessory", chr.mPrimaryStats.DemonSlayerAccessory);
            command.Parameters.AddWithValue("@Fatigue", chr.mPrimaryStats.Fatigue);
            command.Parameters.AddWithValue("@BattlePoints", chr.mPrimaryStats.BattlePoints);
            command.Parameters.AddWithValue("@BattleExp", chr.mPrimaryStats.BattleEXP);
            command.Parameters.AddWithValue("@CharacterId", chr.mID);
            command.ExecuteNonQuery();

            query = "UPDATE `character_traits` SET `Ambition` = @Ambition, `Insight` = @Insight, `Willpower` = @Willpower, `Diligence` = @Diligence, `Empathy` = @Empathy, `Charm` = @Charm, `AmbitionGained` = @AmbitionGained, `InsightGained` = @InsightGained, `WillpowerGained` = @WillpowerGained, `DiligenceGained` = @DiligenceGained, `EmpathyGained` = @EmpathyGained, `CharmGained` = @CharmGained WHERE CharacterId = @CharacterId;";
            if (newchr)
                query = "INSERT `character_traits` (CharacterId, Ambition, Insight, Willpower, Diligence, Empathy, Charm, AmbitionGained, InsightGained, WillpowerGained, DiligenceGained, EmpathyGained, CharmGained) VALUES (@CharacterId, @Ambition, @Insight, @Willpower, @Diligence, @Empathy, @Charm, @AmbitionGained, @InsightGained, @WillpowerGained, @DiligenceGained, @EmpathyGained, @CharmGained);";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Ambition", chr.mTraits.Ambition);
            command.Parameters.AddWithValue("@Insight", chr.mTraits.Insight);
            command.Parameters.AddWithValue("@Willpower", chr.mTraits.Willpower);
            command.Parameters.AddWithValue("@Diligence", chr.mTraits.Diligence);
            command.Parameters.AddWithValue("@Empathy", chr.mTraits.Empathy);
            command.Parameters.AddWithValue("@Charm", chr.mTraits.Charm);
            command.Parameters.AddWithValue("@AmbitionGained", chr.mTraits.AmbitionGained);
            command.Parameters.AddWithValue("@InsightGained", chr.mTraits.InsightGained);
            command.Parameters.AddWithValue("@WillpowerGained", chr.mTraits.WillpowerGained);
            command.Parameters.AddWithValue("@DiligenceGained", chr.mTraits.DiligenceGained);
            command.Parameters.AddWithValue("@EmpathyGained", chr.mTraits.EmpathyGained);
            command.Parameters.AddWithValue("@CharmGained", chr.mTraits.CharmGained);
            command.Parameters.AddWithValue("@CharacterId", chr.mID);
            command.ExecuteNonQuery();

            SaveInventoryItems(chr.mID, chr.mInventory);
        }

        public static Dictionary<sbyte, AbstractItem>[] GetInventoryItems(int CId)
        {
            List<Equip> equips = new List<Equip>();
            string query = "SELECT * FROM equips WHERE CharacterId = " + CId;
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                Equip equip = new Equip();
                equip.CharacterId = Convert.ToInt32(commandreader["CharacterId"]);
                equip.ItemId = Convert.ToInt32(commandreader["ItemId"]);
                equip.Position = Convert.ToSByte(commandreader["Position"]);
                equip.Expiration = Convert.ToInt64(commandreader["Expiration"]);
                equip.Owner = Convert.ToString(commandreader["Owner"]);
                equip.Flag = Convert.ToInt16(commandreader["Flag"]);
                equip.Origin = Convert.ToString(commandreader["Origin"]);
                equip.CreationTime = Convert.ToInt64(commandreader["CreationTime"]);

                equip.PossibleUpgrades = Convert.ToByte(commandreader["PossibleUpgrades"]);
                equip.Level = Convert.ToByte(commandreader["Level"]);
                equip.Str = Convert.ToInt16(commandreader["Str"]);
                equip.Dex = Convert.ToInt16(commandreader["Dex"]);
                equip.Int = Convert.ToInt16(commandreader["Int"]);
                equip.Luk = Convert.ToInt16(commandreader["Luk"]);
                equip.IncHP = Convert.ToInt16(commandreader["IncHP"]);
                equip.IncMP = Convert.ToInt16(commandreader["IncMP"]);
                equip.Watk = Convert.ToInt16(commandreader["Watk"]);
                equip.Wdef = Convert.ToInt16(commandreader["Wdef"]);
                equip.Mdef = Convert.ToInt16(commandreader["Mdef"]);
                equip.Mdef = Convert.ToInt16(commandreader["Mdef"]);
                equip.Accuracy = Convert.ToInt16(commandreader["Accuracy"]);
                equip.Avoid = Convert.ToInt16(commandreader["Avoid"]);
                equip.Speed = Convert.ToInt16(commandreader["Speed"]);
                equip.Jump = Convert.ToInt16(commandreader["Jump"]);
                equip.Durability = Convert.ToInt32(commandreader["Durability"]);
                equip.State = Convert.ToByte(commandreader["State"]);
                equip.Enhancements = Convert.ToByte(commandreader["Enhancements"]);
                equip.Pot1 = Convert.ToInt16(commandreader["Potential1"]);
                equip.Pot2 = Convert.ToInt16(commandreader["Potential2"]);
                equip.Pot3 = Convert.ToInt16(commandreader["Potential3"]);
                equip.Pot4 = Convert.ToInt16(commandreader["Potential4"]);
                equip.Pot5 = Convert.ToInt16(commandreader["Potential5"]);
                equip.SocketMask = Convert.ToInt16(commandreader["SocketMask"]);
                equip.Socket1 = Convert.ToInt16(commandreader["Socket1"]);
                equip.Socket2 = Convert.ToInt16(commandreader["Socket2"]);
                equip.Socket3 = Convert.ToInt16(commandreader["Socket3"]);
                equips.Add(equip);
            }
            commandreader.Close();

            List<Item> items = new List<Item>();
            query = "SELECT * FROM items WHERE CharacterId = " + CId;
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                Item item = new Item();
                item.CharacterId = Convert.ToInt32(commandreader["CharacterId"]);
                item.ItemId = Convert.ToInt32(commandreader["ItemId"]);
                item.Position = Convert.ToSByte(commandreader["Position"]);
                item.Expiration = Convert.ToInt64(commandreader["Expiration"]);
                item.Owner = Convert.ToString(commandreader["Owner"]);
                item.Flag = Convert.ToInt16(commandreader["Flag"]);
                item.Origin = Convert.ToString(commandreader["Origin"]);
                item.CreationTime = Convert.ToInt64(commandreader["CreationTime"]);

                item.Quantity = Convert.ToInt16(commandreader["Quantity"]);
                items.Add(item);
            }
            commandreader.Close();

            Dictionary<sbyte, AbstractItem>[] ret = new Dictionary<sbyte, AbstractItem>[6];
            for (int i = 0; i < 6; i++)
                ret[i] = new Dictionary<sbyte, AbstractItem>();
            foreach (Equip e in equips)
                if (e.Position < 0)
                    ret[0].Add(e.Position, e);
                else
                    ret[1].Add(e.Position, e);
            foreach (Item i in items)
                ret[i.ItemId / 1000000].Add(i.Position, i);
            return ret;
        }

        private static void SaveInventoryItems(int cid, IEnumerable<Dictionary<sbyte, AbstractItem>> inventory)
        {
            string query = "DELETE FROM `items` WHERE CharacterId = " + cid;
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            query = "DELETE FROM `equips` WHERE CharacterId = " + cid;
            command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            foreach (Dictionary<sbyte, AbstractItem> invtype in inventory)
            {
                foreach (KeyValuePair<sbyte, AbstractItem> item in invtype)
                {
                    if (item.Value is Item)
                    {

                        query = "INSERT `items` (CharacterId, ItemId, Position, Expiration, Quantity, Owner, Flag, Origin, CreationTime) VALUES (@CharacterId, @ItemId, @Position, @Expiration, @Quantity, @Owner, @Flag, @Origin, @CreationTime);";
                        command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CharacterId", cid);
                        command.Parameters.AddWithValue("@ItemId", item.Value.ItemId);
                        command.Parameters.AddWithValue("@Position", item.Value.Position);
                        command.Parameters.AddWithValue("@Expiration", item.Value.Expiration);
                        command.Parameters.AddWithValue("@Owner", item.Value.Owner);
                        command.Parameters.AddWithValue("@Flag", item.Value.Flag);
                        command.Parameters.AddWithValue("@Origin", item.Value.Origin);
                        command.Parameters.AddWithValue("@CreationTime", item.Value.CreationTime);
                        Item titem = (Item)item.Value;
                        command.Parameters.AddWithValue("@Quantity", titem.Quantity);
                        command.ExecuteNonQuery();
                    }
                    else if (item.Value is Equip)
                    {
                        query = "INSERT `equips` (CharacterId, ItemId, Position, Expiration, Owner, Flag, Origin, CreationTime, PossibleUpgrades, Level, Str, Dex, `Int`, Luk, IncHP, IncMP, Watk, Wdef, Matk, Mdef, Accuracy, Avoid, Speed, Jump, Durability, State, Enhancements, Potential1, Potential2, Potential3, Potential4, Potential5, SocketMask, Socket1, Socket2, Socket3) VALUES (@CharacterId, @ItemId, @Position, @Expiration, @Owner, @Flag, @Origin, @CreationTime, @PossibleUpgrades, @Level, @Str, @Dex, @Int, @Luk, @IncHP, @IncMP, @Watk, @Wdef, @Matk, @Mdef, @Accuracy, @Avoid, @Speed, @Jump, @Durability, @State, @Enhancements, @Potential1, @Potential2, @Potential3, @Potential4, @Potential5, @SocketMask, @Socket1, @Socket2, @Socket3);";
                        command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@CharacterId", cid);
                        command.Parameters.AddWithValue("@ItemId", item.Value.ItemId);
                        command.Parameters.AddWithValue("@Position", item.Value.Position);
                        command.Parameters.AddWithValue("@Expiration", item.Value.Expiration);
                        command.Parameters.AddWithValue("@Owner", item.Value.Owner);
                        command.Parameters.AddWithValue("@Flag", item.Value.Flag);
                        command.Parameters.AddWithValue("@Origin", item.Value.Origin);
                        command.Parameters.AddWithValue("@CreationTime", item.Value.CreationTime);
                        Equip tequip = (Equip)item.Value;
                        command.Parameters.AddWithValue("@PossibleUpgrades", tequip.PossibleUpgrades);
                        command.Parameters.AddWithValue("@Level", tequip.Level);
                        command.Parameters.AddWithValue("@Str", tequip.Str);
                        command.Parameters.AddWithValue("@Dex", tequip.Dex);
                        command.Parameters.AddWithValue("@Int", tequip.Int);
                        command.Parameters.AddWithValue("@Luk", tequip.Luk);
                        command.Parameters.AddWithValue("@IncHP", tequip.IncHP);
                        command.Parameters.AddWithValue("@IncMP", tequip.IncMP);
                        command.Parameters.AddWithValue("@Watk", tequip.Watk);
                        command.Parameters.AddWithValue("@Wdef", tequip.Wdef);
                        command.Parameters.AddWithValue("@Matk", tequip.Matk);
                        command.Parameters.AddWithValue("@Mdef", tequip.Mdef);
                        command.Parameters.AddWithValue("@Accuracy", tequip.Accuracy);
                        command.Parameters.AddWithValue("@Avoid", tequip.Avoid);
                        command.Parameters.AddWithValue("@Speed", tequip.Speed);
                        command.Parameters.AddWithValue("@Jump", tequip.Jump);
                        command.Parameters.AddWithValue("@Durability", tequip.Durability);
                        command.Parameters.AddWithValue("@State", tequip.State);
                        command.Parameters.AddWithValue("@Enhancements", tequip.Enhancements);
                        command.Parameters.AddWithValue("@Potential1", tequip.Pot1);
                        command.Parameters.AddWithValue("@Potential2", tequip.Pot2);
                        command.Parameters.AddWithValue("@Potential3", tequip.Pot3);
                        command.Parameters.AddWithValue("@Potential4", tequip.Pot4);
                        command.Parameters.AddWithValue("@Potential5", tequip.Pot5);
                        command.Parameters.AddWithValue("@SocketMask", tequip.SocketMask);
                        command.Parameters.AddWithValue("@Socket1", tequip.Socket1);
                        command.Parameters.AddWithValue("@Socket2", tequip.Socket2);
                        command.Parameters.AddWithValue("@Socket3", tequip.Socket3);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SetAccountInfo(Account acc)
        {
            string query = "SELECT * FROM account WHERE AccountID = " + acc.AccountId + ";";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                acc.Username = commandreader["AccountName"].ToString();
                acc.Password = commandreader["PasswordHash"].ToString();
                acc.Pin = commandreader["Pin"].ToString();
                acc.Banned = Convert.ToInt32(commandreader["IsBanned"]);
                acc.CurrentIP = commandreader["CurrentIP"].ToString();
                acc.EmailPending = Convert.ToInt32(commandreader["IsEmailPending"]);
                acc.TradeBlock = Convert.ToInt32(commandreader["IsTradeBanned"]);
                acc.BanExpiration = Convert.ToInt64(commandreader["BanExpiration"]);
                acc.TradeBlockExpiration = Convert.ToInt64(commandreader["TradeBanExpiration"]);
                acc.Pic = commandreader["Pic"].ToString();
                acc.Admin = Convert.ToInt32(commandreader["Admin"]);
                acc.BanDescription = commandreader["BanDescription"].ToString();
                acc.MacAddress = HexEncoding.GetBytes(commandreader["MacAddress"].ToString());
                acc.HDDSerial = HexEncoding.GetBytes(commandreader["HWID"].ToString());
                acc.SessionID = Convert.ToInt64(commandreader["SessionID"]);
                acc.CreateDate = commandreader["CreateDate"].ToString();
            }
            commandreader.Close();
        }

        public static List<Skill> GetSkills(int cid)
        {
            List<Skill> ret = new List<Skill>();
            string query = "SELECT * FROM skills WHERE characterid = " + cid;
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                Skill skill = new Skill();
                skill.SkillID = Convert.ToInt32(commandreader["skillid"]);
                skill.SkillLevel = Convert.ToInt32(commandreader["level"]);
                skill.SkillMastery  = Convert.ToInt32(commandreader["mastery"]);
                skill.Expiration   = Convert.ToInt64(commandreader["expiration"]);
                skill.Cooldown = Convert.ToInt64(commandreader["cooldown"]);
                ret.Add(skill);
            }
            commandreader.Close();
            return ret;
        }

        public static List<WavebanEntry> GetWavebans()
        {
            List<WavebanEntry> ret = new List<WavebanEntry>();
            string query = "SELECT * FROM wavebans;";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.ExecuteNonQuery();
            MySqlDataReader commandreader = command.ExecuteReader();
            while (commandreader.Read())
            {
                WavebanEntry entry = new WavebanEntry(
                    Convert.ToInt32(commandreader["issuedby"]),
                    Convert.ToInt64(commandreader["issuedtime"]),
                    Convert.ToInt32(commandreader["target"]),
                    Convert.ToInt32(commandreader["bantype"]),
                    Convert.ToInt64(commandreader["banlength"]),
                    Convert.ToInt32(commandreader["processed"]),
                    Convert.ToInt64(commandreader["processedtime"]),
                    Convert.ToString(commandreader["comment"]));
                ret.Add(entry);
            }
            commandreader.Close();
            return ret;
        }

        public struct WavebanEntry
        {
            public int issuedby, target, bantype, processed;
            public long issuedtime, banlength, processedtime;
            public string comment;

            public WavebanEntry(int cidIssued, long timeIssued, int cidTarget, int banType, long banLength, int processed, long timeProcessed, string comment = "")
            {
                issuedby = cidIssued;
                target = cidTarget;
                bantype = banType;
                this.processed = processed;
                issuedtime = timeIssued;
                processedtime = timeProcessed;
                banlength = banLength;
                this.comment = comment;
            }
        }

        public static void AddWaveban(WavebanEntry entry)
        {
            string query = "INSERT INTO wavebans (`issuedby`, `issuedtime`, `target`, `bantype`, `banlength`, `processed`, `processedtime`, `comment`) VALUES (@issuedby, @issuedtime, @target, @bantype, @banlength, @processed, @processedtime, @comment);";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@issuedby", entry.issuedby);
            command.Parameters.AddWithValue("@issuedtime", entry.issuedtime);
            command.Parameters.AddWithValue("@target", entry.target);
            command.Parameters.AddWithValue("@bantype", entry.bantype);
            command.Parameters.AddWithValue("@banlength", entry.banlength);
            command.Parameters.AddWithValue("@processed", entry.processed);
            command.Parameters.AddWithValue("@processedtime", entry.processedtime);
            command.Parameters.AddWithValue("@comment", entry.comment);
            command.ExecuteNonQuery();
        }

        public static List<WavebanEntry> ProcessWaveBans(List<WavebanEntry> entries)
        {
            List<WavebanEntry> newentries = new List<WavebanEntry>();
            foreach (WavebanEntry entry in entries)
            {
                WavebanEntry newentry = entry;
                if (newentry.processed == 0)
                {
                    newentry.processed = 1;
                    string query = "UPDATE wavebans SET processed = 1, processedtime = " + newentry.processedtime + " WHERE issuedtime = " + newentry.issuedtime + ";";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    if (command.ExecuteNonQuery() == 0) // no rows updated
                    {
                        AddWaveban(newentry);
                    }
                    int accountid = GetAccountId(newentry.target);
                    query = "UPDATE account SET IsBanned = " + newentry.bantype + ", BanExpiration = " + newentry.banlength + ", BanDescription = '" + newentry.comment + "' WHERE AccountId = " + accountid + ";";
                    command = new MySqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                newentries.Add(newentry);
            }
            return newentries;
        }

        public static void Unban(string acc)
        {
            ExecuteQuery("UPDATE account SET IsBanned = 0 WHERE AccountName = '{0}';", acc);
        }

        public static int GetGuildID(int cid)
        {
            int ret = 0;
            var reader = ExecuteDataQuery("SELECT * FROM guild_member WHERE CharacterID = {0};", cid);
            while (reader.Read())
            {
                ret = Convert.ToInt32(reader["GuildID"]);
            }
            reader.Close();
            return ret;
        }
    }
}
