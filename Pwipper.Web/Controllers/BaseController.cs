using System.Linq;
using System.Web.Mvc;
using Pwipper.Core.DataInterfaces;
using Pwipper.Core.Domain;

namespace Pwipper.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUserRepos UserRepos;

        public BaseController(IUserRepos userRepos)
        {
            UserRepos = userRepos;
        }

        private User cachedPwipperUser;

        public virtual User PwipperUser
        {
            get { return cachedPwipperUser ?? (cachedPwipperUser = UserRepos.GetAll().Take(1).FirstOrDefault()); }
        }
    }
}