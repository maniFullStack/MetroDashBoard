using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteUtilities;

namespace TestingWebsite {
    public class UserInfoDerived : UserInformation {
        /// <summary>
        /// New property on our UserInfoDerived class.
        /// </summary>
        public string TestValue { get; set; }
        
        //Override the Load methods so we can populate our new property.

        protected override void Load(string usernameOrEmail, string password, bool useEmailForLogin, int clientID, out int outputValue) {
            base.Load(usernameOrEmail, password, useEmailForLogin, clientID, out outputValue);
            //Make sure we loaded something successfully
            if (UserID != -1) {
                //Do new things here
                TestValue = "Hello";
            }
        }
        protected override void Load(int userID) {
            base.Load(userID);
            //Make sure we loaded something successfully
            if (UserID != -1) {
                //Do new things here
                TestValue = "Hello";
            }
        }
        protected override void Load(string guid) {
            base.Load(guid);
            //Make sure we loaded something successfully
            if (UserID != -1) {
                //Do new things here
                TestValue = "Hello";
            }
        }
    }
}
