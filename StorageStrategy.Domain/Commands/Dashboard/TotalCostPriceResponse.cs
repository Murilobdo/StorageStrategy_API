namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class TotalCostPriceResponse
    {
        public int Day { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalCost { get; set; }

        public TotalCostPriceResponse()
        {
        }

        public TotalCostPriceResponse(int day, decimal totalPrice, decimal totalCost)
        {
            Day = day;
            TotalPrice = totalPrice;
            TotalCost = totalCost;
        }
    }
}
