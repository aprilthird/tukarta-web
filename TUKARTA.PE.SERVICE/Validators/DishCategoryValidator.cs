using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class DishCategoryValidator : AbstractValidator<DishCategoryResource>
    {
        public DishCategoryValidator()
        {
            RuleFor(dishCategory => dishCategory.Name)
                .NotEmpty().WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
        }
    }
}
