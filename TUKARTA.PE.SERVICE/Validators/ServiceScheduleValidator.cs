using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class ServiceScheduleValidator : AbstractValidator<ServiceScheduleResource>
    {
        public ServiceScheduleValidator()
        {
            RuleFor(serviceSchedule => serviceSchedule.Day)
                .GreaterThanOrEqualTo(0)
                    .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL)
                .LessThanOrEqualTo(6)
                    .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN_OR_EQUAL);
            When(serviceSchedule => !serviceSchedule.IsClosed, () =>
            {
                RuleFor(serviceSchedule => serviceSchedule.OpeningTime.TimeOfDay)
                    .LessThan(serviceSchedule => serviceSchedule.ClosingTime.TimeOfDay)
                        .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN);
                RuleFor(serviceSchedule => serviceSchedule.ClosingTime.TimeOfDay)
                    .GreaterThan(serviceSchedule => serviceSchedule.OpeningTime.TimeOfDay)
                        .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
                When(serviceSchedule => serviceSchedule.SecondOpeningTime.HasValue, () =>
                {
                    RuleFor(serviceSchedule => serviceSchedule.SecondOpeningTime.Value.TimeOfDay)
                        .GreaterThan(serviceSchedule => serviceSchedule.ClosingTime.TimeOfDay)
                            .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
                    RuleFor(serviceSchedule => serviceSchedule.SecondClosingTime)
                        .NotNull()
                            .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
                    RuleFor(serviceSchedule => serviceSchedule.ClosingTime.TimeOfDay)
                        .LessThan(serviceSchedule => serviceSchedule.SecondOpeningTime.Value.TimeOfDay)
                            .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN);
                    When(serviceSchedule => serviceSchedule.SecondClosingTime.HasValue, () =>
                    {
                        RuleFor(serviceSchedule => serviceSchedule.SecondOpeningTime.Value.TimeOfDay)
                            .LessThan(serviceSchedule => serviceSchedule.SecondClosingTime.Value.TimeOfDay)
                                .WithMessage(ConstantHelpers.Message.Validation.LESS_THAN);
                    });
                });
                When(serviceSchedule => serviceSchedule.SecondClosingTime.HasValue, () =>
                {
                    RuleFor(serviceSchedule => serviceSchedule.SecondOpeningTime)
                        .NotNull()
                            .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
                    When(serviceSchedule => serviceSchedule.SecondOpeningTime.HasValue, () =>
                    {
                        RuleFor(serviceSchedule => serviceSchedule.SecondClosingTime.Value.TimeOfDay)
                            .GreaterThan(serviceSchedule => serviceSchedule.SecondOpeningTime.Value.TimeOfDay)
                                .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
                    });
                });
            });
        }
    }
}
