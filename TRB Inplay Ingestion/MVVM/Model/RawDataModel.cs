using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRB_Inplay_Ingestion.MVVM.Model
{
    class RawDataModel
    {
        public string EventTitle { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Location { get; set; }
        public string RoomAndFloor { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Topic { get; set; }
        public string Series { get; set; }
        public string PrimaryAudience { get; set; }
        public string Image { get; set; }
    }
}
