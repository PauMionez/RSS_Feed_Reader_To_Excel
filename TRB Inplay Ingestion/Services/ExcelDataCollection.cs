using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TRB_Inplay_Ingestion.MVVM.Model;

namespace TRB_Inplay_Ingestion.Services
{
    class ExcelDataCollection : Abstract.ViewBaseModel
    {
       
        private readonly string patternStartTime = @"\d{2}\:\d{2}";
        

       	//Organization 
        public string Organization_Name = "San Jose Public Library";
        public string Short_Description = "San José Public Library enriches lives by fostering lifelong learning and by ensuring that every member of the community has access to a vast array of ideas and information.";
        public string Long_Description = "\"**Vision** We strive to provide: * Library services that are known and valued by the culturally diverse community, resulting in use from the broadest base of the public. * A welcoming and lively cultural and lifelong learning center for the community. * Timely and accurate information assistance that will inform and empower the public. * Services and collections that are relevant to community needs, readily accessible, and easy to use. * A well-trained and highly capable staff that reflects the diversity of San Jos√© and works well together to provide quality service to all users. * Appropriate facilities which are inviting and well maintained. * Technology that appropriately expands and enhances service. * Defense of intellectual freedom and the confidentiality of each individual's use of the library. * A close working relationship with other libraries, community agencies and organizations that foster cooperation, making the most efficient and effective use of the taxpayer's resources...[TRIMMED]\"";
        
        public string ProgramVisibility = "published";
        public string ProviderVisibility = "published";


       
        //Administrative Contact 
        public string Main_phone_number = "(408) 808-2000";

       

        //Parce the Time
        HelperConverter helperConverter = new HelperConverter();
        internal string getClassStartTime(string startDate)
        {
            string result = "";

            try
            {
                if (Regex.IsMatch(startDate, patternStartTime))
                {
                    result = Regex.Match(startDate, patternStartTime).Value;
                    DateTime dd = helperConverter.ParseMilitaryTime(result.Replace(":", ""), 2000, 12, 15);
                    result = dd.ToShortTimeString();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

            return result;
        }

        internal string getClassEndTime(string endDate)
        {
            string result = "";

            try
            {
                if (Regex.IsMatch(endDate, patternStartTime))
                {
                    result = Regex.Match(endDate, patternStartTime).Value;
                    DateTime dd = helperConverter.ParseMilitaryTime(result.Replace(":", ""), 2000, 12, 15);
                    result = dd.ToShortTimeString();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

            return result;
        }

        /* dumped code
         * //public string Web_URL = "https://www.facebook.com/sanjoselibrary/";
        //private readonly string patternEndTime = @"\d{4}$";
         *  //private readonly string patternDate = @"\d{2}\s+\w+\s+\d{4}";
        //private readonly string patternDay = @"^\w{3}";
        //internal string getStreetAddress1(string location)
        //{
        //    string result = "";

        //    try
        //    {
        //        Address m = ExcelFileDataToInformation.ins.listAddress.Where(x => x.Location.Equals(location)).FirstOrDefault();
        //        result = m != null ? m.StreetAddress1 : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return HelperConverter.convertToUTF8(result);
        //}

        //internal string getStreetAddress2(string location)
        //{
        //    string result = "";

        //    try
        //    {
        //        Address m = ExcelFileDataToInformation.ins.listAddress.Where(x => x.Location.Equals(location)).FirstOrDefault();
        //        result = m != null ? m.StreetAddress2 : "";

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return HelperConverter.convertToUTF8(result);
        //}

        //internal string getCity(string location)
        //{
        //    string result = "";

        //    try
        //    {
        //        Address m = ExcelFileDataToInformation.ins.listAddress.Where(x => x.Location.Equals(location)).FirstOrDefault();
        //        result = m != null ? m.City : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return HelperConverter.convertToUTF8(result);
        //}

        //internal string getState(string location)
        //{
        //    string result = "";

        //    try
        //    {
        //        Address m = ExcelFileDataToInformation.ins.listAddress.Where(x => x.Location.Equals(location)).FirstOrDefault();
        //        result = m != null ? m.State : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return HelperConverter.convertToUTF8(result);
        //}

        //internal string getZip(string location)
        //{
        //    string result = "";

        //    try
        //    {
        //        Address m = ExcelFileDataToInformation.ins.listAddress.Where(x => x.Location.Equals(location)).FirstOrDefault();
        //        result = m != null ? m.Zip : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}

        //internal string getClassStartsOn(string startDate)
        //{
        //    string result = "";

        //    try
        //    {
        //        if (Regex.IsMatch(startDate, patternDate))
        //        {
        //            result = Regex.Match(startDate, patternDate).Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}

        //internal string getClassEndsOn(string endDate)
        //{
        //    string result = "";

        //    try
        //    {
        //        if (Regex.IsMatch(endDate, patternDate))
        //        {
        //            result = Regex.Match(endDate, patternDate).Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}

        //internal string getDayClass(string startDate)
        //{
        //    string result = "";

        //    try
        //    {
        //        if (Regex.IsMatch(startDate, patternDay))
        //        {
        //            result = Regex.Match(startDate, patternDay).Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}

        //internal string convertDate(string date)
        //{
        //    string result = "";

        //    try
        //    {
        //        if (DateTime.TryParse(date, out DateTime d))
        //        {
        //            result = d.ToShortDateString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}



        //internal string getMinimumAge(string primaryAudience)
        //{
        //    string result = "";

        //    try
        //    {
        //        Audience m = ExcelFileDataToInformation.ins.listAudience.Where(x => x.PrimaryAudience.Equals(primaryAudience)).FirstOrDefault();
        //        result = m != null ? m.MinimumAge : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}

        //internal string getMaximumAge(string primaryAudience)
        //{
        //    string result = "";

        //    try
        //    {
        //        Audience m = ExcelFileDataToInformation.ins.listAudience.Where(x => x.PrimaryAudience.Equals(primaryAudience)).FirstOrDefault();
        //        result = m != null ? m.MaximumAge : "";
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage(ex);
        //    }

        //    return result;
        //}*/
    }
}
