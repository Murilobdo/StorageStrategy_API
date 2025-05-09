﻿using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class FinishCommandCommand : CommandBase, IValidation
    {
        public FinishCommandCommand(
            int commandId, 
            PaymentCommand payment, 
            int companyId,
            decimal discount,
            decimal increase
        ) {
            CommandId = commandId;
            Payments = new List<PaymentCommand>{payment} ;
            CompanyId = companyId;
            Discount = discount;
            Increase = increase;
        }

        public FinishCommandCommand()
        {
            
        }

        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public decimal Discount { get; set; }
        public decimal Increase { get; set; }
        public List<PaymentCommand> Payments { get; set; }

        public List<Error> GetErros() => new FinishCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        public bool IsValid() => new FinishCommandValidation().Validate(this).IsValid;
    }
}
