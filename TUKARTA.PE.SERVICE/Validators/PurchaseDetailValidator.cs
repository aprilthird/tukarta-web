using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class PurchaseDetailValidator : AbstractValidator<PurchaseDetailResource>
    {
        public PurchaseDetailValidator()
        {
            RuleFor(purchaseDetail => purchaseDetail.UnitPrice)
                .GreaterThan(0)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
            RuleFor(purchaseDetail => purchaseDetail.Description)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(purchaseDetail => purchaseDetail.Quantity)
                .GreaterThanOrEqualTo(1)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
            When(purchaseDetail => purchaseDetail.PromotionId.HasValue, () =>
            {
                RuleFor(purchaseDetail => purchaseDetail.OriginalPrice)
                    .GreaterThan(0)
                        .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
            });
        }
    }
}
