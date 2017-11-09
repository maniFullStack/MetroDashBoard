using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SharedClasses {
    public class DynamicDropDownList : DropDownList {
        //This class is meant to be used for dropdowns that are populated via JavaScript.
        //The SupportsEventValidation attribute is not inherited for subclasses, so this control won't be validated.
        //See: http://stackoverflow.com/a/8581311
    }
}
