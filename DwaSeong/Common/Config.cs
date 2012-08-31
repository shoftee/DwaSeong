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
using System.IO;

namespace Common
{
    public static class Config
    {/*
        public static INIFile ini = new INIFile(@".\config.ini");
        public static string SQLCon = ini.IniReadValue("SQL", "CONNECTION");
        public static short Version = short.Parse(ini.IniReadValue("GAME", "VERSION"));
        public static string PatchLocation = ini.IniReadValue("GAME", "PATCH_LOCATION");
        public static string InteractionIP = ini.IniReadValue("GAME", "INTERSERVER_IP");
        public static short InteractionPort = short.Parse(ini.IniReadValue("GAME", "INTERSERVER_PORT"));
        public static Language InterfaceLanguage = (Language)Enum.Parse(typeof(Language), ini.IniReadValue("INTERFACE", "LANGUAGE"));
        public static bool LogPackets = bool.Parse(ini.IniReadValue("INTERFACE", "LOG_PACKETS"));*/
        public static short MajorVersion = 111;
        public static bool MessageBoxes = true;
        public static byte[] CenterServerKey = new byte[] { 
            0x16, 0xA2, 0xC0, 0x8A, 0xFB, 0x5A, 0xF8, 0xF0, 0xF4, 0x77, 0x03, 0xBA, 0x1A, 0xA3, 0x31, 0x51, 
            0xD1, 0xE0, 0x22, 0x48, 0x11, 0x2B, 0x8C, 0x6F, 0xBC, 0xD3, 0xAA, 0x7A, 0x79, 0x2D, 0xDB, 0x76, 
            0x4E, 0xB2, 0x45, 0x4C, 0x18, 0xBF, 0x22, 0xC4, 0x23, 0x3F, 0xDB, 0xF6, 0xD0, 0x24, 0xFA, 0xFD, 
            0xD5, 0x11, 0x64, 0x9E, 0x0A, 0xC3, 0xD6, 0x73, 0x23, 0xFA, 0x96, 0xB5, 0x28, 0x85, 0x62, 0x11, 
            0x5E, 0x9F, 0x94, 0xA6, 0xC3, 0xDA, 0x5D, 0xBA, 0x10, 0xBB, 0xFF, 0x8A, 0xF4, 0x2C, 0xB9, 0xC3, 
            0x7E, 0xA6, 0x0F, 0x25, 0x38, 0x5D, 0x73, 0x7D, 0xFD, 0x5A, 0x97, 0xAA, 0x8F, 0x43, 0xB9, 0x90, 
            0x99, 0x43, 0x32, 0xFD, 0xB4, 0x38, 0x56, 0xD2, 0x35, 0xCE, 0x79, 0xA4, 0x37, 0x4F, 0xD8, 0x4F, 
            0x7E, 0xA0, 0xEC, 0xE5, 0xAB, 0xB7, 0x87, 0x93, 0x57, 0x2C, 0xF0, 0xCD, 0x68, 0xCF, 0xFB, 0xF7 }; // this can be anything. however length must be 128 bytes
    }

    public enum Language
    {
        EN = 0x01,
        KR = 0x02,
    }

    public class ConfigReader
    {
        private string File { get; set; }
        private StreamReader Reader;

        public int Line { get; set; }

        private List<string> ConfigText = new List<string>();

        public ConfigReader(string filename)
        {
            File = filename;
            if (System.IO.File.Exists(filename))
            {
                Reader = new StreamReader(filename, true);
            }
            else
            {
                throw new FileNotFoundException("Cannot find config file '" + File + "'");
            }
            string line = "";
            while (!Reader.EndOfStream)
            {
                line = Reader.ReadLine().Replace("\t", ""); // Remove tabs
                line = line.Trim();
                ConfigText.Add(line);
            }
        }

        ~ConfigReader()
        {
            if (Reader != null)
                Reader.Close();
        }

        public void Close()
        {
            if (Reader != null)
            {
                Reader.Close();
                Reader.Dispose();
            }
        }

        private string GetValue(string sBlock, string sParameter)
        {
            bool startPart = false;
            string ans = "";
            Line = 0;
            foreach (string line in ConfigText)
            {
                Line++;
                if (sBlock != "" && !startPart && line == sBlock + " = {")
                {
                    // Found beginning of block
                    startPart = true;
                }
                else if (startPart && line == "}")
                {
                    // Found end of block while begin found already
                    ans = "";
                    break;
                    //throw new InvalidOperationException("Parameter '" + sParameter + "' not found in block '" + sBlock + "'. (line: " + Line.ToString() + ")");
                }
                else if (line.StartsWith(sParameter + " = "))
                {
                    if (sBlock == "")
                    {
                        ans = line.Replace(sParameter + " = ", "");
                        break;
                    }
                    else if (sBlock != "" && startPart)
                    {
                        ans = line.Replace(sParameter + " = ", "");
                        break;
                    }
                }
            }
            return ans.Trim();
        }

        public IEnumerable<string> GetBlocks(string sMainBlock, bool skipBlocksInsideBlock)
        {
            var ret = new List<string>();
            int block = 0; // Start out of a block
            Line = 0;
            foreach (string line in ConfigText)
            {
                Line++;
                if (block == 0 && line == sMainBlock + " = {")
                {
                    block = 1;
                }
                else if (block == 1 && line == "}")
                {
                    block = 0;
                    break;
                }
                else
                {
                    if (block >= 1)
                    {
                        if (line.Contains(" = {"))
                        {
                            // Another block found
                            block++;
                            ret.Add(line.Replace(" = {", ""));
                        }
                        else if (line == "}")
                        {
                            // Block end found
                            block--;
                        }
                    }
                }
            }
            return ret;
        }

        public IEnumerable<string> getBlocksFromBlock(string sMainBlock, int innerBlock)
        {
            var ret = new List<string>();
            int block = sMainBlock == "" ? 1 : 0;
            int skipBlock = 0;
            Line = 0;
            foreach (string line in ConfigText)
            {
                Line++;
                if (block == 0 && line == sMainBlock + " = {")
                {
                    block = 1;
                }
                else if (block == 1 && line == "}")
                {
                    block = 0;
                    break;
                }
                else
                {
                    if (block >= 1)
                    {
                        if (line.Contains(" = {"))
                        {
                            // Another block found
                            if (block <= innerBlock)
                            {
                                block++;
                                ret.Add(line.Replace(" = {", ""));
                            }
                            else
                            {
                                skipBlock++; // For skipping the '}' 's
                            }
                        }
                        else if (line == "}")
                        {
                            // Block end found
                            if (skipBlock == 0)
                                block--;
                            else
                                skipBlock--;
                        }
                    }
                }
            }
            return ret;
        }

        public string getString(string sBlock, string sParameter)
        {
            return GetValue(sBlock, sParameter);
        }

        public int getInt(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            int retval = 0;
            int.TryParse(val, out retval);
            return retval;
        }

        public uint getUInt(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            uint retval = 0;
            uint.TryParse(val, out retval);
            return retval;
        }

        public short getShort(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            short retval = 0;
            short.TryParse(val, out retval);
            return retval;
        }

        public ushort getUShort(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            ushort retval = 0;
            ushort.TryParse(val, out retval);
            return retval;
        }

        public byte getByte(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            byte retval = 0;
            byte.TryParse(val, out retval);
            return retval;
        }

        public bool getBool(string sBlock, string sParameter)
        {
            string val = GetValue(sBlock, sParameter);
            bool retval = false;
            bool.TryParse(val, out retval);
            return retval;
        }
    }
}