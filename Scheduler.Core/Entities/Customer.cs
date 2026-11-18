using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class Customer
    {
        public string Id { get; private set; } = "";
        public string FirstName { get; private set; } = "";
        public string? MiddleName { get; private set; }
        public string LastName { get; private set; } = "";
        public string Email { get; private set; } = "";
        public List<PhoneNumber> PhoneNumbers { get; private set; } = new();
        public string Address1 { get; private set; } = "";
        public string? Address2 { get; private set; }
        public string City { get; private set; } = "";
        public string State { get; private set; } = "";
        public string Zip { get; private set; } = "";

        private Customer() { }

        public Customer(
            string id,
            string firstName,
            string lastName,
            string email,
            string address1,
            string city,
            string state,
            string zip,
            string? middleName = null,
            string? address2 = null,
            IEnumerable<PhoneNumber>? phoneNumbers = null)
        {
            // Validate FirstName, LastName, Email
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.", nameof(email));

            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format.", nameof(email));

            // Validate PhoneNumbers are valid if flow has gotten this far

            Id = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            Email = email;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zip = zip;
            PhoneNumbers = phoneNumbers?.ToList() ?? new();
        }

        public static bool IsValidEmail(string email) =>
            MailAddress.TryCreate(email, out var address) && address.Address == email;
    }
}
