using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Item
{
    [Key]
    public string ItemType { get; set; }
    public int Count { get; set; }
}