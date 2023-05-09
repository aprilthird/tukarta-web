using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class ContactValidator : AbstractValidator<ContactResource>
    {
        public ContactValidator()
        {
            RuleFor(contact => contact.ResponsibleName)
                .NotEmpty().WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(contact => contact.BusinessName)
                .NotEmpty().WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(contact => contact.Email)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                .EmailAddress()
                    .WithMessage(ConstantHelpers.Message.Validation.EMAIL_ADDRESS);
            RuleFor(contact => contact.PhoneNumber)
                .NotEmpty().WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
        }
    }
}
