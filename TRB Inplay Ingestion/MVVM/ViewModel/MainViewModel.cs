using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using TRB_Inplay_Ingestion.MVVM.Model;
using TRB_Inplay_Ingestion.Process;

namespace TRB_Inplay_Ingestion.MVVM.ViewModel
{
    internal enum Age
    {
        Minimum,
        Maximum
    }



    class MainViewModel : Abstract.ViewBaseModel
    {
        public AsyncCommand StartIngestionCommand { get; private set; }

        #region Properties
        private string _statusTextReport;

        public string StatusTextReport
        {
            get { return _statusTextReport; }
            set { _statusTextReport = value; RaisePropertiesChanged(nameof(StatusTextReport)); }
        }

        private double _processBar;

        public double ProcessBar
        {
            get { return _processBar; }
            set { _processBar = value; RaisePropertiesChanged(nameof(ProcessBar)); }
        }

        private string _RSSFeedUrl;

        public string RSSFeedUrl
        {
            get { return _RSSFeedUrl; }
            set { _RSSFeedUrl = value; RaisePropertiesChanged(nameof(RSSFeedUrl)); }
        }

        private ObservableCollection<string> _RSSFeedURLType;

        public ObservableCollection<string> RSSFeedURLType
        {
            get { return _RSSFeedURLType; }
            set { _RSSFeedURLType = value; RaisePropertiesChanged(nameof(RSSFeedURLType)); }
        }

       

        private string _RSSFeedURLTypeSelectedItem;

        public string RSSFeedURLTypeSelectedItem
        {
            get { return _RSSFeedURLTypeSelectedItem; }
            set { _RSSFeedURLTypeSelectedItem = value; RaisePropertiesChanged(nameof(RSSFeedURLTypeSelectedItem)); }
        }

        private int _RSSFeedURLTypeSelectedIndex;

        public int RSSFeedURLTypeSelectedIndex
        {
            get { return _RSSFeedURLTypeSelectedIndex; }
            set { _RSSFeedURLTypeSelectedIndex = value; RaisePropertiesChanged(nameof(RSSFeedURLTypeSelectedIndex)); }
        }



        public int ComboBoxTypeSelectedIndex
        {
            get { return GetProperty(() => ComboBoxTypeSelectedIndex); }
            set { SetProperty(() => ComboBoxTypeSelectedIndex, value); }
        }

        #endregion


        public string SJPLRSSFeedURL { get; set; } = @"https://sjpl.bibliocommons.com/events/rss/all";
        public string SCCLRSSFeedURL { get; set; } = @"https://sccl.bibliocommons.com/events/rss/all";

        public List<ExcelModel> listExcelOutput = new List<ExcelModel>();

        public MainViewModel()
        {

            listExcelOutput = new List<ExcelModel>();
            StartIngestionCommand = new AsyncCommand(StartIngestion);

            RSSFeedURLType = new ObservableCollection<string> { SJPLRSSFeedURL, SCCLRSSFeedURL};
        }



        private async Task StartIngestion()
        {
            try
            {
                if(string.IsNullOrEmpty(RSSFeedURLTypeSelectedItem))
                {
                    InformationMessage("Please Select RSS Feed URL.", "Input Error");
                    return;
                }

                StatusTextReport = "Reading RSS feed...";

                string SelectedProject;

                switch(RSSFeedURLTypeSelectedIndex)
                {
                    case 0:
                        SelectedProject = "SJPL";
                        break;
                    case 1:
                        SelectedProject = "SCCL";
                        break;
                    default:
                        InformationMessage("Please Select a valid RSS Feed URL.", "Input Error");
                        return;
                }

                /*
                if (RSSFeedURLTypeSelectedItem == SJPLRSSFeedURL)
                {
                    SelectedProject = "SJPL";
                }
                else if (RSSFeedURLTypeSelectedItem == SCCLRSSFeedURL)
                {
                    SelectedProject = "SCCL";
                }
                else
                {
                    SelectedProject = "Other";
                }*/

                Progress<double> statusProgressBar = new Progress<double>((value) =>
                {
                    ProcessBar = value;
                });

                Progress<string> statusProgressText = new Progress<string>((value) =>
                {
                    StatusTextReport = value;
                });

                await Task.Run(() =>
                {
                    ProcessWorks processWorks = new ProcessWorks();
                    listExcelOutput.Clear();
                    processWorks.ProcessRssFeeds(RSSFeedURLTypeSelectedItem, SelectedProject, listExcelOutput, statusProgressBar, statusProgressText);
                });

                StatusTextReport = "Completed!";
            }
            catch (Exception ex)
            {
                ErrorMessage(ex);
            }
        }
    }
}
