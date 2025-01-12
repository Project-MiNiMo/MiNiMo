namespace MinimoShared;
using System.Collections.Generic;

public class RewardDTO
{
    public int? Star { get; set; }
    public int? BlueStar { get; set; }
    public int? Heart { get; set; }
    public List<ItemDTO>? Items { get; set; }
}