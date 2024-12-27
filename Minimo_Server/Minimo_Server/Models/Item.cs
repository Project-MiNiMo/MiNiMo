using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Item
{
    [Key]
    public int Id { get; set; }
    
    public string ItemID { get; set; }
    public int Count { get; set; }
}