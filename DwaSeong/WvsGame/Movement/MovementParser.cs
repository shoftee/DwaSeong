using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MapleLib.PacketLib;

namespace WvsGame.Movement
{
    class MovementParser
    {
        public static List<IMovePath> ParseMovementPath(PacketReader packet)
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
                        Logger.Write(Logger.LogTypes.경고, "NEW MOVEMENT {0} : {1}", fragmentType, /*packet.ToArray().ToString2s()*/"ㅋㅋ");
                        break;
                }
            }
            return Fragments;
        }
    }
}