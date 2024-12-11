using System.Numerics;

namespace MinimoShared;

public class BuildingDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsInstalled { get; set; }
    public Vector3 Position { get; set; }
    
    public bool ProduceStatus { get; set; }
    
    public int RecipeIndex { get; set; }
    
    public DateTime ProduceStartAt { get; set; }
}