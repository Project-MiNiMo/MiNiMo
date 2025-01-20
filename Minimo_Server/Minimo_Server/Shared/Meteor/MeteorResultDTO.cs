using System;

namespace MinimoShared
{
    public class MeteorResultDTO
    {
        public int RemovedMeteorId { get; set; }
        public ItemDTO? ResultItem { get; set; }
        public QuestDTO? ResultQuest { get; set; }
        public DateTime LastMeteorCreatedAt { get; set; }
    }
}