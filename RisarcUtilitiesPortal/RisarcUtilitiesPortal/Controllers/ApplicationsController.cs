using Risarc.Enterprise.Repository;
using Risarc.Enterprise.Repository.Entities;
using RisarcUtilitiesPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Controllers
{
    public class ApplicationsController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new EnterpriseContext())
            {
                var model = context.Applications.OrderBy(a => a.AppName).ToList();
                return View(model);
            }            
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public bool AppNameExists(Application appVM)
        {
            using (var context = new EnterpriseContext())
            {
                if (context.Applications.Where(a => a.AppName == appVM.AppName && a.AppID != appVM.AppID).Count() > 0)
                    return true;
                else
                    return false;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Application appVM)
        {
            try
            {
                using (var context = new EnterpriseContext())
                {
                    appVM.EnteredAt = DateTime.Parse(DateTime.Today.ToShortDateString());

                    if (appVM.IsConsoleApp)
                        appVM.AppName = Request["ConsoleAppName"].ToString();
                    context.Applications.Add(appVM);
                    await context.SaveChangesAsync();

                    if (!Request.IsAjaxRequest())
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var model = new JobViewModel();
                        List<SelectListItem> ddApps = new List<SelectListItem>();
                        ddApps.Add(new SelectListItem { Value = appVM.AppID.ToString(), Text = appVM.AppName });
                        model.JobName = appVM.AppName;
                        model.JobDescription = appVM.AppName;
                        model.JobParameters = "<PARAMETERS></PARAMETERS>";
                        model.EnteredAt = DateTime.Now;
                        model.Active = true;
                        model.ActivateTransaction = false;
                        model.Apps = ddApps;

                        List<SelectListItem> ddJobCategories = new List<SelectListItem>();
                        context.JobCategories.OrderBy(o => o.Description).ToList().ForEach(cat =>
                        {
                            ddJobCategories.Add(new SelectListItem { Value = cat.JobCategoryID.ToString(), Text = cat.Description, Selected = cat.JobCategoryID == 0 });
                        });
                        model.JobCategories = ddJobCategories;

                        return PartialView("_CreateJob", model);
                    }
                }                
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult Edit(int id)
        {
            using (var context = new EnterpriseContext())
            {
                var model = context.Applications.Find(id);
                return View(model);
            }
        }

        [HttpPost]
        public string Edit(int id, Application app)
        {
            using (var context = new EnterpriseContext())
            {
                if (app.IsConsoleApp)
                    app.AppName = Request["ConsoleAppName"].ToString();
                context.Applications.Attach(app);
                context.SaveChangesAsync();
            }

            return Request["NavigateTo"].ToString();
        }
    }
}