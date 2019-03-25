using Risarc.Enterprise.Repository;
using Risarc.Enterprise.Repository.Constants;
using Risarc.Enterprise.Repository.Entities;
using RisarcUtilitiesPortal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Controllers
{
    public class JobsController : Controller, IDisposable
    {
        private EnterpriseContext unitOfWork = new EnterpriseContext();

        public ActionResult Index()
        {
            if (Request.QueryString["jobCategoryID"] != null)
                Session["JOBCATEGORYID"] = Request.QueryString["jobCategoryID"].ToString();
            else if (Session["JOBCATEGORYID"] == null)
                Session["JOBCATEGORYID"] = "-1";

            var jobCategoryID = Int32.Parse(Session["JOBCATEGORYID"].ToString());

            List<SelectListItem> ddJobCategories = new List<SelectListItem>();
            ddJobCategories.Add(new SelectListItem { Value = "-1", Text = "All", Selected = jobCategoryID == -1 });
            unitOfWork.JobCategories.OrderBy(o => o.Description).ToList().ForEach(cat =>
            {
                ddJobCategories.Add(new SelectListItem { Value = cat.JobCategoryID.ToString(), Text = cat.Description, Selected = cat.JobCategoryID == jobCategoryID });
            });

            ddJobCategories.Add(new SelectListItem { Value = "0", Text = "Un-Assigned", Selected = jobCategoryID == 0 });

            var model = new List<Job>();

            if (jobCategoryID > -1)
                model = unitOfWork.Jobs.Where(job => job.Active && job.JobCategoryID == jobCategoryID).OrderBy(j => j.JobDescription).ToList();
            else
                model = unitOfWork.Jobs.Where(job => job.Active).OrderBy(j => j.JobDescription).ToList();

            var jModel = new List<JobViewModel>();
            model.ForEach(job =>
            {
                jModel.Add(new JobViewModel
                {
                    JobCategories = ddJobCategories,
                    JobDescription = job.JobDescription,
                    JobName = job.JobName,
                    JobID = job.JobID,
                    JobStatusTypeID = job.JobStatusTypeID,
                    LastStatusDetail = job.LastStatusDetail,
                    LastStatusTime = job.LastStatusTime
                });
            });

            if (!Request.IsAjaxRequest())
            {
                return View(jModel);
            }
            else
            {
                return PartialView("_Index", jModel);
            }
        }

        public ActionResult CheckJobRun(int jobID)
        {
            var thisJob = unitOfWork.Jobs.Where(j => j.JobID == jobID).FirstOrDefault();
            var temp = unitOfWork.GetEntityBySp<JobScheduleViewModel>("sp_ScheduledJobsGetAll");
            var model = temp.FirstOrDefault(job => job.JobID == jobID);
            ViewBag.ScheduleJob = true;
            if (model == null)
            {
                ViewBag.ScheduleJob = false;
                model = new JobScheduleViewModel { JobID = thisJob.JobID, JobDescription = thisJob.JobDescription, NextStartDateTime = DateTime.MinValue };
            }
            return PartialView("_JobRunCheck", model);
        }

        public ActionResult JobAction(int jobID, string jobAction)
        {
            bool trigger = false;
            int jobStatusTypeID = 0;
            if (jobAction == "Stop" || jobAction == "Reset")
                jobStatusTypeID = (int)JobStatusTypes.Stopped;
            else
                jobStatusTypeID = (int)JobStatusTypes.ReadyToRun;

            var job = unitOfWork.Jobs.Find(jobID);

            switch (jobAction)
            {
                case "Run":
                    jobStatusTypeID = (int)JobStatusTypes.ReadyToRun;
                    trigger = true;
                    break;
                case "Stop":
                    jobStatusTypeID = (int)JobStatusTypes.Stopped;
                    trigger = true;
                    break;
                case "Reset":
                    jobStatusTypeID = (int)JobStatusTypes.Stopped;
                    job.LastStatusDetail = "Stopped";
                    break;
                default:
                    break;
            }

            job.JobStatusTypeID = jobStatusTypeID;
            unitOfWork.Attach(job, unitOfWork.Jobs);
            unitOfWork.SaveChanges();

            if (trigger)
            {
                string path = ConfigurationManager.AppSettings["JobsControlFilePath"].ToString();
                string controlFile = ConfigurationManager.AppSettings["JobsControlFileName"].ToString();

                using (StreamWriter sr = new StreamWriter(path + controlFile))
                {
                    try
                    {
                        sr.WriteLineAsync("Triggering Job Id " + jobID.ToString());
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new JobViewModel();
            List<SelectListItem> ddApps = new List<SelectListItem>();
            unitOfWork.Applications.OrderBy(o => o.AppName).ToList().ForEach(app =>
            {
                ddApps.Add(new SelectListItem { Value = app.AppID.ToString(), Text = app.AppName });
            });
            ddApps.Add(new SelectListItem { Value = "0", Text = "Select an app" });

            model.JobParameters = "<PARAMETERS></PARAMETERS>";
            model.EnteredAt = DateTime.Now;
            model.Active = true;
            model.ActivateTransaction = false;
            model.Apps = ddApps;
            model.AppSelected = true;

            List<SelectListItem> ddJobCategories = new List<SelectListItem>();
            unitOfWork.JobCategories.OrderBy(o => o.Description).ToList().ForEach(cat =>
            {
                ddJobCategories.Add(new SelectListItem { Value = cat.JobCategoryID.ToString(), Text = cat.Description, Selected = cat.JobCategoryID == 0 });
            });
            ddJobCategories.Add(new SelectListItem { Value = "0", Text = "Un-Assigned", Selected = true });
            model.JobCategories = ddJobCategories;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(JobViewModel job)
        {
            try
            {
                var model = job;
                List<SelectListItem> ddApps = new List<SelectListItem>();
                unitOfWork.Applications.OrderBy(o => o.AppName).ToList().ForEach(app =>
                {
                    ddApps.Add(new SelectListItem { Value = app.AppID.ToString(), Text = app.AppName });
                });
                ddApps.Add(new SelectListItem { Value = "0", Text = "Select an app" });
                model.EnteredAt = DateTime.Now;
                model.Active = true;
                model.Apps = ddApps;
                model.JobDescription = model.JobName;
                model.AppSelected = true;

                List<SelectListItem> ddJobCategories = new List<SelectListItem>();
                unitOfWork.JobCategories.OrderBy(o => o.Description).ToList().ForEach(cat =>
                {
                    ddJobCategories.Add(new SelectListItem { Value = cat.JobCategoryID.ToString(), Text = cat.Description, Selected = cat.JobCategoryID == 0 });
                });
                ddJobCategories.Add(new SelectListItem { Value = "0", Text = "Un-Assigned" });
                model.JobCategories = ddJobCategories;

                if (ModelState.IsValid)
                {
                    if (job.AppID == 0)
                    {
                        model.AppSelected = false;
                        return View(model);
                    }
                    else if (unitOfWork.Jobs.Where(j => j.JobName == job.JobName).Count() > 0)
                    {
                        model.AppSelected = true;
                        model.DupJobName = true;
                        return View(model);
                    }

                    unitOfWork.Jobs.Add(new Job
                    {
                        Active = true,
                        LastStatusDetail = "Stopped",
                        AppID = job.AppID,
                        EnteredAt = DateTime.Parse(DateTime.Today.ToShortDateString()),
                        JobDescription = job.NameToDesc(job.JobName),
                        JobName = job.JobName,
                        JobParameters = job.JobParameters,
                        JobCategoryID = Int32.Parse(Request["SelectedJobCategoryID"].ToString())
                    });
                    unitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var job = unitOfWork.Jobs.Find(id);
            var model = new JobViewModel();

            List<SelectListItem> ddJobCategories = new List<SelectListItem>();

            unitOfWork.JobCategories.OrderBy(o => o.Description).ToList().ForEach(cat =>
            {
                ddJobCategories.Add(new SelectListItem { Value = cat.JobCategoryID.ToString(), Text = cat.Description, Selected = cat.JobCategoryID == job.JobCategoryID });
            });
            ddJobCategories.Add(new SelectListItem { Value = "0", Text = "Un-Assigned", Selected = job.JobCategoryID == 0 });
            model.JobCategories = ddJobCategories;

            model.JobName = job.JobName;
            model.JobDescription = job.JobDescription;
            model.JobParameters = job.JobParameters;
            model.Active = job.Active;
            model.AppID = job.AppID;

            var appName = unitOfWork.Applications.Find(job.AppID).AppName;
            ViewData.Add("AppName", appName);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, JobViewModel job)
        {
            try
            {
                var jj = unitOfWork.Jobs.Find(id);
                var appName = unitOfWork.Applications.Find(jj.AppID).AppName;
                ViewData.Add("AppName", appName);

                if (unitOfWork.Jobs.Where(j => j.JobName == job.JobName && j.JobID != id).Count() > 0)
                {
                    job.DupJobName = true;
                    return View(job);
                }

                if (string.IsNullOrEmpty(job.JobDescription))
                {
                    job.DescRequired = true;
                    return View(job);
                }

                if (unitOfWork.Jobs.Where(j => j.JobDescription == job.JobDescription && j.JobID != id).Count() > 0)
                {
                    job.DupJobDesc = true;
                    return View(job);
                }

                if (ModelState.IsValid)
                {
                    var j = unitOfWork.Jobs.Find(id);
                    j.JobParameters = job.JobParameters;
                    j.JobDescription = job.JobDescription;
                    j.JobName = job.JobName;
                    j.Active = job.Active;
                    j.JobCategoryID = Int32.Parse(Request["SelectedJobCategoryID"].ToString());
                    unitOfWork.Jobs.Attach(j);
                    unitOfWork.Entry(j).State = EntityState.Modified;
                    unitOfWork.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(job);
                }

            }
            catch
            {
                return View();
            }
        }
    }
}