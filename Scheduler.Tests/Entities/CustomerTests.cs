using Scheduler.Core.Entities;
using static Scheduler.Core.Entities.PhoneNumber;

namespace Scheduler.Tests.Entities
{
    public class CustomerTests
    {
        private const string Id = "cust-1";
        private const string FirstName = "Jane";
        private const string LastName = "Doe";
        private const string Email = "jane.doe@example.com";
        private const string Address1 = "123 Main St";
        private const string City = "Springfield";
        private const string State = "IL";
        private const string Zip = "62701";

        private static Customer CreateCustomer(
            string firstName = FirstName,
            string lastName = LastName,
            string email = Email,
            string? middleName = null,
            string? address2 = null,
            IEnumerable<PhoneNumber>? phoneNumbers = null) =>
            new(Id, firstName, lastName, email, Address1, City, State, Zip,
                middleName, address2, phoneNumbers);

        // ---------- Valid input ----------

        [Fact]
        public void Constructor_RequiredValues_AreStored()
        {
            var customer = CreateCustomer();

            Assert.Equal(Id, customer.Id);
            Assert.Equal(FirstName, customer.FirstName);
            Assert.Equal(LastName, customer.LastName);
            Assert.Equal(Email, customer.Email);
            Assert.Equal(Address1, customer.Address1);
            Assert.Equal(City, customer.City);
            Assert.Equal(State, customer.State);
            Assert.Equal(Zip, customer.Zip);
        }

        [Fact]
        public void Constructor_OptionalValuesOmitted_DefaultToNullAndEmptyList()
        {
            var customer = CreateCustomer();

            Assert.Null(customer.MiddleName);
            Assert.Null(customer.Address2);
            Assert.NotNull(customer.PhoneNumbers);
            Assert.Empty(customer.PhoneNumbers);
        }

        [Fact]
        public void Constructor_OptionalValuesProvided_AreStored()
        {
            var phones = new[]
            {
                new PhoneNumber("5551234567", PhoneType.Mobile),
                new PhoneNumber("5559876543", PhoneType.Work)
            };

            var customer = CreateCustomer(middleName: "Q", address2: "Apt 4", phoneNumbers: phones);

            Assert.Equal("Q", customer.MiddleName);
            Assert.Equal("Apt 4", customer.Address2);
            Assert.Equal(phones, customer.PhoneNumbers);
        }

        [Fact]
        public void Constructor_PhoneNumbers_AreCopiedNotShared()
        {
            var phones = new List<PhoneNumber> { new("5551234567", PhoneType.Mobile) };

            var customer = CreateCustomer(phoneNumbers: phones);
            phones.Add(new PhoneNumber("5559876543", PhoneType.Work));

            Assert.Equal(2, phones.Count);              // original list did change
            Assert.Single(customer.PhoneNumbers);       // customer's copy did not
            Assert.NotSame(phones, customer.PhoneNumbers);
        }

        // ---------- Required names ----------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_MissingFirstName_Throws(string? firstName)
        {
            var ex = Assert.Throws<ArgumentException>(() => CreateCustomer(firstName: firstName!));

            Assert.Equal("firstName", ex.ParamName);
            Assert.StartsWith("First name is required.", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_MissingLastName_Throws(string? lastName)
        {
            var ex = Assert.Throws<ArgumentException>(() => CreateCustomer(lastName: lastName!));

            Assert.Equal("lastName", ex.ParamName);
            Assert.StartsWith("Last name is required.", ex.Message);
        }

        // ---------- Email ----------

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Constructor_MissingEmail_Throws(string? email)
        {
            var ex = Assert.Throws<ArgumentException>(() => CreateCustomer(email: email!));

            Assert.Equal("email", ex.ParamName);
            Assert.StartsWith("Email is required.", ex.Message);
        }

        [Theory]
        [InlineData("jane@example.com")]
        [InlineData("jane.doe+tag@example.co.uk")]
        [InlineData("j_doe-99@sub.example.org")]
        public void Constructor_ValidEmail_IsStored(string email)
        {
            var customer = CreateCustomer(email: email);

            Assert.Equal(email, customer.Email);
        }

        [Theory]
        [InlineData("not-an-email")]
        [InlineData("jane@")]
        [InlineData("@example.com")]
        [InlineData("jane@@example.com")]
        [InlineData("jane doe@example.com")]
        [InlineData("Jane Doe <jane@example.com>")] // display-name form is rejected
        [InlineData(" jane@example.com ")]          // surrounding whitespace is rejected
        public void Constructor_InvalidEmail_Throws(string email)
        {
            var ex = Assert.Throws<ArgumentException>(() => CreateCustomer(email: email));

            Assert.Equal("email", ex.ParamName);
            Assert.StartsWith("Invalid email format.", ex.Message);
        }

        // ---------- Validation order ----------

        [Fact]
        public void Constructor_MultipleInvalidValues_ReportsFirstNameFirst()
        {
            var ex = Assert.Throws<ArgumentException>(
                () => CreateCustomer(firstName: "", lastName: "", email: "bad"));

            Assert.Equal("firstName", ex.ParamName);
        }
    }
}
