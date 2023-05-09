using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class UserValidator : AbstractValidator<UserResource>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(user => user.Email)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                .EmailAddress()
                    .WithMessage(ConstantHelpers.Message.Validation.EMAIL_ADDRESS);
            When(user => !user.Id.HasValue && !user.IsExternalLogin, () =>
            {
                RuleFor(user => user.Password)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            });
            RuleFor(user => user.RepeatPassword)
                .Equal(user => user.Password)
                    .WithMessage(ConstantHelpers.Message.Validation.COMPARE_PASSWORD);
        }
    }
}
