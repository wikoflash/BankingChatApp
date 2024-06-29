using BankingChatApp.Core.Entities;
using BankingChatApp.Core.Interfaces;

namespace BankingChatApp.Application.Services
{
    public class ChatService
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatService(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<IEnumerable<ChatMessage>> GetMessagesAsync()
        {
            return await _chatMessageRepository.GetAllAsync();
        }

        public async Task AddMessageAsync(ChatMessage message)
        {
            message.Timestamp = DateTime.Now;
            await _chatMessageRepository.AddAsync(message);
        }
    }
}
