namespace Alexandria.Services.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class MessagesService : IMessagesService
    {
        private readonly AlexandriaDbContext db;

        public MessagesService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateMessageAsync(string authorId, string receiverId, string content)
        {
            var message = new Message
            {
                AuthorId = authorId,
                ReceiverId = receiverId,
                Content = content,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Messages.AddAsync(message);

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllMessagesByUserIdAsync<TModel>(string currentUserId, string userId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Messages.AsNoTracking()
                                            .Where(m => !m.IsDeleted
                                            && ((m.AuthorId == currentUserId && m.ReceiverId == userId)
                                            || (m.AuthorId == userId && m.ReceiverId == currentUserId)))
                                            .OrderByDescending(m => m.CreatedOn)
                                            .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value).Reverse();
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<Tuple<string, string>>> GetAllDistinctChatsAsync(string currentUserId)
        {
            var distinctChats = await this.db.Messages.AsNoTracking()
                                        .Where(m => !m.IsDeleted
                                               && (m.ReceiverId == currentUserId
                                               || m.AuthorId == currentUserId))
                                        .Select(m => Tuple.Create(m.AuthorId, m.ReceiverId))
                                        .Distinct()
                                        .ToListAsync();

            return distinctChats;
        }

        public async Task<TModel> GetLatestChatMessagesAsync<TModel>(string authorId, string receiverId)
        {
            var messages = await this.db.Messages.AsNoTracking()
                                           .Where(m => !m.IsDeleted
                                           && m.AuthorId == authorId
                                           && m.ReceiverId == receiverId)
                                           .OrderByDescending(m => m.CreatedOn)
                                           .To<TModel>()
                                           .FirstOrDefaultAsync();

            return messages;
        }
    }
}
