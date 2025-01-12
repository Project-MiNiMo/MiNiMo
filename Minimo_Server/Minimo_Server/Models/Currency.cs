using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Currency
{
    public int Star { get; set; }
    public int BlueStar { get; set; }
    public int Heart { get; set; }
    
    // 생성자
    public Currency() { } // EF Core용

    public Currency(int star, int blueStar, int heart)
    {
        if (star < 0 || blueStar < 0 || heart < 0)
            throw new ArgumentException("Currency values cannot be negative");

        Star = star;
        BlueStar = blueStar;
        Heart = heart;
    }

    // 화폐 증가 메서드
    public void Add(int star, int blueStar, int heart)
    {
        Star += star;
        BlueStar += blueStar;
        Heart += heart;
    }
}