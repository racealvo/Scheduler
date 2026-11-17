using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class User
    {
        string _id;
        string _email;
        string _roleId;   // Role ID of the user, e.g. "Admin", "Therapist", "Customer"
        string _customerId;   // Customer ID of the user, if applicable
    }
}
