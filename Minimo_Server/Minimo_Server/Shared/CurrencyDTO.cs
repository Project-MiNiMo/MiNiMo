namespace MinimoServer.Shared;

/// <summary>
/// 화폐 정보를 담는 DTO. 별, 파란별, 하트를 담고 있다.
/// </summary>
public class CurrencyDTO
{
    public int Star { get; set; }
    public int BlueStar { get; set; }
    public int Heart { get; set; }
}