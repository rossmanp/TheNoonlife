using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TheNoonlife.Models
{
    public class UserGroupQuery
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public IEnumerable<SelectListItem> BrunchList { get; set; }

        public string FavoriteRestaurant { get; set; }

        public UserGroupQuery(int ageGroupQueried)
        {
            UsersByGroup = _db.Users
                .Where(m => m.Age > ageGroupQueried && m.Age < ageGroupQueried + 11).OrderBy(m => m.Gender).ToList();
        }

        public UserGroupQuery(string place)
        {
            UsersByGroup = _db.Users.Where(m => m.FavoriteRestaurant == place).ToList();
            FavoriteRestaurant = place;
        }

        public UserGroupQuery()
        {

        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _db.Users.ToList();
        }

        public List< ApplicationUser> UsersByGroup { get; set; }

        
    }
}