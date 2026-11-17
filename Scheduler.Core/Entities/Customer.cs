using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class Customer
    {
        string _id;
        string _firstName;
        string _lastName;
        string _middleName;
        string _email;
        List<PhoneNumber> _phoneNumbers;
        string _address1;
        string _address2;
        string _city;
        string _state;
        string _zip;
    }
}
