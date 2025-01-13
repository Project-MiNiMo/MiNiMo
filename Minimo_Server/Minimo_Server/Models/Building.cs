using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MinimoServer.Models;

public class Building
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } // 건물 이름

    public int Level { get; set; } // 건물 레벨

    public DateTime CreatedAt { get; set; } // 건물 생성일
    public DateTime ActivatedAt { get; set; } // 건물 활성화 시점
    
    public int[] Position { get; set; } = new int[3];
    
    public bool ProduceStatus { get; set; } // 생산 상태
    
    public int RecipeIndex { get; set; } // 레시피 인덱스
    
    public DateTime ProduceStartAt { get; set; } // 생산 시작 시간
    
    [NotMapped]
    public Vector3 PositionVector
    {
        get
        {
            return new Vector3(
                Position[0],
                Position[1],
                Position[2]
            );
        }
        set
        {
            Position[0] = (int)value.X;
            Position[1] = (int)value.Y;
            Position[2] = (int)value.Z;
        }
    }
}