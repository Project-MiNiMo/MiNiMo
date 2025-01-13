namespace MinimoServer.Models;

public class BuildingInfo
{
    public string BuildingType { get; set; }
    public int OwnCount { get; set; }
    public int MaxCount { get; set; }
    public int InstallCount { get; set; } // 클라이언트에는 따로 제공하지 않음
    
    // 제조 슬롯 해금 개수
    public int ProduceSlotCount { get; set; }
}