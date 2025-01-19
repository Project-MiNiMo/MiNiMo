using System.Collections.Generic;

namespace MinimoShared
{
    public class BuildingCompleteProduceResultDTO
    {
        public BuildingDTO UpdatedBuilding { get; set; }
        public List<ItemUpdateDTO> ItemUpdateInfos { get; set; }
    }
}