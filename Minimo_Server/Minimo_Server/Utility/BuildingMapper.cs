using MinimoServer.Models;
using MinimoShared;

public class BuildingMapper
{
    public static BuildingDTO ToBuildingDTO(Building building)
    {
        return new BuildingDTO
        {
            Id = building.Id,
            Name = building.Name,
            IsInstalled = building.IsInstalled,
            Position = building.PositionVector,
            ProduceStatus = building.ProduceStatus,
            RecipeIndex = building.RecipeIndex,
            ProduceStartAt = building.ProduceStartAt
        };
    }

    public static IEnumerable<BuildingDTO> ToBuildingDTOs(IEnumerable<Building> buildings)
    {
        return buildings.Select(ToBuildingDTO);
    }
}