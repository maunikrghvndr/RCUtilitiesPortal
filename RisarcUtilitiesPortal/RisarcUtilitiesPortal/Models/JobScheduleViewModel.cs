using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Models
{
    public class JobScheduleViewModel
    {
        public JobScheduleViewModel()
        {
            RunDays = new[]
            {
                new ScheduleRunDays { Name = "Sunday" },
                new ScheduleRunDays { Name = "Monday" },
                new ScheduleRunDays { Name = "Tuesday" },
                new ScheduleRunDays { Name = "Wednesday" },
                new ScheduleRunDays { Name = "Thursday" },
                new ScheduleRunDays { Name = "Friday" },
                new ScheduleRunDays { Name = "Saturday" }
            }.ToList();
        }

        [Display(Name = "Job Schedule ID")]
        public int JobScheduleID { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Job ID")]
        public int JobID { get; set; }
        public IEnumerable<SelectListItem> Jobs { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        [Range(1000, 86400000)]
        [Display(Name = "Interval In Milliseconds")]
        public int IntervalInMilliseconds { get; set; }
        public string RunDaysString { get; set; }
        public IList<ScheduleRunDays> RunDays { get; set; }
        public DateTime? StartDateTime { get; set; }
        [Display(Name = "Next Start Date Time")]
        public DateTime? NextStartDateTime { get; set; }
        public string StartDateTimeString { get; set; }
        public string NextStartDateTimeString { get; set; }
    }

    public class ScheduleRunDays
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}