using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class DeliveryValidator : AbstractValidator<DeliveryResource>
    {
        public DeliveryValidator()
        {
            When(delivery => delivery.IssueType == ConstantHelpers.Service.IssueType.TICKET, () =>
            {
                RuleFor(delivery => delivery.TicketName)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            });
            When(delivery => delivery.IssueType == ConstantHelpers.Service.IssueType.BILL, () =>
            {
                RuleFor(delivery => delivery.RUC)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
                RuleFor(delivery => delivery.BusinessName)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            });
            RuleFor(delivery => delivery.IssueType)
                .InclusiveBetween(1, ConstantHelpers.Service.IssueType.VALUES.Count);
            RuleFor(delivery => delivery.PaymentMethod)
                .InclusiveBetween(1, ConstantHelpers.Service.PaymentMethod.VALUES.Count);
            When(delivery => delivery.PaymentMethod == ConstantHelpers.Service.PaymentMethod.EFFECTIVE, () =>
            {
                RuleFor(delivery => delivery.PaymentAmount)
                    .NotNull()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                    .GreaterThan(0)
                        .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
            });
            RuleFor(delivery => delivery.Address)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
        }
    }
}
