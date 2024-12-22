using MinimoServer.Shared;
using System.Collections.Generic;

namespace MinimoShared
{
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