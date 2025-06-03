using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRB_Inplay_Ingestion.Services;

namespace TRB_Inplay_Ingestion.MVVM.Model
{
    class ExcelModel
    {
        ExcelDataCollection excelDataCollection = new ExcelDataCollection();

        public string Organization_Name
        {
            get { return excelDataCollection.Organization_Name; }
        }

        public string Long_Description
        {
            get { return excelDataCollection.Long_Description; }
        }

        public string Web_URL { get; set; }
        public string Main_phone_number
        {
            get { return excelDataCollection.Main_phone_number; }
        }

        private string _Location_or_Branch_Name;

        public string Location_or_Branch_Name
        {
            get { return _Location_or_Branch_Name; }
            set { _Location_or_Branch_Name = value; }
        }

        private string _Street_Address_1;

        public string Street_Address_1
        {
            get { return _Street_Address_1; }
            set { _Street_Address_1 = value; }
        }

        private string _Street_Address_2;

        public string Street_Address_2
        {
            get { return _Street_Address_2; }
            set { _Street_Address_2 = value; }
        }

        private string _City;

        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _State;

        public string State
        {
            get { return _State; }
            set { _State = value; }
        }

        public string Zip { get; set; }

        private string _Program_Name;

        public string Program_Name
        {
            get { return _Program_Name; }
            set { _Program_Name = value; }
        }

        private string _Main_Program_Image;

        public string Main_Program_Image
        {
            get { return _Main_Program_Image; }
            set { _Main_Program_Image = value; }
        }

        private string _Program_Description;

        public string Program_Description
        {
            get { return _Program_Description; }
            set { _Program_Description = value; }
        }

        public int Cost { get { return 0; } }

        private string _Class_starts_on;

        public string Class_starts_on
        {
            get { return _Class_starts_on; }
            set { _Class_starts_on = value; }
        }

        private string _Class_ends_on;

        public string Class_ends_on
        {
            get { return _Class_ends_on; }
            set { _Class_ends_on = value; }
        }

        private string _Days_classes_are_regularly_held;

        public string Days_classes_are_regularly_held
        {
            get { return _Days_classes_are_regularly_held; }
            set { _Days_classes_are_regularly_held = value; }
        }

        private string _Class_start_time;

        public string Class_start_time
        {
            get { return _Class_start_time; }
            set { _Class_start_time = value; }
        }

        private string _Class_end_time;

        public string Class_end_time
        {
            get { return _Class_end_time; }
            set { _Class_end_time = value; }
        }

        public string Minimum_Age { get; set; }

        public string Maximum_Age { get; set; }

        private string _Link_to_registration_page_or_form;

        public string Link_to_registration_page_or_form
        {
            get { return _Link_to_registration_page_or_form; }
            set { _Link_to_registration_page_or_form = value; }
        }

        public string Type { get; internal set; }
        public string Audience { get; internal set; }
        public string LocationDetail { get; internal set; }
        public string GMTStartTime { get; internal set; }
        public string GMTEndTime { get; internal set; }
        public string Series { get; internal set; }
        public DateTime RawDataStartTime { get; internal set; }
        public DateTime RawDataEndTime { get; internal set; }

        public string Program_Visibility
        {
            get { return excelDataCollection.ProgramVisibility; }
        }

        public string Provider_Visibility
        {
            get { return excelDataCollection.ProviderVisibility; }
        }
    }
}
