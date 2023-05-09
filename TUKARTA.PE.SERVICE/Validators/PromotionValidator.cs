using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class PromotionValidator : AbstractValidator<PromotionResource>
    {
        public PromotionValidator()
        {
            RuleFor(dish => dish.NewPrice)
                .GreaterThan(0)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
        }
    }
}
