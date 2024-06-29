using BankingChatApp.Core.Entities;
using BankingChatApp.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankingChatApp.Infrastructure.Data
{
    public class ChatContext : DbContext
    {
        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }

    public class ChatMessageRepository : IChatMessageRepository
    {
        private readonly ChatContext _context;

        public ChatMessageRepository(ChatContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatMessage entity)
        {
            await _context.ChatMessages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _context.ChatMessages.FindAsync(id);
            if (message != null)
            {
                _context.ChatMessages.Remove(message);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ChatMessage>> GetAllAsync()
        {
            return await _context.ChatMessages.Include(m => m.User).ToListAsync();
        }

        public async Task<ChatMessage> GetByIdAsync(int id)
        {
            return await _context.ChatMessages.Include(m => m.User).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesByUserIdAsync(int userId)
        {
            return await _context.ChatMessages.Where(m => m.UserId == userId).ToListAsync();
        }

        public async Task UpdateAsync(ChatMessage entity)
        {
            _context.ChatMessages.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
