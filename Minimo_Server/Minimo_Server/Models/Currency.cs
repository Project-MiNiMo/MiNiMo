using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Currency
{
    [Key]
    public int Id { get; set; }
    
    public int Star { get; set; }
    public int BlueStar { get; set; }
    public int Heart { get; set; }
}