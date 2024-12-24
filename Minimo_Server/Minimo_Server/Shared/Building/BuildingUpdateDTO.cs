using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace MinimoShared;

public class UpdateBuildingParameter
{
    [Required]
    public int Id { get; set; }
    public bool? IsInstalled { get; set; }
    public Vector3? Position { get; set; }
}