using System.Numerics;
using System;

namespace MinimoShared
{
    /// <summary>
    /// 건물 정보를 담는 DTO. ID, 이름, 설치 여부, 위치, 생산 여부, 레시피 인덱스, 생산 시작 시간을 담고 있다.
    /// </summary>
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
}