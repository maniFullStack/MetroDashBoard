using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteUtilities;

namespace SharedClasses {
    public interface IReportFilter {
        string Label { get; set; }
        string SessionKey { get; set; }
        string DBColumn { get; set; }
        MessageManager MessageManager { get; }
        bool IsActive { get; }
        string GetSelectedFilterText();
        void Save();
        void AddToQuery( SQLParamList sqlParams );
        void Clear();
        string ID { get; set; }
    }
}
