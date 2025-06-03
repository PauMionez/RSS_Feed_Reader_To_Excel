using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Xml;
using TRB_Inplay_Ingestion.MVVM.ViewModel;
using TRB_Inplay_Ingestion.Services;
using TRB_Inplay_Ingestion.MVVM.Model;
using TRB_Inplay_Ingestion.Files;

namespace TRB_Inplay_Ingestion.Process
{
    class ProcessWorks : Abstract.ViewBaseModel
    {

        /// <summary>
        /// Process the RSS feeds and convert them to Excel format
        /// Use SyndicationFeed to read the RSS feed and extract the required information
        /// Read RSS 2.0 
        /// </summary>
        /// <param name="SelectedRSSFeed"></param>
        /// <param name="selectedProject"></param>
        /// <param name="listExcelOutput"></param>
        /// <param name="progress"></param>
        public void ProcessRssFeeds(string SelectedRSSFeed, string selectedProject, List<ExcelModel> listExcelOutput, IProgress<double> progress)
        {
            try
            {
                ExcelDataCollection excelDataCollection = new ExcelDataCollection();
                HelperConverter helperConverter = new HelperConverter();
                #region November 2019 version. Rss feed input.

                GetsFeeds rss = new GetsFeeds();
                SyndicationFeed feed = rss.GetFeed(SelectedRSSFeed);
                int feedCount = feed.Items.Count();
                int progressCounter = 0;

                foreach (SyndicationItem item in feed.Items)
                {
                    #region Local variables
                    XmlNode locationNode = null;
                    XmlNode startDateNode = null;
                    XmlNode endDateNode = null;

                    ////Change process
                    double progressValue = ((double)progressCounter / feedCount) * 100d;
                    progress.Report(progressValue);
                    ++progressCounter;

                    #endregion

                    SyndicationElementExtension locationPosition = item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "location");
                    SyndicationElementExtension startDatePosition = item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "start_date");
                    SyndicationElementExtension endDatePosition = item.ElementExtensions.FirstOrDefault(ext => ext.OuterName == "end_date");

                    if (locationPosition != null)
                    {
                        locationNode = locationPosition.GetObject<XmlElement>();
                    }

                    if (startDatePosition != null)
                    {
                        startDateNode = startDatePosition.GetObject<XmlElement>();
                    }

                    if (endDatePosition != null)
                    {
                        endDateNode = endDatePosition.GetObject<XmlElement>();
                    }

                    System.Collections.ObjectModel.Collection<SyndicationCategory> categoryNode = item.Categories;

                    //SyndicationCategory _audience = categoryNode.FirstOrDefault(x => x.Scheme == "Audience");
                    //string audience = _audience == null ? "" : _audience.Name;

                    //Get all Autdience in one title
                    List<string> _audienceList = categoryNode.Where(x => x.Scheme == "Audience").Select(x => x.Name.Trim()).ToList();
                    string audience = string.Join(Environment.NewLine, _audienceList);

                    SyndicationCategory _type = categoryNode.FirstOrDefault(x => x.Scheme == "Type");
                    string type = _type == null ? "" : _type.Name;

                    SyndicationCategory _series = categoryNode.FirstOrDefault(x => x.Scheme == "Program");
                    string series = _series == null ? "" : _series.Name;

                    string location = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "name"));
                    string street = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "street"));
                    string addressNumber = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "number"));
                    string city = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "city"));
                    string state = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "state"));
                    string zip = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "zip"));
                    string locationDetails = locationNode == null ? "" : helperConverter.ConvertToUTF8(rss.GetChildNodeText(locationNode.ChildNodes, "location_details"));

                    SyndicationLink _mainProgramImage = item.Links.FirstOrDefault(x => x.RelationshipType == "enclosure");
                    string mainProgramImage = _mainProgramImage == null ? "" : _mainProgramImage.Uri.OriginalString;

                    SyndicationLink _descriptionLink = item.Links.FirstOrDefault(x => x.RelationshipType == "alternate");
                    string descriptionLink = _descriptionLink == null ? "" : _descriptionLink.Uri.OriginalString;

                    // Convert UTC to Pacific Time Zone
                    DateTime startTimeText = DateTime.Parse(startDateNode.InnerText.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal - 1);

                    // Daylight savings time as of 3 13 2020
                    startTimeText = startTimeText.Subtract(TimeSpan.FromHours(1));

                    DateTime endTimeText = DateTime.Parse(endDateNode.InnerText.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal);
                    endTimeText = endTimeText.Subtract(TimeSpan.FromHours(1));

                    DateTime startTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(startTimeText, "Pacific Standard Time");
                    startTime = startTime.Subtract(TimeSpan.FromHours(1));

                    DateTime endTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(endTimeText, "Pacific Standard Time");
                    endTime = endTime.Subtract(TimeSpan.FromHours(1));

                    string minimumAge = string.IsNullOrWhiteSpace(audience) ? "" : rss.GetAge(Age.Minimum, audience, selectedProject);
                    string maximumAge = string.IsNullOrWhiteSpace(audience) ? "" : rss.GetAge(Age.Maximum, audience, selectedProject);

                    // Decode description text
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(item.Summary.Text.Replace("&nbsp;", " "));

                    // string descriptionText = MISC.Encoding.convertToUTF8(doc.DocumentNode.InnerText);
                    string descriptionText = doc.DocumentNode.InnerText;

                    ExcelModel excelOutputModel = new ExcelModel
                    {
                        Location_or_Branch_Name = location,
                        Street_Address_1 = $"{addressNumber} {street}",
                        //Street_Address_2 = street,
                        City = city,
                        State = state,
                        Zip = zip,
                        Program_Name = item.Title.Text,
                        Main_Program_Image = mainProgramImage,
                        Program_Description = descriptionText.Trim(),
                        Class_starts_on = $"{startTime.ToString("MM")}/{startTime.Day.ToString("D2")}/{startTime.Year}",
                        Class_ends_on = $"{endTime.ToString("MM")}/{endTime.Day.ToString("D2")}/{endTime.Year}",
                        Days_classes_are_regularly_held = startTime.DayOfWeek.ToString().Substring(0, 3),
                        Class_start_time = excelDataCollection.getClassStartTime($"{startTime.Hour.ToString("D2")}:{startTime.Minute.ToString("D2")}"),
                        Class_end_time = excelDataCollection.getClassEndTime($"{endTime.Hour.ToString("D2")}:{endTime.Minute.ToString("D2")}"),
                        Minimum_Age = minimumAge,
                        Maximum_Age = maximumAge,
                        Link_to_registration_page_or_form = helperConverter.ConvertToUTF8(descriptionLink),
                        Audience = audience,
                        Type = type,
                        LocationDetail = locationDetails,
                        GMTStartTime = "0800",
                        GMTEndTime = "0800",
                        //GMTStartTime = Regex.Match(startDateNode.InnerText, GMTTImePattern).Value,
                        //GMTEndTime = Regex.Match(endDateNode.InnerText, GMTTImePattern).Value,
                        Series = series,
                        RawDataStartTime = startTimeText,
                        RawDataEndTime = endTimeText
                    };

                    // Age filter.
                    // Accept below 18 only
                    if (!string.IsNullOrWhiteSpace(maximumAge))
                    {
                        if (int.Parse(maximumAge) > 18)
                        {
                            continue;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(minimumAge))
                    {
                        if (int.Parse(minimumAge) >= 18)
                        {
                            continue;
                        }
                    }


                    listExcelOutput.Add(excelOutputModel);

                }

                    #region Export to excel 

                    CreateExcel createExcel = new CreateExcel();
                    FileManagement excelTemp = new FileManagement();

                    string sjplTemplate = excelTemp.SJPL_XLSX_TEMP;
                    string scclTemplate = excelTemp.SCCL_XLSX_TEMP;


                    createExcel.ExcelOutputProcess(sjplTemplate, scclTemplate, selectedProject, listExcelOutput);

                    #endregion
                #endregion

            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }

    }
}
