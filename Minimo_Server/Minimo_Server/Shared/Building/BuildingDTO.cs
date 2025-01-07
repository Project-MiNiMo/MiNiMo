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
        public int[]? Position { get; set; }

        public bool? ProduceStatus { get; set; }

        public int? RecipeIndex { get; set; }

        public DateTime? ProduceStartAt { get; set; }
    }
}