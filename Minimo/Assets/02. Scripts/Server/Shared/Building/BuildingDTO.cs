#nullable enable
using System.Numerics;
using System;

namespace MinimoShared
{
    public enum ProduceSlotStatus
    {
        Idle,       // 생산되지 않은 상태
        Producing,  // 생산 중
        Completed   // 생산 완료 (수확 대기)
    }
    
    /// <summary>
    /// 건물 정보를 담는 DTO. ID, 이름, 설치 여부, 위치, 생산 여부, 레시피 인덱스, 생산 시작 시간을 담고 있다.
    /// </summary>
    public class BuildingDTO
    {
        public int Id { get; set; } // DB 내부 ID
        public string BuildingType { get; set; } // BuildingTable ID
        public DateTime? ActivatedAt { get; set; } // 건물 설치 완료 시간
        public int[]? Position { get; set; } // 건물 위치(3칸)

        public ProduceSlotStatus[]? ProduceStatus { get; set; } // 생산 상태

        public int[]? Recipes { get; set; } // 레시피 인덱스(5칸)

        public DateTime[]? ProduceStartAt { get; set; } // 생산 시작 시간
        public DateTime[]? ProduceEndAt { get; set; } // 생산 종료 시간
        
        public void CopyFrom(BuildingDTO building)
        {
            Id = building.Id;
            BuildingType = building.BuildingType;
            ActivatedAt = building.ActivatedAt;
            Position = building.Position;
            ProduceStatus = building.ProduceStatus;
            Recipes = building.Recipes;
            ProduceStartAt = building.ProduceStartAt;
            ProduceEndAt = building.ProduceEndAt;
        }
    }
}