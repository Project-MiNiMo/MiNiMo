using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

// Player Entity
public class Account
{
    [Key]
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public string Nickname { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Level { get; set; }
    public int Experience { get; set; }
    public DateTime LastLogin { get; set; }
    
    // 건물
    public List<Building> Buildings { get; set; }
    
    // 
}