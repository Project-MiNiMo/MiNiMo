namespace MinimoServer.Models;

public class Meteor
{
    public int Id { get; set; }
    public int MeteorType { get; set; }
    public int ValueIndex { get; set; }
    public int ValueCount { get; set; } // 자원일 경우 개수
}