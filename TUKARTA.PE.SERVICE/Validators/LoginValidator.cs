using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class LoginValidator : AbstractValidator<LoginResource>
    {
        public LoginValidator()
        {
            RuleFor(login => login.Email)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                .EmailAddress()
                    .WithMessage(ConstantHelpers.Message.Validation.EMAIL_ADDRESS);
            RuleFor(login => login.Password)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
        }
    }
}
