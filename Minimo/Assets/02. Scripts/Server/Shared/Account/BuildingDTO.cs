using System.Numerics;

namespace MinimoShared
{
    public class BuildingDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsInstalled { get; set; }
        public Vector3 Position { get; set; }
    }
}