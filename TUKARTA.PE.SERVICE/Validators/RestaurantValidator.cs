using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;
using TUKARTA.PE.SERVICE.Resources;

namespace TUKARTA.PE.SERVICE.Validators
{
    public class RestaurantValidator : AbstractValidator<RestaurantResource>
    {
        public RestaurantValidator()
        {
            RuleFor(restaurant => restaurant.Name)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(restaurant => restaurant.RUC)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                .Length(11)
                    .WithMessage(ConstantHelpers.Message.Validation.LENGTH);
            RuleFor(restaurant => restaurant.Email)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED)
                .EmailAddress()
                    .WithMessage(ConstantHelpers.Message.Validation.EMAIL_ADDRESS);
            RuleFor(restaurant => restaurant.PhoneNumber)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            RuleFor(restaurant => restaurant.Latitude)
                .NotEqual(0)
                    .WithMessage(ConstantHelpers.Message.Validation.NOT_EQUAL);
            RuleFor(restaurant => restaurant.Longitude)
                .NotEqual(0)
                    .WithMessage(ConstantHelpers.Message.Validation.NOT_EQUAL);
            RuleFor(restaurant => restaurant.Address)
                .NotEmpty()
                    .WithMessage(ConstantHelpers.Message.Validation.REQUIRED);
            When(restaurant => restaurant.IsDeliveryEnabled, () =>
            {
                RuleFor(restaurant => restaurant.DeliveryCostPerKilometer)
                    .GreaterThan(0)
                        .WithMessage(ConstantHelpers.Message.Validation.GREATER_THAN);
            });
        }
    }
}
