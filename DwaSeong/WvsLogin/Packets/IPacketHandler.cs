/*
	This file is part of the DwaSeong MapleStory Server
    Copyright (C) 2012 "XiaoKia" <xiaokia@naver.com> 

    이 프로그램은 무료 소프트웨어 입니다: GNU Affero General Public 
    License version 3으로만 퍼가기/수정은 허락되어 있습니다. 하지만 
    다른 버젼의 GNU Affero General Public License밑으로는 이 프로그램을 
    수정하거나 퍼가기는 금지되어있습니다.


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
using WvsLogin.User;
using MapleLib.PacketLib;

namespace WvsLogin.Packets
{
    interface IPacketHandler
    {
        void handlePacket(Client client, PacketReader packet);
    }
}
