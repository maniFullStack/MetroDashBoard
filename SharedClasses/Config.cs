using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace SharedClasses {
    public static class Config {
        /// <summary>
        /// Represents the client ID used for handling logins and other parts of the common login system.
        /// </summary>
        public static int ClientID {
            get {
                return 1;
            }
        }

        public static string CacheFileDirectory {
            get {
                string path = ConfigurationManager.AppSettings["CacheFileDirectory"];
                if ( String.IsNullOrWhiteSpace( path ) ) {
                    return "~/Files/Cache/";
                } else {
                    return path;
                }

            }
        }
        public static string PINFileDirectory {
            get {
                string path = ConfigurationManager.AppSettings["EmailPINFileDirectory"];
                if ( String.IsNullOrWhiteSpace( path ) ) {
                    return "~/Files/PINFiles/";
                } else {
                    return path;
                }

            }
        }
        
    }
}
