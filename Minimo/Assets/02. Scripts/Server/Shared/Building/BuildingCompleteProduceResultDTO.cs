using System.Collections.Generic;

namespace MinimoShared
{
    public class BuildingCompleteProduceResultDTO
    {
        public BuildingDTO UpdatedBuilding { get; set; }
        public List<ItemDTO> ItemUpdateInfos { get; set; }
        public CurrencyDTO UpdatedCurrency { get; set; }
    }
}