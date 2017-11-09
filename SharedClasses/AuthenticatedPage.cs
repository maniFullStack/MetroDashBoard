using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using WebsiteUtilities;

namespace SharedClasses {
    public class AuthenticatedPage : AuthenticatedPage<UserInfo> {
        List<UserGroups> _groups = new List<UserGroups>();

        /// <summary>
        /// If true, the User's property value will be matched with the current page's value to ensure they're allowed to view the page. This only happens for Property Maagers and Property Staff.
        /// </summary>
        public bool CheckUserProperty { get; set; }
        
        /// <summary>
        /// Sets the allowed user groups for this page based on a comma separated string.
        /// </summary>
        /// <param name="groupString"></param>
        public void SetGroups( string groupString ) {
            string[] vals = groupString.Split( ',' );
            _groups.Clear();
            foreach ( string s in vals ) {
                UserGroups grp;
                if ( Enum.TryParse<UserGroups>( s, out grp ) ) {
                    _groups.Add( grp );
                }
            }
        }

        /// <summary>
        /// Sets the allowed groups for this page via a comma separated string.
        /// </summary>
        public string AllowedGroups {
            set {
                SetGroups( value );
            }
        }

        /// <summary>
        /// Returns true if the user is within the groups for this page.
        /// </summary>
        public bool IsUsersGroupValid {
            get {
                return User != null && _groups.Contains( User.Group );
            }
        }

        /// <summary>
        /// Gets the property short code for the current request.
        /// </summary>
        public GCCPropertyShortCode PropertyShortCode {
			get {
				if ( ForceSpecificProperty != GCCPropertyShortCode.None ) {
					return ForceSpecificProperty;
				}
                object property = Page.RouteData.Values["propertyshortcode"];
                if ( property != null ) {
                    GCCPropertyShortCode sc;
                    if ( Enum.TryParse<GCCPropertyShortCode>( property.ToString().ToUpper(), out sc ) ) {
                        return sc;
                    }
                    return GCCPropertyShortCode.GCC;
                } else {
                    return GCCPropertyShortCode.GCC;
                }
            }
        }

		GCCPropertyShortCode _forceSpecificProperty = GCCPropertyShortCode.None;
		public GCCPropertyShortCode ForceSpecificProperty {
			get {
				return ( User.Property != GCCProperty.GCCMain && User.Property != GCCProperty.None ) ? User.PropertyShortCode : _forceSpecificProperty;
			}
			set {
				_forceSpecificProperty = value;
			}
		}

        protected override void OnInit( EventArgs e ) {
            base.OnInit( e );
            if ( !IsUsersGroupValid ||
                ( CheckUserProperty &&
                    ( User == null
						|| ( User.Group == UserGroups.PropertyManagers && User.PropertyShortCode != PropertyShortCode && User.PropertyShortCode != GCCPropertyShortCode.None )
						|| ( User.Group == UserGroups.PropertyStaff && User.PropertyShortCode != PropertyShortCode )
                    )
                ) ) {
                HttpContext.Current.Response.Redirect( "~/Unauthorized.aspx", true );
            }
        }

    }
}
