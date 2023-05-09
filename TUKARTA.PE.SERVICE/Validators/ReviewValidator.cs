using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class ReviewValidator : AbstractValidator<ReviewResource>
    {
        public ReviewValidator()
        {
            RuleFor(review => review.Comment)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(review => review.Score)
                .LessThanOrEqualTo(5)
                    .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN_OR_EQUAL)
                .GreaterThanOrEqualTo(1)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
        }
    }
}
