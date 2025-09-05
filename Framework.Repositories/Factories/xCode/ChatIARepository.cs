using System.Threading.Tasks;
using System;
using Data;
using Framework.Data.Models.xCode;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Data.Models.Core;

namespace Framework.Repositories.Factories.xCode
{
    public class ChatIARepository
    {
        private readonly ApplicationDbContext _context;

        public ChatIARepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChatIA> CreateChatIAAsync(ChatIA ChatIA)
        {
            _context.ChatIAs.Add(ChatIA);
            await _context.SaveChangesAsync();
            return ChatIA;
        }

		public async Task<ChatIA> UpdateChatIAAsync(ChatIA ChatIA)
		{
			_context.ChatIAs.Update(ChatIA);
			await _context.SaveChangesAsync();
			return ChatIA;
		}

		public async Task<ChatIA> GetChatIAByIdAsync(int ChatIAId)
        {
            return await _context.ChatIAs
                .Include(c => c.ChatIAItems)
                .FirstOrDefaultAsync(c => c.Id == ChatIAId);
        }

        public async Task AddChatIAItemAsync(ChatIAItem ChatIAItem)
        {
            _context.ChatIAItems.Add(ChatIAItem);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ChatIA>> GetAllChatsAsync()
        {
            return await _context.ChatIAs
                .Include(c => c.ChatIAItems) // Opcional, caso deseje incluir as mensagens
                .OrderByDescending(c => c.DataCriacao)
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatIA>> GetAllChatsByUserIdAsync(int userId)
        {
            return await _context.ChatIAs.Where(b=> b.UsuarioId == userId)
               .Include(c => c.ChatIAItems) // Opcional, caso deseje incluir as mensagens
               .OrderByDescending(c => c.DataCriacao)
               .ToListAsync();
        }

        public async Task<IEnumerable<ChatIA>> GetAllChatsByUserIdAndTypedAsync(int userId, string typed)
        {
            return await _context.ChatIAs.Where(b => b.UsuarioId == userId && b.Typed == typed)
            .Include(c => c.ChatIAItems) // Opcional, caso deseje incluir as mensagens
            .OrderByDescending(c => c.DataCriacao)
            .ToListAsync();
        }
    }
}
