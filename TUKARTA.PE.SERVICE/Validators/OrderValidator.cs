using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class OrderValidator : AbstractValidator<OrderResource>
    {
        public OrderValidator()
        {
            RuleFor(order => order.PeopleAmount)
                .GreaterThanOrEqualTo(1)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
            When(order => order.IssueType == ConstantHelpers.Service.IssueType.TICKET, () =>
            {
                RuleFor(order => order.TicketName)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            });
            When(order => order.IssueType == ConstantHelpers.Service.IssueType.BILL, () =>
            {
                RuleFor(order => order.RUC)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
                RuleFor(order => order.BusinessName)
                    .NotEmpty()
                        .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            });
            RuleFor(order => order.IssueType)
                .InclusiveBetween(1, ConstantHelpers.Service.IssueType.VALUES.Count);
            RuleFor(order => order.ConsumptionModality)
                .InclusiveBetween(1, ConstantHelpers.Service.Order.ConsumptionModality.VALUES.Count);
        }
    }
}
