using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRB_Inplay_Ingestion.Services
{
    class HelperConverter : Abstract.ViewBaseModel
    {
        public void WriteMilitaryTime(DateTime date)
        {
            // Convert hours and minutes to 24-hour scale.
            string value = date.ToString("HHmm");
            //Console.WriteLine(value);
        }

        public  DateTime ParseMilitaryTime(string time,int year, int month, int day)
        {
            
            // Convert hour part of string to integer.
            string hour = time.Substring(0, 2);
            int hourInt = int.Parse(hour);
            if (hourInt >= 24)
            {
               WarningMessage("Invalid hour");
            }
            
            // Convert minute part of string to integer.
            string minute = time.Substring(2, 2);
            int minuteInt = int.Parse(minute);
            if (minuteInt >= 60)
            {
                WarningMessage("Invalid minute");
            }
            
            // Return the DateTime.
            return new DateTime(year, month, day, hourInt, minuteInt, 0);
        }

        public string ConvertToUTF8(string text)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(text);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }


    }
}
