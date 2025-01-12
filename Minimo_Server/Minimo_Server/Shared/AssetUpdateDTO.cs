namespace MinimoShared;

public class AssetUpdateDTO
{
    public CurrencyUpdateDTO? CurrencyUpdate { get; set; }
    public List<BuildingInfoUpdateDTO> BuildingsUpdate { get; set; }
    public List<ItemUpdateDTO>? ItemsUpdate { get; set; }
}