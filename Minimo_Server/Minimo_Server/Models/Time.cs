using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Time
{
    [Key]
    public int Id { get; set; }
    public TimeSpan Offset { get; set; }
}