using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pwipper.Core.Config;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;
using Pwipper.Filters;
using Pwipper.Models;

namespace Pwipper.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IPwipRepos pwipRepos;
        private readonly AppConfig appConfig;

        public HomeController(
            IUserRepos userRepos, 
            IPwipRepos pwipRepos,
            AppConfig appConfig) 
            : base(userRepos)
        {
            this.pwipRepos = pwipRepos;
            this.appConfig = appConfig;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var pwips = GetPwipModels(null);

            return View(new HomeModel
                            {
                                UserName = PwipperUser.Name,
                                Pwips = pwips,
                            });
        }

        [HttpGet]
        public ActionResult Partial(long? start)
        {
            var pwips = GetPwipModels(start);

            return PartialView("Pwips", new HomeModel
            {
                UserName = PwipperUser.Name,
                Pwips = pwips,
            });
        }

        [HttpPost]
        [TransactionAction]
        public ActionResult Post(string text)
        {
            var pwip = new Pwip {Author = PwipperUser, Text = text, Time = DateTime.UtcNow};
            pwipRepos.Save(pwip);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [TransactionAction]
        public ActionResult Delete(long id)
        {
            var pwip = pwipRepos.Get(id);
            if (pwip == null)
                return Json(false);

            if (pwip.Author != PwipperUser)
                throw new Exception("Can't delete a post which is not yours");

            pwipRepos.Delete(pwip);
            return Json(true);
        }

        private List<HomeModel.PwipModel> GetPwipModels(long? start)
        {
            return pwipRepos.GetNewFrom(appConfig.MaxPwipsPerQuery, start)
                .Select(p => new HomeModel.PwipModel { Id = p.Id, Text = p.Text, Time = new DateTime(p.Time.Ticks, DateTimeKind.Utc) })
                .ToList();
        }
    }
}
