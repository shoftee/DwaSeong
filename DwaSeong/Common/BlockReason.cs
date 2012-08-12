using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public enum BlockReason
    {
        HACK = 0x01,
        BOT = 0x02,
        AD = 0x03,
        HARASS = 0x04,
        CURSE = 0x05,
        SCAM = 0x06,
        MISCONDUCT = 0x07,
        SELL = 0x08,
        ICASH = 0x09,
        TEMP = 0x0A,
        GM = 0x0B,
        IPROGRAM = 0x0C,
        MEGAPHONE = 0x0D,
        사행성 = 0x0E,
        버그악용 = 0x11,
    }

    public static class BlockReasonOperation
    {
        public static string GetReason(BlockReason reason)
        {
            switch (reason)
            {
                case BlockReason.HACK:
                    return "Your account has been blocked for hacking or illegal use of third-party programs.";
                case BlockReason.BOT:
                    return "Your account has been blocked for using macro / auto-keyboard.";
                case BlockReason.AD:;
                    return "Your account has been blocked for illicit promotion and advertising.";
                case BlockReason.HARASS:
                    return "Your account has been blocked for harassment.";
                case BlockReason.CURSE:
                    return "Your account has been blocked for using profane language.";
                case BlockReason.SCAM:
                    return "Your account has been blocked for scamming.";
                case BlockReason.MISCONDUCT:
                    return "Your account has been blocked for misconduct.";
                case BlockReason.SELL:
                    return "Your account has been blocked for illegal cash transaction";
                case BlockReason.ICASH:
                    return "Your account has been blocked for illegal charging/funding. Please contact customer support for further details.";
                case BlockReason.TEMP:
                    return "Your account has been blocked for temporary request. Please contact customer support for further details.";
                case BlockReason.GM:
                    return "Your account has been blocked for impersonating GM. ";
                case BlockReason.IPROGRAM:
                    return "Your account has been blocked for using illegal programs or violating the game policy. ";
                case BlockReason.MEGAPHONE:
                    return "Your account has been blocked for one of cursing, scamming, or illegal trading via Megaphones.";
                case BlockReason.사행성:
                    return "게임 내에서 사행성 행위를 진행, 조장, 홍보, 지원, 보조하는 등 사행성 관련 일체의 행위로 접속 중지된 아이디입니다.";
                case BlockReason.버그악용:
                    return "게임 내 버그를 사용하거나 악용하는 등 운영원칙에 위배되는 행위로 접속 중지된 아이디입니다. ";
                default:
                    return "(null)";
            }
        }
    }
}
