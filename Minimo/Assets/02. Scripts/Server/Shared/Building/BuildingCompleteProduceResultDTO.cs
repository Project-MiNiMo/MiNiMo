using System.Collections.Generic;

namespace MinimoShared
{
    public class BuildingCompleteProduceResultDTO
    {
        public BuildingDTO UpdatedBuilding { get; set; }
        public List<ItemDTO> ProducedItems { get; set; }
        public CurrencyDTO UpdatedCurrency { get; set; }
    }
}