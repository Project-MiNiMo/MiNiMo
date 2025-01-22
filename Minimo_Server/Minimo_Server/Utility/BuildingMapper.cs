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
            BuildingType = building.Type,
            ActivatedAt = building.ActivatedAt,
            Position = building.Position,
            ProduceStatus = building.ProduceStatus,
            Recipes = building.Recipes,
            ProduceStartAt = building.ProduceStartAt,
            ProduceEndAt = building.ProduceEndAt
        };
    }
    
    public static Building ToBuilding(BuildingDTO buildingDTO)
    {
        return new Building
        {
            Id = buildingDTO.Id,
            Type = buildingDTO.BuildingType,
            ActivatedAt = buildingDTO.ActivatedAt ?? DateTime.MinValue, // 기본값으로 최소값 사용
            Position = buildingDTO.Position ?? new int[3],
            ProduceStatus = buildingDTO.ProduceStatus ?? new ProduceSlotStatus[5],
            Recipes = buildingDTO.Recipes ?? new int[5],
            ProduceStartAt = buildingDTO.ProduceStartAt ?? new DateTime[5],
            ProduceEndAt = buildingDTO.ProduceEndAt ?? new DateTime[5]
        };
    }

    public static IEnumerable<BuildingDTO> ToBuildingDTOs(IEnumerable<Building> buildings)
    {
        return buildings.Select(ToBuildingDTO);
    }
    
    // BuildingOwnInfo
    public static BuildingInfoDTO ToBuildingInfoDTO(BuildingInfo buildingInfo)
    {
        return new BuildingInfoDTO
        {
            BuildingType = buildingInfo.BuildingType,
            OwnCount = buildingInfo.OwnCount,
            MaxCount = buildingInfo.MaxCount,
            InstallCount = buildingInfo.InstallCount,
            ProduceSlotCount = buildingInfo.ProduceSlotCount
        };
    }
    
    public static BuildingInfo ToBuildingInfo(BuildingInfoDTO buildingInfoDto)
    {
        return new BuildingInfo
        {
            BuildingType = buildingInfoDto.BuildingType,
            OwnCount = buildingInfoDto.OwnCount,
            MaxCount = buildingInfoDto.MaxCount,
            InstallCount = buildingInfoDto.InstallCount,
            ProduceSlotCount = buildingInfoDto.ProduceSlotCount
        };
    }
}