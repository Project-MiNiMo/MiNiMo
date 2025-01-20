using System;
using System.Collections.Generic;

namespace MinimoShared
{
    public class MeteorCreateDTO
    {
        public List<MeteorDTO> CreatedMeteors { get; set; }
        public DateTime LastMeteorCreatedAt { get; set; }
    }
}