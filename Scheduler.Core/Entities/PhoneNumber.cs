using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class PhoneNumber
    {
        public enum PhoneType
        {
            Mobile,
            Landline,
            Work,
            Fax
        }

        public string Number { get; private set; } = "";
        public PhoneType Type { get; private set; }

        private PhoneNumber() { }

        private const string AllowedSeparators = " ()-.";

        public PhoneNumber(string number, PhoneType type)
        {
            ArgumentNullException.ThrowIfNull(number);

            if (!Enum.IsDefined(type))
                throw new ArgumentOutOfRangeException(nameof(type), type, "Invalid phone type.");

            if (!TryNormalize(number, out var digits, out var error))
                throw new ArgumentException(error, nameof(number));

            Number = "+1" + digits;   // E.164 for US numbers
            Type = type;
        }

        public static bool TryNormalize(string? number, out string digits, out string? error)
        {
            error = Validate(number, out digits);
            return error is null;
        }

        // Returns null when valid, otherwise the reason it isn't.
        private static string? Validate(string? number, out string digits)
        {
            digits = string.Empty;
            if (number is null)
                return "Phone number is required.";

            // A '+' is only allowed as the first non-whitespace character (e.g. "+1 555 123 4567").
            var trimmed = number.Trim();
            var body = trimmed.StartsWith('+') ? trimmed[1..] : trimmed;

            if (body.Any(c => !char.IsAsciiDigit(c) && !AllowedSeparators.Contains(c)))
                return "Phone number contains invalid characters.";

            var d = new string(body.Where(char.IsAsciiDigit).ToArray());

            // Accept a leading US country code (+1 / 1).
            if (d.Length == 11 && d[0] == '1')
                d = d[1..];

            if (d.Length != 10)
                return "Phone number must have 10 digits.";

            digits = d;
            return null;
        }
    }
}
