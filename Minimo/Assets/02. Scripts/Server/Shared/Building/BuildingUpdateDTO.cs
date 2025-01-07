#nullable enable
using System.Numerics;
using System.Collections.Generic;

namespace MinimoShared
{
    public class UpdateBuildingParameter
    {
        public int Id { get; set; }
        public int[]? Position { get; set; }
        public Vector3 PositionVector => Position == null ? Vector3.Zero : new Vector3(Position[0], Position[1], Position[2]);
    }
}