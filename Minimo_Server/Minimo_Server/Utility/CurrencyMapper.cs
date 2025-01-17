using MinimoServer.Models;
using MinimoShared;

public static class CurrencyMapper
{
    // Convert Currency to CurrencyDTO
    public static CurrencyDTO ToCurrencyDTO(Currency currency)
    {
        return new CurrencyDTO
        {
            Star = currency.Star,
            BlueStar = currency.BlueStar,
            Heart = currency.Heart,
            HPI = currency.HPI
        };
    }

    // Convert CurrencyDTO to Currency
    public static Currency ToCurrency(CurrencyDTO currencyDto)
    {
        return new Currency
        {
            Star = currencyDto.Star,
            BlueStar = currencyDto.BlueStar,
            Heart = currencyDto.Heart,
            HPI = currencyDto.HPI
        };
    }
}