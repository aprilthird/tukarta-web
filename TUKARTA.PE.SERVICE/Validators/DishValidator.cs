using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class DishValidator : AbstractValidator<DishResource>
    {
        public DishValidator()
        {
            RuleFor(dish => dish.Name)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(dish => dish.Price)
                .GreaterThan(0)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
        }
    }
}
