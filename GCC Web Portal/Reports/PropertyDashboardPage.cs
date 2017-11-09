using GCC_Web_Portal;
using System;
using System.Data;
using System.Data.SqlClient;
using WebsiteUtilities;

namespace SharedClasses
{
    public class PropertyDashboardPage : AuthenticatedPage
    {
        public PropertyDashboardPage()
        {
            CheckUserProperty = true;
            SetGroups("ForumAdmin,CorporateManagement,PropertyManagers,PropertyStaff,CorporateMarketing");
            Dashboard dm = Master as Dashboard;
            if (dm != null)
            {
                dm.HidePropertyFilter = true;
            }
        }

        protected string GetJSData(DataRow dr, string prefix)
        {
            double total = dr[prefix + "_B2B"].ToString().StringToDbl() +
                            dr[prefix + "_MB"].ToString().StringToDbl() +
                            dr[prefix + "_T2B"].ToString().StringToDbl();
            if (total == 0)
            {
                return "[[0],[0],[0]]";
            }
            else
            {
                return String.Format("[[{0:0.0}], [{1:0.0}], [{2:0.0}]]",
                                        (dr[prefix + "_B2B"].ToString().StringToDbl() / total) * 100.0,
                                        (dr[prefix + "_MB"].ToString().StringToDbl() / total) * 100.0,
                                        (dr[prefix + "_T2B"].ToString().StringToDbl() / total) * 100.0);
            }
        }

        protected SQLParamList GetFilters()
        {
            Dashboard dm = Master as Dashboard;
            SQLParamList sqlParams = new SQLParamList();
            if (dm != null)
            {
                sqlParams = dm.GetFilters();
                if (dm.IsPropertyUser)
                {
                    SQLParamList sp2 = new SQLParamList();
                    foreach (SqlParameter sp in sqlParams.ToArray())
                    {
                        if (sp.ParameterName.Equals("@Property"))
                        {
                            sp2.Add("@PropertyID", (int)User.PropertyShortCode);
                        }
                        else
                        {
                            sp2.Add(sp);
                        }
                    }
                    sqlParams = sp2;
                }
                else
                {
                    sqlParams.Add("@PropertyID", (int)PropertyShortCode);
                }
            }
            else
            {
                sqlParams.Add("@PropertyID", (int)PropertyShortCode);
            }
            return sqlParams;
        }
    }
}