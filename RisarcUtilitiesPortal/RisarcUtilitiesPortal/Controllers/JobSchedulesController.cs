using Risarc.Enterprise.Repository;
using Risarc.Enterprise.Repository.Entities;
using RisarcUtilitiesPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Controllers
{
    public class JobSchedulesController : Controller
    {
        private EnterpriseContext unitOfWork = new EnterpriseContext();

        public ActionResult Index()
        {
            //The sp could move to a table ViewModelSps.            
            var model = unitOfWork.GetEntityBySp<JobScheduleViewModel>("sp_ScheduledJobsGetAll");
            model.ForEach(s => {
                if (s.NextStartDateTime.ToString().Length > 0)
                    s.StartDateTimeString = s.StartDateTime.ToString();
                else
                    s.StartDateTimeString = "";
                s.NextStartDateTimeString = s.NextStartDateTime.ToString();
            });
            if (Request.IsAjaxRequest())
                return Json(model, JsonRequestBehavior.AllowGet);
            else
                return View(model);
        }

        public ActionResult Create()
        {
            var model = new JobScheduleViewModel();
            List<SelectListItem> ddJobs = new List<SelectListItem>();
            unitOfWork.Jobs.ToList().ForEach(job =>
            {
                if (unitOfWork.JobSchedules.FirstOrDefault(j => j.JobID == job.JobID) == null && job.Active)
                    ddJobs.Add(new SelectListItem { Value = job.JobID.ToString(), Text = job.JobDescription });
            });
            model.Jobs = ddJobs;
            model.IntervalInMilliseconds = 10000;
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(JobScheduleViewModel schedule)
        {
            try
            {
                DateTime? startDateTime = (schedule.StartDateTime ?? null);

                StringBuilder runDays = new StringBuilder();
                schedule.RunDays.ToList().ForEach(s =>
                {
                    runDays.AppendFormat("{0}", s.Checked ? s.Name + "," : "");
                });

                if (ModelState.IsValid)
                {
                    unitOfWork.JobSchedules.Add(new JobSchedule
                    {
                        JobID = schedule.JobID,
                        IntervalInMilliseconds = schedule.IntervalInMilliseconds,
                        NextStartDateTime = startDateTime,
                        RunDays = runDays.ToString()
                    });
                    await unitOfWork.SaveChangesAsync();
                }
                else
                {
                    if (schedule.JobID.ToString().Trim().Length == 0)
                        ModelState.AddModelError("JobID", "JobID is required.");
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            var sch = unitOfWork.JobSchedules.Find(id);
            var splitter = ',';
            string[] setDays = sch.RunDays.Split(splitter);

            var model = from js in unitOfWork.JobSchedules
                        join j in unitOfWork.Jobs on js.JobID equals j.JobID
                        where js.JobScheduleID == id
                        select new JobScheduleViewModel
                        {
                            IntervalInMilliseconds = js.IntervalInMilliseconds,
                            JobID = j.JobID,
                            JobDescription = j.JobDescription,
                            JobScheduleID = js.JobScheduleID,
                            NextStartDateTime = js.NextStartDateTime,
                            StartDateTime = js.StartDateTime
                        };
            var m = model.First();
            m.RunDays.ToList().ForEach(rd =>
            {
                rd.Checked = setDays.Contains(rd.Name);
            });
            return View(m);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit(int id, JobScheduleViewModel schedule)
        {
            try
            {
                var sch = unitOfWork.JobSchedules.Find(id);
                StringBuilder runDays = new StringBuilder();
                StringBuilder runDaysCode = new StringBuilder();
                sch.NextStartDateTime = schedule.NextStartDateTime;
                sch.IntervalInMilliseconds = schedule.IntervalInMilliseconds;
                schedule.RunDays.ToList().ForEach(s =>
                {
                    runDays.AppendFormat("{0}", s.Checked ? s.Name + "," : "");
                    runDaysCode.AppendFormat("{0}", s.Checked ? "1" : "0");
                });
                sch.RunDays = runDays.ToString();

                unitOfWork.Attach(sch, unitOfWork.JobSchedules);
                await unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult Delete(int id)
        //{
        //    var model = from js in unitOfWork.JobSchedules
        //                join j in unitOfWork.Jobs on js.JobID equals j.JobID
        //                where js.JobScheduleID == id
        //                select new JobScheduleViewModel
        //                {
        //                    IntervalInMilliseconds = js.IntervalInMilliseconds,
        //                    JobDescription = j.JobDescription,
        //                    JobScheduleID = js.JobScheduleID,
        //                    NextStartDateTime = js.NextStartDateTime,
        //                    RunDaysString = js.RunDays,
        //                    StartDateTime = js.StartDateTime
        //                };

        //    return View(model.First());
        //}

        //[HttpPost]
        //public async Task<ActionResult> Delete(int id, JobScheduleViewModel schedule)
        //{
        //    try
        //    {
        //        unitOfWork.JobSchedules.Remove(id);
        //        await unitOfWork.SaveAsync();
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}