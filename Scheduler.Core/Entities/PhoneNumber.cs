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
            Home,
            Work,
            Spouse,
            Other
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

            // A '+' is only allowed as the first non-whitespace character (e.g. "+1 555 123 4567").
            var trimmed = number.Trim();
            var body = trimmed.StartsWith('+') ? trimmed[1..] : trimmed;

            if (body.Any(c => !char.IsAsciiDigit(c) && !AllowedSeparators.Contains(c)))
                throw new ArgumentException("Phone number contains invalid characters.", nameof(number));

            var digits = new string(body.Where(char.IsAsciiDigit).ToArray());

            // Accept a leading US country code (+1 / 1).
            if (digits.Length == 11 && digits[0] == '1')
                digits = digits[1..];

            if (digits.Length != 10)
                throw new ArgumentException("Phone number must have 10 digits.", nameof(number));

            Number = "+1" + digits;   // E.164 for US numbers
            Type = type;
        }
    }
}
