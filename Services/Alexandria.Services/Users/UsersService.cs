namespace Alexandria.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly AlexandriaDbContext db;

        public UsersService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await this.GetByIdAsync(id);

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>()
        {
            var users = await this.db.Users.AsNoTracking()
                                     .Where(u => !u.IsDeleted)
                                     .To<TModel>()
                                     .ToListAsync();

            return users;
        }

        public async Task<TModel> GetUserByIdAsync<TModel>(string id)
        {
            var user = await this.db.Users.Where(u => u.Id == id && !u.IsDeleted)
                                    .To<TModel>()
                                    .FirstOrDefaultAsync();

            return user;
        }

        private async Task<ApplicationUser> GetByIdAsync(string id)
            => await this.db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }
}
