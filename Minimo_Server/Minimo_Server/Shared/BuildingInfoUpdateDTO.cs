using System;

namespace MinimoShared
{
    public class BuildingInfoUpdateDTO
    {
        public BuildingInfoDTO ObtainedBuildingInfo { get; set; }
        public BuildingInfoDTO CurrentBuildingInfo { get; set; }
        public string UpdateReason { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}