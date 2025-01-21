using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MinimoServer.Models;

public enum ProduceSlotStatus
{
    Idle,       // 생산되지 않은 상태
    Producing,  // 생산 중
    Completed   // 생산 완료 (수확 대기)
}

public class Building
{
    public int Id { get; set; }

    [Required]
    public string Type { get; set; } // 건물 이름

    public int Level { get; set; } // 건물 레벨

    public DateTime CreatedAt { get; set; } // 건물 생성일
    public DateTime ActivatedAt { get; set; } // 건물 활성화 시점
    
    public int[] Position { get; set; } = new int[3];

    // TODO 아.. 이건 리팩토링..해야해.... //화이팅!! - 다은
    public ProduceSlotStatus[] ProduceStatus { get; set; } = new ProduceSlotStatus[5];// 생산 상태

    public int[] Recipes { get; set; } = new int[5];
    public DateTime[] ProduceStartAt { get; set; } = new DateTime[5]; // 생산 시작 시간
    public DateTime[] ProduceEndAt { get; set; } = new DateTime[5]; // 생산 완료 시간
    
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