using System;
using System.Collections.Generic;
using System.Text;
using TUKARTA.PE.CORE.Helpers;

namespace TUKARTA.PE.SERVICE.Managers
{
    public class CustomLanguageManager : FluentValidation.Resources.LanguageManager
    {
        public CustomLanguageManager()
        {
            AddTranslation("es", "NotEmptyValidator", ConstantHelpers.Message.Validation.REQUIRED);
            AddTranslation("es", "EmailAddressValidator", ConstantHelpers.Message.Validation.EMAIL_ADDRESS);
            AddTranslation("es", "EqualValidator", ConstantHelpers.Message.Validation.EQUAL);
            AddTranslation("es", "NotEqualValidator", ConstantHelpers.Message.Validation.NOT_EQUAL);
            AddTranslation("es", "GreaterThanValidator", ConstantHelpers.Message.Validation.GREATER_THAN);
            AddTranslation("es", "GreaterThanOrEqualValidator", ConstantHelpers.Message.Validation.GREATER_THAN_OR_EQUAL);
            AddTranslation("es", "LessThanValidator", ConstantHelpers.Message.Validation.LESS_THAN);
            AddTranslation("es", "LessThanOrEqualValidator", ConstantHelpers.Message.Validation.LESS_THAN_OR_EQUAL);
            AddTranslation("es", "LengthValidator", ConstantHelpers.Message.Validation.LENGTH_RANGE);
            AddTranslation("es", "ExactLengthValidator", ConstantHelpers.Message.Validation.LENGTH);
            AddTranslation("es", "MaximumLengthValidator", ConstantHelpers.Message.Validation.MAX_LENGTH);
            AddTranslation("es", "MinimumLengthValidator", ConstantHelpers.Message.Validation.MIN_LENGTH);

        }
    }
}
