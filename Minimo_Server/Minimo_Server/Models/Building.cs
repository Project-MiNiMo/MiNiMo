using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MinimoServer.Models;

public class Building
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } // 건물 이름

    public int Level { get; set; } // 건물 레벨

    public DateTime CreatedAt { get; set; } // 건물 생성일
    
    public bool IsInstalled { get; set; } // 건물 설치 여부
    
    public string Position { get; set; } // 건물 위치 "x,y,z"
    
    public bool ProduceStatus { get; set; } // 생산 상태
    
    public int RecipeIndex { get; set; } // 레시피 인덱스
    
    public DateTime ProduceStartAt { get; set; } // 생산 시작 시간
    
    [NotMapped]
    public Vector3 PositionVector
    {
        get
        {
            var parts = Position.Split(',');
            return new Vector3(
                float.Parse(parts[0]),
                float.Parse(parts[1]),
                float.Parse(parts[2])
            );
        }
        set => Position = $"{value.X},{value.Y},{value.Z}";
    }
}