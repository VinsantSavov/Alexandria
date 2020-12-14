namespace Alexandria.Services.Data.Tests
{

    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using Alexandria.Services.Messages;
    using Microsoft.EntityFrameworkCore;

    using Xunit;

    public class MessagesServiceTest
    {
        public MessagesServiceTest()
        {
            AutoMapperConfig.RegisterMappings(typeof(MessageTestModel).GetTypeInfo().Assembly);
        }

        [Theory]
        [InlineData("author1", "receiver1", "content1")]
        [InlineData("author2", "author2", "content2")]
        [InlineData("receiver3", "receiver3", "content3")]
        public async Task CreateMessageShouldAddToDatabase(string authorId, string receiverId, string content)
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);

            var messagesService = new MessagesService(db);

            await messagesService.CreateMessageAsync(authorId, receiverId, content);
            var result = await db.Messages.FirstOrDefaultAsync();

            Assert.Equal(1, await db.Messages.CountAsync());
            Assert.Equal(authorId, result.AuthorId);
            Assert.Equal(receiverId, result.ReceiverId);
            Assert.Equal(content, result.Content);
        }

        [Fact]
        public async Task GetAllMessagesBetweenUsersIdShouldReturnAllMessages()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                },
                new Message
                {
                    AuthorId = "author2",
                    ReceiverId = "receiver2",
                    Content = "content2",
                },
                new Message
                {
                    AuthorId = "author3",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllMessagesBetweenUsersAsync<MessageTestModel>("author1", "receiver1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllMessagesBetweenUsersIdShouldNotReturnDeletedMessages()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    IsDeleted = true,
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                },
                new Message
                {
                    AuthorId = "author2",
                    ReceiverId = "receiver2",
                    Content = "content2",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllMessagesBetweenUsersAsync<MessageTestModel>("author1", "receiver1");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllMessagesBetweenUsersIdShouldReturnCorrectResultWithTakeAndSkip()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content2",
                },
                new Message
                {
                    AuthorId = "author3",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllMessagesBetweenUsersAsync<MessageTestModel>("author1", "receiver1", 2, 1);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllMessagesBetweenUsersIdShouldReturnReversedCollectionWhenTakeHasValue()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content2",
                    CreatedOn = new DateTime(2018, 11, 11),
                },
                new Message
                {
                    AuthorId = "author3",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllMessagesBetweenUsersAsync<MessageTestModel>("author1", "receiver1", 3);
            var resultMessage = result.FirstOrDefault();

            Assert.Equal(3, result.Count());
            Assert.Equal("author1", resultMessage.AuthorId);
            Assert.Equal("receiver1", resultMessage.ReceiverId);
            Assert.Equal("content2", resultMessage.Content);
        }

        [Fact]
        public async Task GetAllDistinctChatsShouldReturnDistinctChats()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content2",
                    CreatedOn = new DateTime(2018, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllDistinctChatsAsync("author1");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllDistinctChatsShouldNotReturnDeletedDistinctChats()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                    IsDeleted = true,
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content2",
                    CreatedOn = new DateTime(2018, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllDistinctChatsAsync("author1");

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllDistinctChatsShouldReturnRightChat()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "receiver1",
                    ReceiverId = "author1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content2",
                    CreatedOn = new DateTime(2018, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver3",
                    Content = "content3",
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetAllDistinctChatsAsync("author1");
            var resultChat = result.FirstOrDefault();

            Assert.Equal("author1", resultChat.Item1);
            Assert.Equal("receiver1", resultChat.Item2);
        }

        [Fact]
        public async Task GetLatestChatMessagesShouldReturnLatestMessage()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "contentTOP",
                    CreatedOn = new DateTime(2020, 12, 12),
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetLatestChatMessagesAsync<MessageTestModel>("author1", "receiver1");

            Assert.Equal("contentTOP", result.Content);
        }

        [Fact]
        public async Task GetLatestChatMessagesShouldNotReturnDeletedMessage()
        {
            var options = new DbContextOptionsBuilder<AlexandriaDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new AlexandriaDbContext(options);
            await db.Messages.AddRangeAsync(
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content1",
                    CreatedOn = new DateTime(2019, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "content01",
                    CreatedOn = new DateTime(2020, 11, 11),
                },
                new Message
                {
                    AuthorId = "author1",
                    ReceiverId = "receiver1",
                    Content = "contentTOP",
                    CreatedOn = new DateTime(2020, 12, 12),
                    IsDeleted = true,
                });
            await db.SaveChangesAsync();

            var messagesService = new MessagesService(db);

            var result = await messagesService.GetLatestChatMessagesAsync<MessageTestModel>("author1", "receiver1");

            Assert.Equal("content01", result.Content);
        }

        public class MessageTestModel : IMapFrom<Message>
        {
            public int Id { get; set; }

            public string Content { get; set; }

            public string AuthorId { get; set; }

            public string ReceiverId { get; set; }
        }
    }
}
