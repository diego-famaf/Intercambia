using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IExchangesRepository
    {
        Task<UserExchange> GetUserExchange(int sourceUserId, int targetUserId);
        Task<AppUser> GetUserWithExchanges(int userId);
        Task<PagedList<ExchangeDto>> GetUserExchanges(ExchangesParams exchangesParams);
    }
}