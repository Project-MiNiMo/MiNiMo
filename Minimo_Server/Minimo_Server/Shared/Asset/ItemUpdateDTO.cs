using System;

namespace MinimoShared
{
    public class ItemUpdateDTO
    {
        public ItemDTO ObtainedItem { get; set; }
        public ItemDTO CurrentItem { get; set; }
        public string UpdateReason { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}