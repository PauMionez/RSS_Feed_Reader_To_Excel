using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Xml;
using TRB_Inplay_Ingestion.MVVM.ViewModel;

namespace TRB_Inplay_Ingestion.Services
{
    class GetsFeeds : Abstract.ViewBaseModel
    {
        /// <summary>
        /// Get the feed of the RSS url
        /// </summary>
        /// <param name="url">The RSS feed url</param>
        /// <returns>SyndicationFeed</returns>
        public SyndicationFeed GetFeed(string url)
        {
            SyndicationFeed syndicationFeed = new SyndicationFeed();

            try
            {
                using (XmlReader reader = XmlReader.Create(url))
                {
                    syndicationFeed = SyndicationFeed.Load(reader);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

            return syndicationFeed;
        }
        /// <summary>
        /// Get the child name by its node name
        /// </summary>
        /// <param name="childNodes">List of XML child nodes</param>
        /// <param name="nodeName">Node name to search</param>
        /// <returns></returns>
        public string GetChildNodeText(XmlNodeList childNodes, string nodeName)
        {
            string result = "";

            try
            {
                foreach (XmlNode item in childNodes)
                {
                    if (item.LocalName.ToLower().Equals(nodeName.ToLower()))
                    {
                        result = item.InnerText.Trim();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

            return result;
        }
        /// <summary>
        /// Get the age required.(e.g. n-n; n+)
        /// </summary>
        /// <param name="ageOption">Either minimum or maximum</param>
        /// <param name="audienceText">Audience xml node text</param>
        /// <returns></returns>
        public string GetAge(Age ageOption, string audienceText, string selectedProject)
        {
            string result = "";

            try
            {
                if (selectedProject == "SJPL")
                {
                    //Get ages
                    List<int> matches = Regex.Matches(audienceText, @"\d+")
                                       .Cast<Match>()
                                       .Select(x => int.Parse(x.Value))
                                       .ToList();

                    switch (ageOption)
                    {
                        case Age.Minimum:
                            result = matches.Min().ToString();
                            break;
                        case Age.Maximum:
                            if (matches.Count > 1)
                            {
                                result = matches.Max().ToString();
                                if (result == "0")
                                {
                                    result = "";
                                }
                            }
                            else
                            {
                                result = "";
                            }
                            break;
                        default:
                            break;

                    }
                }
                else if( selectedProject == "SCCL")
                {
                    int noAge = 0;

                    switch (ageOption)
                    {
                        case Age.Minimum:
                            result = string.Empty;
                            break;
                        case Age.Maximum:
                            result = string.Empty;
                            break;
                        default:
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }

            return result;
        }
    }
}
