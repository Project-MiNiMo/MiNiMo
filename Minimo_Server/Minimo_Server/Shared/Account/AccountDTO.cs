using System.Collections.Generic;
using System;

namespace MinimoShared
{
    /// <summary>
    /// 계정의 정보를 담는 DTO. ID, 닉네임, 레벨, 경험치, 보유한 건물, 아이템 정보를 담고 있다. 
    /// </summary>
    public class AccountDTO
    {
        // 서버측 계정 ID
        public int ID { get; set; }
        // 닉네임
        public string Nickname { get; set; }
        // 레벨
        public int Level { get; set; }
        // 경험치
        public int Experience { get; set; }
        // 재화
        public CurrencyDTO Currency { get; set; }
        // 건물
        public List<BuildingDTO> Buildings { get; set; }
        // 건물 정보
        public List<BuildingInfoDTO> BuildingInfos { get; set; }
        // 자원
        public List<ItemDTO> Items { get; set; }
        // 유성
        public List<MeteorDTO> Meteors { get; set; }
        public DateTime LastMeteorCreatedAt { get; set; }
        // 퀘스트
        public List<QuestDTO> Quests { get; set; }
        // 별나무
        public DateTime LastStarTreeCreatedAt { get; set; }
        public DateTime LastWishedAt { get; set; }
        public int StarTreeLevel { get; set; }
    }
}