using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Core.Entities
{
    public class ReminderType
    {
        string _id;
        string _name;   // e.g. "Info14Day", "Confirm7Day", "FinalDay"
        int _channel;   // e.g. 1 = Email, 2 = SMS, 3 = Phone Call
        int _status;    // e.g. 1 = Pending, 2 = Sent, 3 = Failed
        DateTime _scheduledAt;   // Date and time when the reminder is scheduled to be sent
        DateTime _sentAt;   // Date and time when the reminder was sent
    }
}
