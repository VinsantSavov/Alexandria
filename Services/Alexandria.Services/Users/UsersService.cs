namespace Alexandria.Services.Users
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly AlexandriaDbContext db;

        public UsersService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<TModel>> GetChatUsersAsync<TModel>(string id)
        {
            var users = await this.db.Users.AsNoTracking()
                                           .Where(u => !u.IsDeleted
                                           && (u.SentMessages.Any(m => m.ReceiverId == id)
                                           || u.ReceivedMessages.Any(m => m.AuthorId == id)))
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

        public async Task<bool> IsUserDeletedAsync(string username)
            => await this.db.Users.AnyAsync(u => u.UserName == username && u.IsDeleted);

        public async Task<bool> IsUsernameUsedAsync(string username)
            => await this.db.Users.AnyAsync(u => u.UserName == username);

        public async Task<bool> DoesUserIdExistAsync(string userId)
            => await this.db.Users.AnyAsync(u => !u.IsDeleted && u.Id == userId);
    }
}
