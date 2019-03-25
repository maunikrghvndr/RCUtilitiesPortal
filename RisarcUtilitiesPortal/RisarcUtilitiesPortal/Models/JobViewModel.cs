using Risarc.Enterprise.Repository.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Models
{
    public class JobViewModel : Job
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please select an app from the list.")]
        public IEnumerable<SelectListItem> Apps { get; set; }
        [Required]
        [Display(Name = "Job Name")]
        public new string JobName { get; set; }
        [Display(Name = "Job Description")]
        public new string JobDescription { get; set; }
        [AllowHtml]
        [Display(Name = "Job Parameters")]
        [Required]
        public new string JobParameters { get; set; }
        public bool AppSelected { get; set; }
        public new string LastStatusDetail { get; set; }
        public bool DupJobName { get; set; }
        public bool DupJobDesc { get; set; }
        public bool DescRequired { get; set; }
        public IEnumerable<SelectListItem> JobCategories { get; set; }
    }

    public static class JobViewModelExt
    {
        public static string NameToDesc(this JobViewModel jvm, string name)
        {
            string description = string.Empty;
            name = name.Replace(".", " ");
            foreach (var l in name)
            {
                if (name.ToUpper().IndexOf(l.ToString().ToUpper()) != 0 && l.ToString() == l.ToString().ToUpper())
                    description += " " + l.ToString();
                else
                    description += l.ToString();
            }
            description = description.Replace("  ", " ");
            return description;
        }
    }
}