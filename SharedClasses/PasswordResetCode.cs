using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedClasses
{
    public enum  PasswordResetCode
    {

        Success = 0,
        UserNotFound = 2,
        UserIdNotProvided = 1,
        CriticalError = 3,
        Other = -1,
        InvalidEmail = 4,
        GuidUpdateFailure = 5
    }
}
