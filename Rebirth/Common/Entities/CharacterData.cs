﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Game;
using Common.Packets;

namespace Common.Entities
{
    public sealed class CharacterData
    {
        public AvatarLook Look { get; set; }
        public MapPos Position { get; set; }
        
        //----------

        public GW_CharacterStat Stats { get; set; } //characterStat

        public CInventory<short, GW_ItemSlotEquip> aInvEquippedNormal { get; }
        public CInventory<short, GW_ItemSlotEquip> aInvEquippedCash { get; }
        public CInventory<short, GW_ItemSlotEquip> aInvEquip { get; }
        public CInventory<short, GW_ItemSlotEquip> aInvEquippedExt { get; }
        public CInventory<short, GW_ItemSlotEquip> aInvEquippedUnk { get; }
        public CInventory<byte, GW_ItemSlotBundle> aInvConsume { get; }
        public CInventory<byte, GW_ItemSlotBundle> aInvInstall { get; }
        public CInventory<byte, GW_ItemSlotBundle> aInvEtc { get; }
        public CInventory<byte, GW_ItemSlotBundle> aInvCash { get; }

        //ZRef<GW_ItemSlotBase> aEquipped[60];
        //ZRef<GW_ItemSlotBase> aEquipped2[60];
        //ZRef<GW_ItemSlotBase> aDragonEquipped[4];
        //ZRef<GW_ItemSlotBase> aMechanicEquipped[5];
        //ZArray<ZRef<GW_ItemSlotBase>> aaItemSlot[6];

        //_FILETIME aEquipExtExpire[1];
        //ZMap<long, EQUIPPED_SETITEM, long> m_mEquippedSetItem;
        public int nCombatOrders { get; set; }
        //ZMap<long, long, long> mSkillRecord;
        //ZMap<long, long, long> mSkillRecordEx;
        //ZMap<long, long, long> mSkillMasterLev;
        //ZMap<long, _FILETIME, long> mSkillExpired;
        //ZMap<long, unsigned short, long> mSkillCooltime;
        //ZMap<unsigned short, _FILETIME, unsigned short> mQuestComplete;
        //ZMap<unsigned short, _FILETIME, unsigned short> mQuestCompleteOld;
        //ZMap<long, ZRef<GW_MiniGameRecord>, long> mMiniGameRecord;
        public int nFriendMax { get; set; }
        //ZList<GW_CoupleRecord> lCoupleRecord;
        //ZList<GW_FriendRecord> lFriendRecord;
        //ZList<GW_NewYearCardRecord> lNewYearCardRecord;
        //ZList<GW_MarriageRecord> lMarriageRecord;
        public int[] adwMapTransfer { get; private set; }
        public int[] adwMapTransferEx { get; private set; }
        //int bReachMaxLevel;
        //_FILETIME ftReachMaxLevelTime;
        //int nItemTotalNumber[5];
        //ZMap<long, long, long> mAdminShopCommodityCount;
        public string sLinkedCharacter { get; set; }
        //ZRef<GW_WildHunterInfo> pWildHunterInfo;
        //ZMap<long, ZRef<GW_MonsterBookCard>, long> mpMonsterBookCard;
        public int nMonsterBookCoverID { get; set; }
        public int nMonsterCardNormal { get; set; }
        public int nMonsterCardSpecial { get; set; }
        //ZMap<unsigned short, ZXString<char>, unsigned short> mQuestRecord;
        //ZMap<unsigned short, CSimpleStrMap, unsigned short> mQuestRecordEx;
        //ZArray<Additional::SKILL> aSkill;
        //int aMobCategoryDamage[9];
        //int aElemBoost[8];
        //Additional::CRITICAL critical;
        //Additional::BOSS boss;
        //ZMap<ZXString<char>, ZPair<long, long>, ZXString<char>> aUpgradeCountByDamageTheme;
        //ZMap<long, long, long> m_mVisitorQuestLog;

        private CharacterData()
        {
            // Stats = new GW_CharacterStat();

            adwMapTransfer = new int[5];
            adwMapTransferEx = new int[10];

            aInvEquippedNormal = new CInventory<short, GW_ItemSlotEquip>();
            aInvEquippedCash = new CInventory<short, GW_ItemSlotEquip>();
            aInvEquip = new CInventory<short, GW_ItemSlotEquip>();
            aInvEquippedExt = new CInventory<short, GW_ItemSlotEquip>();
            aInvEquippedUnk = new CInventory<short, GW_ItemSlotEquip>();
            aInvConsume = new CInventory<byte, GW_ItemSlotBundle>();
            aInvInstall = new CInventory<byte, GW_ItemSlotBundle>();
            aInvEtc = new CInventory<byte, GW_ItemSlotBundle>();
            aInvCash = new CInventory<byte, GW_ItemSlotBundle>();
        }

