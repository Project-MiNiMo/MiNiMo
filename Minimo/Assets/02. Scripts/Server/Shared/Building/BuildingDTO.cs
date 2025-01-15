#nullable enable
using System.Numerics;
using System;

namespace MinimoShared
{
    /// <summary>
    /// 건물 정보를 담는 DTO. ID, 이름, 설치 여부, 위치, 생산 여부, 레시피 인덱스, 생산 시작 시간을 담고 있다.
    /// </summary>
    public class BuildingDTO
    {
        public int Id { get; set; } // DB 내부 ID
        public string BuildingType { get; set; } // BuildingTable ID
        public DateTime? ActivatedAt { get; set; } // 건물 설치 완료 시간
        public int[]? Position { get; set; } // 건물 위치(3칸)

        public bool[]? ProduceStatus { get; set; } // 생산 상태

        public int[]? Recipes { get; set; } // 레시피 인덱스(5칸)

        public DateTime[]? ProduceStartAt { get; set; } // 생산 시작 시간
        public DateTime[]? ProduceEndAt { get; set; } // 생산 종료 시간
    }
}