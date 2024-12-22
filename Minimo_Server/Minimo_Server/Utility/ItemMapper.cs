using System.Collections.Generic;
using System.Linq;
using MinimoServer.Models;
using MinimoServer.Shared;

public static class ItemMapper
{
    // Convert Item to ItemDTO
    public static ItemDTO ToItemDTO(Item item)
    {
        return new ItemDTO
        {
            ItemID = item.ItemID,
            Count = item.Count
        };
    }

    // Convert ItemDTO to Item
    public static Item ToItem(ItemDTO itemDto)
    {
        return new Item
        {
            ItemID = itemDto.ItemID,
            Count = itemDto.Count
        };
    }

    // Convert List of Items to List of ItemDTOs
    public static IEnumerable<ItemDTO> ToItemDTOs(IEnumerable<Item> items)
    {
        return items.Select(ToItemDTO).ToList();
    }

    // Convert List of ItemDTOs to List of Items
    public static IEnumerable<Item> ToItems(IEnumerable<ItemDTO> itemDtos)
    {
        return itemDtos.Select(ToItem).ToList();
    }
}