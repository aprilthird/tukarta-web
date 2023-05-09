using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class PurchaseReportParametersValidator : AbstractValidator<PurchaseReportParametersResource>
    {
        public PurchaseReportParametersValidator()
        {
            When(purchaseReportParameters => purchaseReportParameters.StartDateTime.HasValue && purchaseReportParameters.EndDateTime.HasValue, () =>
            {
                RuleFor(purchaseReportParameters => purchaseReportParameters.StartDateTime)
                    .LessThanOrEqualTo(purchaseReportParameters => purchaseReportParameters.EndDateTime)
                        .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN_OR_EQUAL);
                RuleFor(purchaseReportParameters => purchaseReportParameters.EndDateTime)
                    .GreaterThanOrEqualTo(purchaseReportParameters => purchaseReportParameters.StartDateTime)
                        .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
            });
        }
    }
}
