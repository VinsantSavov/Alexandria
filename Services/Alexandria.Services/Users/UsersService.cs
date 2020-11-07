namespace Alexandria.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public UsersService(AlexandriaDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await this.GetByIdAsync(id);

            user.IsDeleted = true;
            user.DeletedOn = DateTime.UtcNow;

            var ratings = await this.db.StarRatings.Where(r => r.UserId == id && !r.IsDeleted).ToListAsync();

            foreach (var rating in ratings)
            {
                rating.IsDeleted = true;
                rating.DeletedOn = DateTime.UtcNow;
            }

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllUsersAsync<TModel>()
        {
            var users = await this.db.Users.AsNoTracking()
                                     .Where(u => !u.IsDeleted)
                                     .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                     .ToListAsync();

            return users;
        }

        public async Task<TModel> GetUserByIdAsync<TModel>(string id)
        {
            var user = await this.db.Users.Where(u => u.Id == id && !u.IsDeleted)
                                    .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                    .FirstOrDefaultAsync();

            return user;
        }

        private async Task<ApplicationUser> GetByIdAsync(string id)
            => await this.db.Users.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }
}
