using System.ComponentModel.DataAnnotations;

namespace StorageStrategy.Models
{
    public enum PaymentEnum
    {
        [Display(Name = "Débito")]
        Debit = 1,
        
        [Display(Name = "Crédito")]
        Credit = 2,
        
        [Display(Name = "Pix")]
        Pix = 3,

        [Display(Name = "Dinheiro")]
        Cash = 4,
    }

    public enum EmployeeRole
    {
        [Display(Name = "Admin")]
        Admin = 7,

        [Display(Name = "Gerente")]
        Manager = 1,

        [Display(Name = "Funcionário")]
        Employee = 2
    }
}
