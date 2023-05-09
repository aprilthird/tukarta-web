using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class PurchasePaymentValidator : AbstractValidator<PurchasePaymentResource>
    {
        public PurchasePaymentValidator()
        {
            RuleFor(purchasePayment => purchasePayment.TransactionId)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(purchasePayment => purchasePayment.TransactionTraceNumber)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(purchasePayment => purchasePayment.Currency)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
        }
    }
}
