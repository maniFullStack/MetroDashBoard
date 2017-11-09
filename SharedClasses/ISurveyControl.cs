using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteUtilities;

namespace SharedClasses {
    public interface ISurveyControl<T> {
        string SessionKey { get; set; }
        string DBColumn { get; set; }
        string DBValue { get; }

        MessageManager MessageManager { get; }

        T GetValue();

        void PrepareQuestionForDB( StringBuilder columnList, SQLParamList sqlParams );
     
    }
}
