using System.ComponentModel.DataAnnotations;

namespace MinimoServer.Models;

public class Currency
{
    public int Star { get; set; }
    public int BlueStar { get; set; }
    public int Heart { get; set; }
    public int HPI { get; set; } = 0; // 행복도
    
    // 생성자
    public Currency() { } // EF Core용

    public Currency(int star, int blueStar, int heart, int HPI = 0)
    {
        if (star < 0 || blueStar < 0 || heart < 0 || HPI < 0)
            throw new ArgumentException("Currency values cannot be negative");

        Star = star;
        BlueStar = blueStar;
        Heart = heart;
        HPI = HPI;
    }

    // 화폐 증가 메서드
    public void Add(int star, int blueStar, int heart, int HPI)
    {
        Star += star;
        BlueStar += blueStar;
        Heart += heart;
        HPI += HPI;
    }
}