using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TUKARTA.PE.CORE.Helpers
{
    public class ConstantHelpers
    {
        public static class Seed
        {
            public const bool ENABLED = true;
        }

        public static class TimeZone
        {
            public const string DEFAULT_WINDOWS_ID = "SA Pacific Standard Time";
            public const string DEFAULT_LINUX_ID = "America/New_York";
            public static string DEFAULT_ID => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? DEFAULT_WINDOWS_ID : DEFAULT_LINUX_ID;
        }

        public static class Format
        {
            public const string DATE = "dd/MM/yyyy";
            public const string DURATION = "{0}h {1}m";
            public const string TIME = "h:mm tt";
            public const string DATETIME = "dd/MM/yyyy h:mm tt";
        }

        public static class Geometry
        {
            public const int DEFAULT_COORD_SYSTEM = 4326;
        }

        public static class Roles
        {
            public const string SUPERADMIN = "Superadmin";
            public const string BUSINESS = "Negocio";
            public const string DINER = "Comensal";
        }

        public static class Message
        {
            public static class Error
            {
                public const string MESSAGE = "Ocurrió un problema al procesar su consulta";
                public const string TITLE = "Error";
            }

            public static class Info
            {
                public const string MESSAGE = "Mensaje informativo";
                public const string TITLE = "Info";
            }

            public static class Success
            {
                public const string MESSAGE = "Tarea realizada satisfactoriamente";
                public const string TITLE = "Éxito";
            }

            public static class Validation
            {
                public const string COMPARE = "El campo '{PropertyName}' no coincide con '{ComparisonValue}'.";
                public const string COMPARE_PASSWORD = "El campo debe coincidir con la contraseña.";
                public const string EMAIL_ADDRESS = "El campo '{PropertyName}' no es un correo electrónico válido.";
                public const string CREDIT_CARD = "El campo '{PropertyName}' no es un número de tarjeta válido.";
                public const string EQUAL = "El campo '{PropertyName}' debe tener un valor igual {ComparisonValue}.";
                public const string NOT_EQUAL = "El campo '{PropertyName}' debe tener un valor diferente a {ComparisonValue}.";
                public const string GREATER_THAN = "El campo '{PropertyName}' debe tener un valor mayor a {ComparisonValue}.";
                public const string GREATER_THAN_OR_EQUAL = "El campo '{PropertyName}' debe tener un valor mayor o igual a {ComparisonValue}.";
                public const string LESS_THAN = "El campo '{PropertyName}' debe tener un valor menor a {ComparisonValue}.";
                public const string LESS_THAN_OR_EQUAL = "El campo '{PropertyName}' debe tener un valor menor o igual a {ComparisonValue}.";
                public const string MAX_LENGTH = "El campo '{PropertyName}' debe tener {ComparisonValue} caracteres como máximo. Ingresaste {TotalLength} caracteres.";
                public const string MIN_LENGTH = "El campo '{PropertyName}' debe tener {ComparisonValue} caracteres como mínimo. Ingresaste {TotalLength} caracteres.";
                public const string LENGTH = "El campo '{PropertyName}' debe tener {MinLength} caracteres. Ingresaste {TotalLength} caracteres.";
                public const string LENGTH_RANGE = "El campo '{PropertyName}' debe tener {MinLength}-{MaxLength} caracteres. Ingresaste {TotalLength} caracteres.";
                public const string INCLUSIVE_BETWEEN = "El campo '{PropertyName}' debe tener un valor entre {From}-{To}. Ingresaste {Value}.";
                public const string EXCLUSIVE_BETWEEN = "El campo '{PropertyName}' debe tener un valor entre {From}-{To} (exclusivo). Ingresaste {Value}.";
                public const string REGULAR_EXPRESSION = "El campo '{PropertyName}' no es válido.";
                public const string SCALED_PRECISION = "El campo '{PropertyName}' no debe tener más de {ExpectedPrecision} dígitos ni más de {ExpectedScale} decimales. Ingresaste {Digits} dígitos con {ActualScale} decimales.";
                public const string REQUIRED = "El campo '{PropertyName}' es obligatorio.";
                public const string NOT_VALID = "El campo '{PropertyName}' no es válido.";
                public const string FILE_EXTENSIONS = "El campo '{PropertyName}' solo acepta archivos con los formatos: {1}.";
            }
        }

        public static class Service
        {
            public static class CurrencyType
            {
                public const int NUEVOS_SOLES = 1;
                public const int AMERICAN_DOLLARS = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { NUEVOS_SOLES, "Nuevos Soles" },
                    { AMERICAN_DOLLARS, "Dólares Americanos" },
                };

                public static Dictionary<int, string> SYMBOLS = new Dictionary<int, string>()
                {
                    { NUEVOS_SOLES, "S/" },
                    { AMERICAN_DOLLARS, "$" }
                };
            }

            public static class IssueType
            {
                public const int BILL = 1;
                public const int TICKET = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { BILL, "Factura" },
                    { TICKET, "Boleta" },
                };
            }

            public static class Order
            {
                public static class ConsumptionModality
                {
                    public const int TO_CARRY_OUT = 1;
                    public const int TO_EAT_IN = 2;

                    public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                    {
                        { TO_CARRY_OUT, "Para llevar" },
                        { TO_EAT_IN, "Comer en Restaurante" },
                    };
                }
            }

            public static class PaymentMethod
            {
                public const int CARD = 1;
                public const int EFFECTIVE = 2;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { CARD, "Tarjeta" },
                    { EFFECTIVE, "Efectivo" },
                };
            }

            public static class Status
            {
                public const int PENDING_APPROVAL = 1;
                public const int ACCEPTED = 2;
                public const int REJECTED = 3;
                public const int CANCELLED = 4;
                public const int PAYMENT_IN_PROCESS = 55;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>() 
                {
                    { PENDING_APPROVAL, "Pendiente de Aprobación" },
                    { ACCEPTED, "Aceptado" },
                    { REJECTED, "Rechazado" },
                    { CANCELLED, "Cancelado" },
                    { PAYMENT_IN_PROCESS, "Pago en Proceso" }
                };
            }

            public static class Type
            {
                public const int ORDER = 1;
                public const int BOOKING = 2;
                public const int DELIVERY = 3;

                public static Dictionary<int, string> VALUES = new Dictionary<int, string>()
                {
                    { ORDER, "Orden" },
                    { BOOKING, "Reserva" },
                    { DELIVERY, "Delivery" },
                };
            }
        }
    }
}
