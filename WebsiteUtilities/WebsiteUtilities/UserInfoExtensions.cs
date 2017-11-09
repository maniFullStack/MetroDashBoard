namespace WebsiteUtilities
{
    /// <summary>
    /// UserInformation Extension Methods 
    /// </summary>
    public static class UserInfoExtensions
    {

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ui"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static int UpdatePassword<T>(this T ui, string oldPass, string newPass)
            where T : UserInformation, new()
        {
            return UserInformation.UpdatePassword(ui, oldPass, newPass);
        }

        /// <summary>
        /// Is User in specified Group 
        /// </summary>
        /// <param name="user">UserInformation Object</param>
        /// <param name="groupId">The GroupId to check if user a part of.</param>
        /// <returns><see cref="bool"/>True if in group false if not</returns>
        public static bool IsInGroup(this UserInformation user, int groupId)
        {
            return user != null && UserInformation.IsInGroup(user.UserID, groupId);
        }

        /// <summary>
        /// Toggle the active status of this user.
        /// </summary>
        /// <param name="user">UserInformation Object</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T of updated user</returns> 
        public static T ToggleActiveStatus<T>(this T user)
            where T : UserInformation, new()
        {
            int error;
            return UserInformation.ToggleActiveUserStatus<T>(user.UserID, null, out error);
        }

        /// <summary>
        /// Toggle the active status of this user.
        /// </summary>
        /// <param name="user">UserInformation Object</param>
        /// <param name="active">Force this value as status.</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>T of updated user</returns>
        public static T ToggleActiveStatus<T>(this T user, bool active)
            where T : UserInformation, new()
        {
            int error;
            return UserInformation.ToggleActiveUserStatus<T>(user.UserID, active, out error);
        }

    }
}