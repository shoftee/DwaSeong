﻿/*
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
using Common;
using WvsGame.Packets.Handlers;

namespace WvsGame.Packets
{
    class PacketHandler
    {
        private static PacketHandler instance = null;
        private IPacketHandler[] handlers;

        public PacketHandler()
        {
            RegisterHandlers();
        }

        public static PacketHandler getInstance()
        {
            if (instance == null)
            {
                instance = new PacketHandler();
            }
            return instance;
        }

        void RegisterHandlers()
        {
            int maxop = 0;
            foreach (short i in Enum.GetValues(typeof(RecvOps)))
                if (i > maxop)
                    maxop = i;
            handlers = new IPacketHandler[maxop + 1];

            RegisterHandler(RecvOps.EnterServer, new EnterServer());
            RegisterHandler(RecvOps.KeepAlive, new KeepAlive());
            RegisterHandler(RecvOps.PublicChat, new PublicChat());
            RegisterHandler(RecvOps.UserMove, new UserMove());
        }

        void RegisterHandler(RecvOps opcode, IPacketHandler handler)
        {
            short op = (short)opcode;
            try
            {
                handlers[op] = handler;
            }
            catch (Exception)
            {
                Console.WriteLine("Error registering packet handler " + Enum.GetName(typeof(RecvOps), op));
            }
        }

        public IPacketHandler GetHandler(short op)
        {
            try
            {
                if (op > handlers.Length)
                    return null;
                IPacketHandler handler = handlers[op];
                if (handler != null)
                    return handler;
            }
            catch (NullReferenceException)
            {
                //    Logger.Write(Logger.LogTypes.Exception, "NRE{0}", nre);
            }
            catch (Exception)
            {
                //Logger.Write(Logger.LogTypes.Exception, "uh oh {0}", e);
            }
            return null;
        }

        public IPacketHandler GetHandler(RecvOps opcode)
        {
            short op = (short)opcode;
            try
            {
                if (op > handlers.Length)
                    return null;
                IPacketHandler handler = handlers[op];
                if (handler != null)
                    return handler;
            }
            catch (NullReferenceException)
            {
                //    Logger.Write(Logger.LogTypes.Exception, "NRE{0}", nre);
            }
            catch (Exception)
            {
                //Logger.Write(Logger.LogTypes.Exception, "uh oh {0}", e);
            }
            return null;
        }
    }
}
