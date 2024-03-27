using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ExchangesRepository : IExchangesRepository
    {
        private readonly DataContext _context;
        public ExchangesRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<UserExchange> GetUserExchange(int sourceUserId, int targetUserId)
        {
            return await _context.Exchanges.FindAsync(sourceUserId, targetUserId);
        }

        public async Task<PagedList<ExchangeDto>> GetUserExchanges(ExchangesParams exchangesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var exchanges = _context.Exchanges.AsQueryable();

            if (exchangesParams.Predicate == "exchanged")
            {
                exchanges = exchanges.Where(exchange => exchange.SourceUserId == exchangesParams.UserId);
                users = exchanges.Select(exchange => exchange.TargetUser);
            }

            if (exchangesParams.Predicate == "exchangedBy")
            {
                exchanges = exchanges.Where(exchange => exchange.TargetUserId == exchangesParams.UserId);
                users = exchanges.Select(exchange => exchange.SourceUser);
            }

            var exchangedUsers = users.Select(user => new ExchangeDto
            {
                UserName = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<ExchangeDto>.CreateAsync(exchangedUsers, exchangesParams.PageNumber, exchangesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithExchanges(int userId)
        {
            return await _context.Users
                .Include(x => x.ExchangedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}