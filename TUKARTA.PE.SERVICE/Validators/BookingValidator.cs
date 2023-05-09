using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class BookingValidator : AbstractValidator<BookingResource>
    {
        public BookingValidator()
        {
            RuleFor(booking => booking.PeopleAmount)
                .GreaterThanOrEqualTo(1)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
        }
    }
}
