namespace MinimoShared
{
    public class BuildingInfoDTO
    {   
        public string BuildingType { get; set; }
        public int OwnCount { get; set; }
        public int MaxCount { get; set; }
        public int InstallCount { get; set; } // 검증용
        public int ProduceSlotCount { get; set; }
    }
}