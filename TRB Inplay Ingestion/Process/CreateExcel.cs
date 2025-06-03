using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.XlsIO;
using TRB_Inplay_Ingestion.Files;
using TRB_Inplay_Ingestion.MVVM.Model;

namespace TRB_Inplay_Ingestion.Process
{
    internal enum CsvHeader
    {
        EventTitle = 1,
        StartDate = 2,
        EndDate = 3,
        Location = 4,
        RoomAndFloor = 5,
        Path = 6,
        Description = 7,
        Topic = 8,
        Series = 9,
        PrimaryAudience = 10,
        Image = 11
    }

    class CreateExcel : Abstract.ViewBaseModel
    {
        //public List<ExcelModel> listExcelOutput = new List<ExcelModel>();

        private string outputFile = "";
        public void ExcelOutputProcess(string sjplTemplate, string scclTemplate, string organization, List<ExcelModel> listExcelOutput)
        {
            try
            {
                List<RawDataModel> rawDataList = new List<RawDataModel>();
                int progress = 0;

                // Select the appropriate template
                string selectedTemplate = organization.ToUpper() == "SJPL" ? sjplTemplate : scclTemplate;

                // Get column mappings
                var columnMap = GetColumnMapping(organization);

                using (ExcelEngine ee = new ExcelEngine())
                {
                    IApplication app = ee.Excel.Application;
                    IWorkbook wb = app.Workbooks.Open(selectedTemplate);
                    IWorksheet ws = wb.Worksheets[0];
                    ws.Name = "Sheet1";

                    int counter = 3;

                    foreach (ExcelModel item in listExcelOutput)
                    {
                        //progress = (int)(((float)counter / listExcelOutput.Count) * 100d);
                        //worker.ReportProgress(progress, "Processing output data");

                        // Assign values using dynamic mapping
                        ws.Range[counter, columnMap["Provider Name"]].Text = item.Organization_Name;
                        ws.Range[counter, columnMap["Long Description"]].Text = item.Long_Description;
                        ws.Range[counter, columnMap["Website URL"]].Text = item.Link_to_registration_page_or_form;
                        ws.Range[counter, columnMap["Provider Phone Number"]].Text = item.Main_phone_number;
                        ws.Range[counter, columnMap["Location Name"]].Text = item.Location_or_Branch_Name;
                        ws.Range[counter, columnMap["Street Address 1"]].Text = item.Street_Address_1;
                        ws.Range[counter, columnMap["Street Address 2"]].Text = item.Street_Address_2;
                        ws.Range[counter, columnMap["City"]].Text = item.City;
                        ws.Range[counter, columnMap["State"]].Text = item.State;
                        ws.Range[counter, columnMap["Zip"]].Text = item.Zip;
                        ws.Range[counter, columnMap["Program Name"]].Text = item.Program_Name;
                        ws.Range[counter, columnMap["Program General Images"]].Text = item.Main_Program_Image;
                        ws.Range[counter, columnMap["Program Description"]].Text = item.Program_Description;
                        ws.Range[counter, columnMap["Price"]].Number = item.Cost;
                        ws.Range[counter, columnMap["Starts On"]].Text = item.Class_starts_on;
                        ws.Range[counter, columnMap["Ends On"]].Text = item.Class_ends_on;
                        ws.Range[counter, columnMap["Days of Week"]].Text = item.Days_classes_are_regularly_held;
                        ws.Range[counter, columnMap["Start Time"]].Text = item.Class_start_time;
                        ws.Range[counter, columnMap["End Time"]].Text = item.Class_end_time;
                        ws.Range[counter, columnMap["Minimum Age"]].Text = item.Minimum_Age;
                        ws.Range[counter, columnMap["Maximum Age"]].Text = item.Maximum_Age;
                        ws.Range[counter, columnMap["Program Visibility"]].Text = item.Program_Visibility;
                        ws.Range[counter, columnMap["Provider Visibility"]].Text = item.Provider_Visibility;

                        if (organization == "SCCL")
                        {
                            ws.Range[counter, columnMap["Audience"]].Text = item.Audience;
                        }
                        
                        ws.Range[counter, columnMap["Foreign Program URL"]].Text = item.Link_to_registration_page_or_form;

                        // Prepare CSV RawData
                        DateTime startDate = DateTime.ParseExact(item.Class_starts_on, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        DateTime endDate = DateTime.ParseExact(item.Class_ends_on, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        string startGMTTime = item.GMTStartTime;
                        string endGMTTime = item.GMTStartTime;

                        rawDataList.Add(new RawDataModel
                        {
                            EventTitle = item.Program_Name,
                            StartDate = string.Format("{0}, {1} {2} {3} {4} -{5}",
                                                        item.Days_classes_are_regularly_held,
                                                        startDate.Day,
                                                        startDate.ToString("MMM"),
                                                        startDate.Year,
                                                        item.RawDataStartTime.ToString("HH:mm"),
                                                        startGMTTime),
                            EndDate = string.Format("{0}, {1} {2} {3} {4} -{5}",
                                                        item.Days_classes_are_regularly_held,
                                                        endDate.Day,
                                                        endDate.ToString("MMM"),
                                                        endDate.Year,
                                                        item.RawDataEndTime.ToString("HH:mm"),
                                                        endGMTTime),
                            Location = item.Location_or_Branch_Name,
                            RoomAndFloor = item.LocationDetail,
                            Path = item.Link_to_registration_page_or_form,
                            Description = item.Program_Description,
                            Topic = item.Type,
                            Series = item.Series,
                            PrimaryAudience = item.Audience,
                            Image = item.Main_Program_Image
                        });

                        counter++;
                    }

                    // Save Excel
                    string desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    DirectoryInfo saveDirectory = Directory.CreateDirectory(Path.Combine(desktopDir, "TRB InPlay Ingestion Output"));
                    outputFile = Path.Combine(saveDirectory.FullName, $"rssFeed_{organization} {DateTime.Now:MMMM-dd-yyyy}.xlsx");
                    wb.SaveAs(outputFile);
                }

                // CSV Output
                using (ExcelEngine ee = new ExcelEngine())
                {
                    IApplication app = ee.Excel.Application;
                    IWorkbook wb = app.Workbooks.Create();
                    IWorksheet ws = wb.Worksheets[0];
                    ws.Name = "Sheet1";

                    // CSV Headers
                    ws.Range[1, (int)CsvHeader.EventTitle].Text = "Event Title";
                    ws.Range[1, (int)CsvHeader.StartDate].Text = "Start Date";
                    ws.Range[1, (int)CsvHeader.EndDate].Text = "End Date";
                    ws.Range[1, (int)CsvHeader.Location].Text = "Location";
                    ws.Range[1, (int)CsvHeader.RoomAndFloor].Text = "Room and Floor";
                    ws.Range[1, (int)CsvHeader.Path].Text = "Path";
                    ws.Range[1, (int)CsvHeader.Description].Text = "Description";
                    ws.Range[1, (int)CsvHeader.Topic].Text = "Topic";
                    ws.Range[1, (int)CsvHeader.Series].Text = "Series";
                    ws.Range[1, (int)CsvHeader.PrimaryAudience].Text = "Primary Audience";
                    ws.Range[1, (int)CsvHeader.Image].Text = "Image";

                    int csvCounter = 2;
                    foreach (RawDataModel item in rawDataList)
                    {
                        //progress = (int)(((float)csvCounter / listExcelOutput.Count) * 100d);
                        //worker.ReportProgress(progress, "Processing csv data");

                        ws.Range[csvCounter, (int)CsvHeader.EventTitle].Text = item.EventTitle;
                        ws.Range[csvCounter, (int)CsvHeader.StartDate].Text = item.StartDate;
                        ws.Range[csvCounter, (int)CsvHeader.EndDate].Text = item.EndDate;
                        ws.Range[csvCounter, (int)CsvHeader.Location].Text = item.Location;
                        ws.Range[csvCounter, (int)CsvHeader.RoomAndFloor].Text = item.RoomAndFloor;
                        ws.Range[csvCounter, (int)CsvHeader.Path].Text = item.Path;
                        ws.Range[csvCounter, (int)CsvHeader.Description].Text = item.Description;
                        ws.Range[csvCounter, (int)CsvHeader.Topic].Text = item.Topic;
                        ws.Range[csvCounter, (int)CsvHeader.Series].Text = item.Series;
                        ws.Range[csvCounter, (int)CsvHeader.PrimaryAudience].Text = item.PrimaryAudience;
                        ws.Range[csvCounter, (int)CsvHeader.Image].Text = item.Image;

                        csvCounter++;
                    }

                    outputFile = Path.ChangeExtension(outputFile, "csv");
                    wb.SaveAs(outputFile);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }


        private Dictionary<string, int> GetColumnMapping(string org)
        {
            try
            {
                if (org.ToUpper() == "SJPL")
                {
                    return new Dictionary<string, int>
                    {
                        ["Provider Name"] = 1,
                        ["Long Description"] = 2,
                        ["Website URL"] = 3,
                        ["Facebook Fan Page"] = 4,
                        ["Logo"] = 5,
                        ["Provider Cover Image"] = 6,
                        ["Administrator Name"] = 7,
                        ["Administrator Email"] = 8,
                        ["Provider Phone Number"] = 9,
                        ["Location Name"] = 10,
                        ["Street Address 1"] = 11,
                        ["Street Address 2"] = 12,
                        ["City"] = 13,
                        ["State"] = 14,
                        ["Zip"] = 15,
                        ["Country"] = 16,
                        ["Program Name"] = 17,
                        ["Program Foreign ID"] = 18,
                        ["Program General Images"] = 19,
                        ["Program Description"] = 20,
                        ["Program Tags"] = 21,
                        ["Overnight"] = 22,
                        ["Schedule Foreign ID"] = 23,
                        ["Price"] = 24,
                        ["Price Description"] = 25,
                        ["Price Unit Cost"] = 26,
                        ["Starts On"] = 27,
                        ["Ends On"] = 28,
                        ["Days of Week"] = 29,
                        ["Start Time"] = 30,
                        ["End Time"] = 31,
                        ["Registration Begins At"] = 32,
                        ["Registration Ends At"] = 33,
                        ["Minimum Age"] = 34,
                        ["Maximum Age"] = 35,
                        ["Program Visibility"] = 36,
                        ["Provider Visibility"] = 37,
                        ["Foreign Program URL"] = 38
                        
                    };
                }
                else if (org.ToUpper() == "SCCL")
                {
                    return new Dictionary<string, int>
                    {
                        ["Provider Name"] = 1,
                        ["Long Description"] = 2,
                        ["Website URL"] = 3,
                        ["Facebook Fan Page"] = 4,
                        ["Logo"] = 5,
                        ["Provider Cover Image"] = 6,
                        ["Administrator Name"] = 7,
                        ["Administrator Email"] = 8,
                        ["Provider Phone Number"] = 9,
                        ["Location Name"] = 10,
                        ["Street Address 1"] = 11,
                        ["Street Address 2"] = 12,
                        ["City"] = 13,
                        ["State"] = 14,
                        ["Zip"] = 15,
                        ["Country"] = 16,
                        ["Program Name"] = 17,
                        ["Program Foreign ID"] = 18,
                        ["Program General Images"] = 19,
                        ["Program Description"] = 20,
                        ["Program Tags"] = 21,
                        ["Overnight"] = 22,
                        ["Schedule Foreign ID"] = 23,
                        ["Price"] = 24,
                        ["Price Description"] = 25,
                        ["Price Unit Cost"] = 26,
                        ["Starts On"] = 27,
                        ["Ends On"] = 28,
                        ["Days of Week"] = 29,
                        ["Start Time"] = 30,
                        ["End Time"] = 31,
                        ["Registration Begins At"] = 32,
                        ["Registration Ends At"] = 33,
                        ["Minimum Age"] = 34,
                        ["Maximum Age"] = 35,
                        ["Program Visibility"] = 36,
                        ["Provider Visibility"] = 37,
                        ["Audience"] = 38,
                        ["Foreign Program URL"] = 39
                    };
                }
                else
                {
                   WarningMessage("Invalid organization type provided.");
                   return null;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
                return null; 
            }
        }

    }
}
