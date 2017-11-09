using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses {
    public enum FeedbackEventType {
        None = 0,
        StatusChange = 1,
        StaffMessage = 2,
        PlayerMessage = 3,
		StaffNote = 4,
		TierChange = 5
    }
}
