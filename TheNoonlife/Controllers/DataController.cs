using System.Web.Mvc;
using TheNoonlife.Models;
using System.Linq;
using System.Collections.Generic;

namespace TheNoonlife.Controllers
{
    public class DataController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Data
        public ActionResult Index()
        {
            ViewBag.Users = _db.Users.Select(u => u.FavoriteRestaurant).ToList();
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

        public ActionResult UserData()
        {
            var model = new UserGroupQuery();
            model.BrunchList = _db.Users.ToList().Select(x => new SelectListItem
            {
                Value = x.FavoriteRestaurant,
                Text = x.FavoriteRestaurant
            });
            return View(model);
        }

        [HttpPost]
        public ActionResult RestaurantLookUp(UserGroupQuery place)
        {
            string selectedValue = place.FavoriteRestaurant;
            UserGroupQuery userGroup = new UserGroupQuery(selectedValue);
            return View(userGroup);
        }
    }
}