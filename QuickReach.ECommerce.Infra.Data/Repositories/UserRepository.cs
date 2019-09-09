using QuickReach.ECommerce.Domain;
using QuickReach.ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickReach.ECommerce.Infra.Data.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ECommerceDbContext context) : base(context)
        {

        }

        public IEnumerable<User> Retrieve(string search = "", int skip = 0, int count = 10)
        {
            var result = this.context.Users
                .Where(c => c.Username.Contains(search))
            .Skip(skip)
            .Take(count)
            .ToList();

            return result;
        }
    }
}
