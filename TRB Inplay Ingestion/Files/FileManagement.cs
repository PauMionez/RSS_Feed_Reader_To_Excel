using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRB_Inplay_Ingestion.Files
{
    class FileManagement
    {
        #region Properties
        //private static FileManagement _ins = null;

        //public static FileManagement ins
        //{
        //    get { return _ins ?? (_ins = new FileManagement()); }
        //}
        #endregion

        public string TEMPLATE_XLSX_FILE { get { return "Files/template.xlsx"; } }
        public string AUDIENCE_XLSX_FILE { get { return "Files/audience.xlsx"; } }
        public string ADDRESS_XLSX_FILE { get { return "Files/address.xlsx"; } }
        public string SCCL_XLSX_TEMP { get { return "Files/Template_rssFeed_SCCL.xlsx"; } }
        public string SJPL_XLSX_TEMP { get { return "Files/Template_rssFeed_SJPL.xlsx"; } }
    }
}
