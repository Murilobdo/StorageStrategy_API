using StorageStrategy.Models;
using StorageStrategy.Models.ViewModels;

namespace StorageStrategy.Utils.Helpers
{
    public static class Calc
    {
        public static int CountSalesPayment(List<CommandEntity> commands, PaymentEnum paymentEnum)
        {
            var totalPrice = commands
                .SelectMany(p => p.Payments)
                .Where(p => p.Method == paymentEnum)
                .Count();
            
            return totalPrice; 
        }

        public static bool CommandHasFinishWithTotalPayments(CommandEntity command)
        {
            var _discount = command.Discount;
            var _increase = command.Increase;
            var _totalPaymentsAmount = command.Payments.Sum(p => p.Amount);
            
            return command.TotalPrice + _increase - _discount == _totalPaymentsAmount;
        }

        public static decimal TotalCostForPayment(List<CommandEntity> commands, PaymentEnum paymentEnum)
        {
            decimal totalPrice = commands
                .SelectMany(p => p.Payments)
                .Where(p => p.Method == paymentEnum)
                .Sum(p => p.Amount);

            return totalPrice;
        }
        

        public static List<TotalSalesCategoryViewModel> TotalSalesPerCategory(List<CommandEntity> commands)
        {
            var salesPerCategory = new List<TotalSalesCategoryViewModel>();

            if(commands.Count == 0)
                return new List<TotalSalesCategoryViewModel>();

            var categorys = commands.SelectMany(p => p.Items).Select(p => p.Product.Category.Name).Distinct();
            var itensComman = commands.SelectMany(p => p.Items);

            foreach (var category in categorys)
            {
                decimal totalPrice = itensComman
                    .Where(p => p.Product.Category.Name == category)
                    .Sum(p => p.Product.Price);

                salesPerCategory.Add(new TotalSalesCategoryViewModel(totalPrice, category));
            }

            return salesPerCategory
                .OrderByDescending(p => p.TotalPrice)
                .Take(10)
                .ToList();
        }

        public static decimal TotalDeImpostos(List<CommandEntity> commands)
        {
            var itens = commands.SelectMany(p => p.Items);
            var result = itens.Sum(p => p.Taxing);
            return result;
        }

        public static decimal CustoProdutoVendido(List<CommandEntity> commands)
        {
            var result = commands.Sum(p => p.TotalCost);
            return result;
        }
    }
}
