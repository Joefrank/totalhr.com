using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace totalhr.web.Areas.TimeRecording.ViewModels
{
    public class SearchVM
    {
        

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }

        public List<TimeRecordingDetailsVM> Results { get; set; }

        public void SetUpInitialSearch()
        {
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            this.PageNumber = 1;
            this.ResultsPerPage = 20;
        }
    }
}