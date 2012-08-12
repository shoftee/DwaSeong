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

namespace WvsCenter
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
    }
}
