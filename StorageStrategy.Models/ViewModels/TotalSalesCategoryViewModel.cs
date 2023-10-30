namespace StorageStrategy.Models.ViewModels
{
    public record class TotalSalesCategoryViewModel
    {
        public TotalSalesCategoryViewModel(decimal totalPrice, string nameCategory)
        {
            NameCategory = nameCategory;
            TotalPrice = totalPrice;
        }

        public string NameCategory { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
    }
}
 