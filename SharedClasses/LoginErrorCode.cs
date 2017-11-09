namespace SharedClasses
{
    public enum LoginErrorCode
    {
        UnknownError = -1,
        Success = 0,
        UserLockedOut = 1,
        LoginFailed = 2,
        NoUserOrEmailSpecified = 3,
        UserOrEmailNotExists = 4,
        PasswordExpired = 5
    }
}
