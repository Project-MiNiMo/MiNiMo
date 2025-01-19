using System.Collections.Generic;

namespace MinimoShared
{
    public class BuildingCreateResultDTO
    {
        public BuildingDTO CreatedBuilding { get; set; }
        public BuildingInfoDTO BuildingInfoDto { get; set; }
        public List<ItemDTO> UpdatedItems { get; set; }
    }
}