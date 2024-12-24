namespace MinimoServer.Shared;

/// <summary>
/// 자원 정보를 담는 DTO. 아이템 ID와 개수를 담고 있다.
/// </summary>
public class ItemDTO
{
    public string ItemID { get; set; }
    public int Count { get; set; }
}