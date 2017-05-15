using System.Collections.Generic;
using System.Linq;

namespace TheNoonlife.Models
{
    public class UserGroupQuery
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        public UserGroupQuery(int ageGroupQueried)
        {
            UsersByAgeGroup = _db.Users
                .Where(m => m.Age > ageGroupQueried && m.Age < ageGroupQueried + 11).ToList();
        }

        public List< ApplicationUser> UsersByAgeGroup { get; }
    }
}