namespace MinimoShared
{
    public class CurrencyUpdateDTO
    {
        public CurrencyDTO ObtainedCurrency { get; set; }
        public CurrencyDTO CurrentCurrency { get; set; }
        public string UpdateReason { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}