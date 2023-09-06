using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Utils.Helpers
{
    public static class Calc
    {
        public static decimal TotalPriceForPayment(List<CommandEntity> commands, PaymentEnum paymentEnum)
        {
            decimal totalPrice = commands
                .Where(p => p.Payment.Value == paymentEnum)
                .Sum(p => p.TotalPrice);

            return totalPrice; 
        }

        public static decimal TotalCostForPayment(List<CommandEntity> commands, PaymentEnum paymentEnum)
        {
            decimal totalPrice = commands
                .Where(p => p.Payment.Value == paymentEnum)
                .Sum(p => p.TotalPrice);

            return totalPrice;
        }
    }
}
