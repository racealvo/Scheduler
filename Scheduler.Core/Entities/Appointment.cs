using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class Appointment
    {
        string _id;
        string _customerId;
        string _therapistId;
        DateTime _startDateTime;
        DateTime _endDateTime;
        Status _status;
        User _bookedBy;   // User ID of the person who booked the appointment
    }
}
