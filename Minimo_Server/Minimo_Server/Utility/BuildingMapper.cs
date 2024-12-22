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
    
    public static Building ToBuilding(BuildingDTO buildingDTO)
    {
        return new Building
        {
            Id = buildingDTO.Id,
            Name = buildingDTO.Name,
            IsInstalled = buildingDTO.IsInstalled,
            PositionVector = buildingDTO.Position,
            ProduceStatus = buildingDTO.ProduceStatus,
            RecipeIndex = buildingDTO.RecipeIndex,
            ProduceStartAt = buildingDTO.ProduceStartAt
        };
    }

    public static IEnumerable<BuildingDTO> ToBuildingDTOs(IEnumerable<Building> buildings)
    {
        return buildings.Select(ToBuildingDTO);
    }
}