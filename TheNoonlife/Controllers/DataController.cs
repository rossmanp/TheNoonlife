using System.Web.Mvc;
using TheNoonlife.Models;

namespace TheNoonlife.Controllers
{
    public class DataController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Data
        public ActionResult Index()
        {
            return View();
        }

        //This method passes a List<ApplicationUser> to the view based on the age queried
        public ActionResult AggregateData(int userGroupQueried)
        {
            var userGroup = new UserGroupQuery(userGroupQueried);
            if (userGroup.UsersByGroup.Count == 0)
            {
                ViewBag.error = "There are no users in that age group; Please try again!";
                return View("QueryError");
            }

            return View(userGroup);
        }
    }
}