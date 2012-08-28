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

namespace Common
{
    //function map "C:\Users\Rice\MapleStory_Global\maplestory_111_local.idb"
    //Default: CWvsApp::SendPackupPacket

    public enum SendOps : short
    {
        //CClientSocket::ProcessPacket               // 0x004DB630
        MigrateCommand = 0x10,                       // 0x004D92D0
        AliveReq = 0x11,                             // 0x004DB480
        AuthenCodeChanged = 0x12,                    // 0x004D9450
        AuthenMessage = 0x13,                        // 0x004D9580
        CheckCrc = 0x1A,                             // 0x004D95D0

        //CLogin::OnPacket                           // 0x00676A30
        CheckPassword = 0x00,                        // 0x00673220
        GuestIDLogin = 0x01,                         // 0x00673E60
        AccountInfo = 0x02,                          // 0x006742E0
        CheckUserLimit = 0x03,                       // 0x00667730
        SetAccount = 0x04,                           // 0x0066B8C0
        ConfirmEULA = 0x05,                          // 0x00669A20
        CheckPinCode = 0x06,                         // 0x00671980
        UpdatePinCode = 0x07,                        // 0x006678F0
        ViewAllChar = 0x08,                          // 0x00674E90
        SelectCharacterByVAC = 0x09,                 // 0x006753E0
        WorldInformation = 0x0A,                     // 0x006710C0
        WorldSelect = 0x0B,                          // 0x00674720
        SelectCharacter = 0x0C,                      // 0x00675870
        CheckDuplicatedID = 0x0D,                    // 0x0066B7C0
        CreateNewCharacter = 0x0E,                   // 0x006714C0
        DeleteCharacter = 0x0F,                      // 0x00670520
        EnableSPW = 0x18,                            // 0x00667770
        LatestConnectedWorld = 0x1B,                 // 0x006676E0
        RecommendWorldMessage = 0x1C,                // 0x0066B6D0
        ExtraCharInfo = 0x1D,                        // 0x00667BC0
        EventCharCreate = 0x1E,                      // 0x00667C10
        CheckSPW = 0x1F,                             // 0x006678C0

        //CWvsContext::OnPacket                      // 0x00C40D10
        AntiMacroResult = 0x30,                      // 0x00C53550
        BroadcastMsg = 0x58,                         // 0x00C57AA0
        GMBoard = 0x9C,

        //CStage::OnPacket
        SetField = 0xB9,

        //CNpcPool::OnPacket
        NpcImitateData = 0x65,
        UpdateLimitedDisableInfo = 0x66,
        NpcEnterField = 0x1AC,
        NpcLeaveField = 0x1AD,
        NpcChangeController = 0x1AF,
        NpcMove = 0x1B0,
        UpdateLimitedInfo = 0x1B1,
        SetSpecialAction = 0x1B2,

        //CField::OnPacket
        GroupMessage = 0xC3,

        //CUserPool::OnUserRemotePacket
        UserEnterField = 0xE6,
        PlayerLeaveField = 0xE7,

        //CUserPool::OnUserCommonPacket              // 0x00B71210
        PublicChatMsg = 0xE8,                        // 0x00AE8720

        //CUserRemote::OnPacket
        UserMove = 0x11C,
        SetActivePOrtableCHair = 0x12D,
        AvatarModified = 0x12E,
        SetTemporaryStat = 0x130,
        ResetTemporaryStat = 0x131,
        RecieveHP = 0x132,
        GuildNameChanged = 0x133,
        GuildMarkChanged = 0x134,
        MapleTeamChanged = 0x135,
        PvpHPChanged = 0x137,

        //CUserLocal::OnPacket
        OpenUI = 0x150,
        NoticeMsg = 0x15F,
        ChatMsg = 0x160, // ㅇㅇ 와라오
    }

    public enum RecvOps : short
    {
        ValidateVersion = 0x14,
        CheckPassword = 0x15,
        WorldSelect = 0x19,
        CheckUserLimit = 0x1A,
        WorldInfoRequest = 0x1F,
        WorldInfoReRequest = 0x20,
        ViewAllCharRequest = 0x21,
        EnterServer = 0x28,
        CheckDuplicatedIDRequest = 0x29,
        CreateNewCharacterRequest = 0x2A,
        DeleteCharacterRequest = 0x2D,
        KeepAlive = 0x2E,
        ClientError = 0x2F,
        SelectCharacter = 0x33,

        UserMove = 0x49,
        PublicChat = 0x54,
    }
}
