using System.Collections.Generic;

namespace MinimoShared
{
    public class AssetUpdateDTO
    {
        public CurrencyUpdateDTO? CurrencyUpdate { get; set; }
        public List<BuildingInfoUpdateDTO> BuildingInfosUpdate { get; set; }
        public List<ItemUpdateDTO> ItemsUpdate { get; set; }
    }
}