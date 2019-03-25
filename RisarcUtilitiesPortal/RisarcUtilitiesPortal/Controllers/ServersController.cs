using Risarc.Enterprise.Repository;
using Risarc.Enterprise.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RisarcUtilitiesPortal.Controllers
{
    public class ServersController : Controller
    {
        private EnterpriseContext unitOfWork = new EnterpriseContext();
        public ActionResult Index()
        {
            var model = unitOfWork.Servers.ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Server server)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Servers.Add(server);
                    await unitOfWork.SaveChangesAsync();
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
            var model = unitOfWork.Servers.Find(id);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Server server)
        {
            try
            {
                unitOfWork.Attach(server, unitOfWork.Servers);
                await unitOfWork.SaveChangesAsync();

                return Redirect(Request["NavigateTo"].ToString());
            }
            catch
            {
                return View();
            }
        }
    }
}