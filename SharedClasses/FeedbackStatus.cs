using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses {
    public enum FeedbackStatus {
        None = 0,
        Open = 1,
        AwaitingGuestResponse = 2,
        ClosedGuestResponseComplete = 3,
        ClosedNoFurtherActionRequired = 4,
        ClosedUnabletoSatisfyGuest = 5,
        ClosedNoResponse = 6
    }
}