        public static CharacterData Create(GW_CharacterStat stats, AvatarLook look)
        {
            var x = new CharacterData();

            x.Stats = stats;
            x.Look = look;
            x.Position = new MapPos();

            //----------

            var v1 = new GW_ItemSlotBundle();
            v1.nItemID = 2000007;
            v1.nNumber = 5;

            var v2 = new GW_ItemSlotBundle();
            v2.nItemID = 2000010;
            v2.nNumber = 500;

            x.aInvConsume.Add(1, v1);
            x.aInvConsume.Add(2, v2);

            var v3 = new GW_ItemSlotEquip();
            v3.nItemID = 1002080;

            var v4 = new GW_ItemSlotEquip();
            v4.nItemID = 1302016;

            x.aInvEquippedNormal.Add(1, v3);
            x.aInvEquip.Add(1, v4);
            
            //----------
            
            return x;
        }

        //CharacterData::Decode
        public void Encode(COutPacket p)
        {
            //TODO: Clean this packet up and make it 
            //cool with the flags at bottom of file
            const long dbcharFlag = -1; //DBCHAR_FLAGS.ALL

            p.Encode8(dbcharFlag);
            p.Encode1((byte)nCombatOrders);
            p.Encode1(0); //Some Loop.

            //if ((dbcharFlag & 1) > 0)
            //{
            Stats.Encode(p);   //addCharStats(mplew, chr);
            p.Encode1((byte)nFriendMax);//chr.getBuddylist().getCapacity()

            bool linkedChar = false;//string.IsNullOrWhiteSpace(sLinkedCharacter);

            p.Encode1(linkedChar);

            if (linkedChar)
            {
                p.EncodeString(sLinkedCharacter);
            }
            //}

            //if ((dbcharFlag & 2) > 0)
            //{
            Stats.EncodeMoney(p);
            //}

            EncodeInventoryInfo(p);
            EncodeSkillInfo(p);
            EncodeQuestInfo(p);
            EncodeMiniGameInfo(p);
            EncodeRingInfo(p);
            EncodeTeleportInfo(p);
            EncodeMonsterBookInfo(p);
            EncodeNewYearInfo(p); // Short 
            EncodeAreaInfo(p); // Short 
            p.Encode2(0);
            p.Encode2(0); //m_mVisitorQuestLog
        }
        private void EncodeInventoryInfo(COutPacket p)
        {
            p.Encode1(aInvEquip.SlotLimit);
            p.Encode1(aInvConsume.SlotLimit);
            p.Encode1(aInvInstall.SlotLimit);
            p.Encode1(aInvEtc.SlotLimit);
            p.Encode1(aInvCash.SlotLimit);

            p.Encode8(Constants.PERMANENT); //EQUIPEXTEXPIRE 

            foreach (var i in aInvEquippedNormal)
            {
                p.Encode2(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode2(0);

            foreach (var i in aInvEquippedCash)
            {
                p.Encode2(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode2(0);

            foreach (var i in aInvEquip)
            {
                p.Encode2(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode2(0);

            foreach (var i in aInvEquippedExt)
            {
                p.Encode2(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode2(0);

            foreach (var i in aInvEquippedUnk)
            {
                p.Encode2(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode2(0);

            foreach (var i in aInvConsume)
            {
                p.Encode1(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode1(0);

            foreach (var i in aInvInstall)
            {
                p.Encode1(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode1(0);

            foreach (var i in aInvEtc)
            {
                p.Encode1(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode1(0);

            foreach (var i in aInvCash)
            {
                p.Encode1(i.Key);
                i.Value.RawEncode(p);
            }
            p.Encode1(0);
        }
        private void EncodeSkillInfo(COutPacket p)
        {
            //Map<Skill, MapleCharacter.SkillEntry> skills = chr.getSkills();
            //int skillsSize = skills.size();
            //// We don't want to include any hidden skill in this, so subtract them from the size list and ignore them.
            //for (Iterator<Entry<Skill, SkillEntry>> it = skills.entrySet().iterator(); it.hasNext();)
            //{
            //    Entry<Skill, MapleCharacter.SkillEntry> skill = it.next();
            //    if (GameConstants.isHiddenSkills(skill.getKey().getId()))
            //    {
            //        skillsSize--;
            //    }
            //}
            p.Encode2(0);//mplew.writeShort(skillsSize);
            //for (Iterator<Entry<Skill, SkillEntry>> it = skills.entrySet().iterator(); it.hasNext();)
            //{
            //    Entry<Skill, MapleCharacter.SkillEntry> skill = it.next();
            //    if (GameConstants.isHiddenSkills(skill.getKey().getId()))
            //    {
            //        continue;
            //    }
            //    mplew.writeInt(skill.getKey().getId());
            //    mplew.writeInt(skill.getValue().skillevel);
            //    addExpirationTime(mplew, skill.getValue().expiration);
            //    if (skill.getKey().isFourthJob())
            //    {
            //        mplew.writeInt(skill.getValue().masterlevel);
            //    }
            //}
            p.Encode2(0);//mplew.writeShort(chr.getAllCooldowns().size());
            //for (PlayerCoolDownValueHolder cooling : chr.getAllCooldowns())
            //{
            //    mplew.writeInt(cooling.skillId);
            //    int timeLeft = (int)(cooling.length + cooling.startTime - System.currentTimeMillis());
            //    mplew.writeShort(timeLeft / 1000);
            //}
        }
        private void EncodeQuestInfo(COutPacket p)
        {
            p.Encode2(0);//mplew.writeShort(chr.getStartedQuestsSize());
            //for (MapleQuestStatus q : chr.getStartedQuests())
            //{
            //    mplew.writeShort(q.getQuest().getId());
            //    mplew.writeMapleAsciiString(q.getQuestData());
            //    if (q.getQuest().getInfoNumber() > 0)
            //    {
            //        mplew.writeShort(q.getQuest().getInfoNumber());
            //        mplew.writeMapleAsciiString(q.getQuestData());
            //    }
            //}
            //List<MapleQuestStatus> completed = chr.getCompletedQuests();
            //mplew.writeShort(completed.size());
            p.Encode2(0);//for (MapleQuestStatus q : completed)
            //{
            //    mplew.writeShort(q.getQuest().getId());
            //    mplew.writeLong(getTime(q.getCompletionTime()));
            //}
        }
        private void EncodeMiniGameInfo(COutPacket p)
        {
            p.Encode2(0);
        }
        private void EncodeRingInfo(COutPacket p)
        {
            p.Encode2(0); //getCrushRings
            p.Encode2(0); //getFriendshipRings
            p.Encode2(0); //getMarriageRing
        }
        private void EncodeTeleportInfo(COutPacket p)
        {
            for (int i = 0; i < adwMapTransfer.Length; i++)
                p.Encode4(adwMapTransfer[i]);

            for (int i = 0; i < adwMapTransferEx.Length; i++)
                p.Encode4(adwMapTransferEx[i]);
        }
        private void EncodeMonsterBookInfo(COutPacket p)
        {
            p.Encode4(nMonsterBookCoverID);//mplew.writeInt(chr.getMonsterBookCover()); // cover
            p.Encode1(0);//mplew.write(0);
            //Map<Integer, Integer> cards = chr.getMonsterBook().getCards();
            p.Encode2(0);//mplew.writeShort(cards.size());
            //for (Entry<Integer, Integer> all : cards.entrySet())
            //{
            //    mplew.writeShort(all.getKey() % 10000); // Id
            //    mplew.write(all.getValue()); // Level
            //}
        }
        private void EncodeNewYearInfo(COutPacket p)
        {
            p.Encode2(0);
        }
        private void EncodeAreaInfo(COutPacket p)
        {
            p.Encode2(0);
        }
    }

    /* 466 */
    enum DBCHAR_FLAGS
    {
        CHARACTER = 0x1,
        MONEY = 0x2,
        ITEMSLOTEQUIP = 0x4,
        ITEMSLOTCONSUME = 0x8,
        ITEMSLOTINSTALL = 0x10,
        ITEMSLOTETC = 0x20,
        ITEMSLOTCASH = 0x40,
        INVENTORYSIZE = 0x80,
        SKILLRECORD = 0x100,
        QUESTRECORD = 0x200,
        MINIGAMERECORD = 0x400,
        COUPLERECORD = 0x800,
        MAPTRANSFER = 0x1000,
        AVATAR = 0x2000,
        QUESTCOMPLETE = 0x4000,
        SKILLCOOLTIME = 0x8000,
        MONSTERBOOKCARD = 0x10000,
        MONSTERBOOKCOVER = 0x20000,
        NEWYEARCARD = 0x40000,
        QUESTRECORDEX = 0x80000,
        ADMINSHOPCOUNT = 0x100000,
        EQUIPEXT = 0x100000,
        WILDHUNTERINFO = 0x200000,
        QUESTCOMPLETE_OLD = 0x400000,
        VISITORLOG = 0x800000,
        VISITORLOG1 = 0x1000000,
        VISITORLOG2 = 0x2000000,
        VISITORLOG3 = 0x4000000,
        VISITORLOG4 = 0x8000000,
        //ALL = 0xFFFFFFFF,
        ITEMSLOT = 0x7C,
    };
}
