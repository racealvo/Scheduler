using Scheduler.Core.Entities;
using static Scheduler.Core.Entities.PhoneNumber;

namespace Scheduler.Tests.Entities
{
    public class PhoneNumberTests
    {
        // ---------- Valid input ----------

        [Theory]
        [InlineData("5551234567")]
        [InlineData("(555) 123-4567")]
        [InlineData("555-123-4567")]
        [InlineData("555.123.4567")]
        [InlineData("555 123 4567")]
        [InlineData("  555-123-4567  ")]
        public void Constructor_ValidFormats_NormalizesToE164(string input)
        {
            var phone = new PhoneNumber(input, PhoneType.Mobile);

            Assert.Equal("+15551234567", phone.Number);
        }

        [Theory]
        [InlineData("+15551234567")]
        [InlineData("+1 555 123 4567")]
        [InlineData("+1 (555) 123-4567")]
        [InlineData("1-555-123-4567")]
        [InlineData("15551234567")]
        public void Constructor_LeadingCountryCode_IsAccepted(string input)
        {
            var phone = new PhoneNumber(input, PhoneType.Landline);

            Assert.Equal("+15551234567", phone.Number);
        }

        [Theory]
        [InlineData(PhoneType.Mobile)]
        [InlineData(PhoneType.Landline)]
        [InlineData(PhoneType.Work)]
        [InlineData(PhoneType.Fax)]
        public void Constructor_ValidType_IsStored(PhoneType type)
        {
            var phone = new PhoneNumber("5551234567", type);

            Assert.Equal(type, phone.Type);
        }

        // ---------- Wrong type ----------

        [Theory]
        [InlineData(-1)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(99)]
        public void Constructor_UndefinedType_Throws(int rawType)
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(
                () => new PhoneNumber("5551234567", (PhoneType)rawType));

            Assert.Equal("type", ex.ParamName);
        }

        // ---------- Non-numeric ----------

        [Theory]
        [InlineData("abcdefghij")]
        [InlineData("555a123b4567")]
        [InlineData("555-CALL-NOW")]
        [InlineData("555_123_4567")]
        [InlineData("555/123/4567")]
        [InlineData("555-123-4567 x12")]
        [InlineData("555+1234567")]
        [InlineData("++15551234567")]
        [InlineData("٥٥٥١٢٣٤٥٦٧")] // non-ASCII (Arabic-Indic) digits
        public void Constructor_InvalidCharacters_Throws(string input)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PhoneNumber(input, PhoneType.Mobile));

            Assert.Equal("number", ex.ParamName);
        }

        // ---------- Too long / too short ----------

        [Theory]
        [InlineData("55512345678")]      // 11 digits, not starting with 1
        [InlineData("+25551234567")]     // non-US country code
        [InlineData("155512345678")]     // 12 digits
        [InlineData("+1 555 123 45678")] // +1 followed by 11 digits
        public void Constructor_TooManyDigits_Throws(string input)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PhoneNumber(input, PhoneType.Mobile));

            Assert.Equal("number", ex.ParamName);
        }

        [Theory]
        [InlineData("555123456")]   // 9 digits
        [InlineData("555-1234")]    // 7 digits
        [InlineData("+1 555 1234")] // +1 followed by 7 digits
        public void Constructor_TooFewDigits_Throws(string input)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PhoneNumber(input, PhoneType.Mobile));

            Assert.Equal("number", ex.ParamName);
        }

        // ---------- No number ----------

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("+")]
        [InlineData("() - .")]
        public void Constructor_NoDigits_Throws(string input)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PhoneNumber(input, PhoneType.Mobile));

            Assert.Equal("number", ex.ParamName);
        }

        [Fact]
        public void Constructor_NullNumber_ThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new PhoneNumber(null!, PhoneType.Mobile));

            Assert.Equal("number", ex.ParamName);
        }

        // ---------- Constructor error messages ----------

        [Theory]
        [InlineData("555a123b4567", "Phone number contains invalid characters.")]
        [InlineData("555+1234567", "Phone number contains invalid characters.")]
        [InlineData("555123456", "Phone number must have 10 digits.")]
        [InlineData("55512345678", "Phone number must have 10 digits.")]
        [InlineData("", "Phone number must have 10 digits.")]
        public void Constructor_InvalidNumber_ThrowsWithSpecificMessage(string input, string expectedMessage)
        {
            var ex = Assert.Throws<ArgumentException>(
                () => new PhoneNumber(input, PhoneType.Mobile));

            // ArgumentException appends " (Parameter 'number')" to Message.
            Assert.StartsWith(expectedMessage, ex.Message);
        }

        // ---------- TryNormalize ----------

        [Theory]
        [InlineData("5551234567")]
        [InlineData("(555) 123-4567")]
        [InlineData("+1 555 123 4567")]
        [InlineData("1-555-123-4567")]
        public void TryNormalize_ValidInput_ReturnsTrueWithDigitsAndNoError(string input)
        {
            var result = PhoneNumber.TryNormalize(input, out var digits, out var error);

            Assert.True(result);
            Assert.Equal("5551234567", digits);
            Assert.Null(error);
        }

        [Theory]
        [InlineData("555a123b4567", "Phone number contains invalid characters.")]
        [InlineData("555-123-4567 x12", "Phone number contains invalid characters.")]
        [InlineData("555123456", "Phone number must have 10 digits.")]
        [InlineData("155512345678", "Phone number must have 10 digits.")]
        [InlineData("   ", "Phone number must have 10 digits.")]
        public void TryNormalize_InvalidInput_ReturnsFalseWithEmptyDigitsAndError(string input, string expectedError)
        {
            var result = PhoneNumber.TryNormalize(input, out var digits, out var error);

            Assert.False(result);
            Assert.Equal(string.Empty, digits);
            Assert.Equal(expectedError, error);
        }

        [Fact]
        public void TryNormalize_Null_ReturnsFalseWithoutThrowing()
        {
            var result = PhoneNumber.TryNormalize(null, out var digits, out var error);

            Assert.False(result);
            Assert.Equal(string.Empty, digits);
            Assert.Equal("Phone number is required.", error);
        }
    }
}
