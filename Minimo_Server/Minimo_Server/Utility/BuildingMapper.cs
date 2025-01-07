using System.Numerics;
using MinimoServer.Models;
using MinimoShared;

public class BuildingMapper
{
    public static BuildingDTO ToBuildingDTO(Building building)
    {
        return new BuildingDTO
        {
            Id = building.Id,
            BuildingType = building.Name,
            Position = building.Position,
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
            Name = buildingDTO.BuildingType,
            Position = buildingDTO.Position ?? new int[3],
            ProduceStatus = buildingDTO.ProduceStatus ?? false, // 기본값으로 false 사용
            RecipeIndex = buildingDTO.RecipeIndex ?? 0, // 기본값으로 0 사용
            ProduceStartAt = buildingDTO.ProduceStartAt ?? DateTime.MinValue // 기본값으로 최소값 사용
        };
    }

    public static IEnumerable<BuildingDTO> ToBuildingDTOs(IEnumerable<Building> buildings)
    {
        return buildings.Select(ToBuildingDTO);
    }
}