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

    public enum PixChargeStatusEnum
    {
        [Display(Name = "Criando")]
        Criando = 1,

        [Display(Name = "Aguardando Pagamento")]
        AguardandoPagamento = 2,

        [Display(Name = "Pago")]
        Pago = 3,

        [Display(Name = "Rejeitado")]
        Rejeitado = 4,

        [Display(Name = "Cancelado")]
        Cancelado = 5,

        [Display(Name = "Expirado")]
        Expirado = 6
    }
}
