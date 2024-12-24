using MinimoServer.Shared;
using System.Collections.Generic;

namespace MinimoShared
{
    /// <summary>
    /// 계정의 정보를 담는 DTO. ID, 닉네임, 레벨, 경험치, 보유한 건물, 아이템 정보를 담고 있다. 
    /// </summary>
    public class AccountDTO
    {
        public int ID { get; set; }
        public string Nickname { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        
        public CurrencyDTO Currency { get; set; }
        
        public List<BuildingDTO> Buildings { get; set; }
        
        public List<ItemDTO> Items { get; set; }
    }
}