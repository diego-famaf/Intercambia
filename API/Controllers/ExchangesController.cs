using System.Drawing;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ExchangesController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        public ExchangesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddExchange(string username)
        {
            var sourceUserId = User.GetUserId();
            var exchangedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.ExchangesRepository.GetUserWithExchanges(sourceUserId);

            if (exchangedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("Tu no puedes intercambiar contigo mismo");

            var userExchange = await _unitOfWork.ExchangesRepository.GetUserExchange(sourceUserId, exchangedUser.Id);

            if (userExchange != null) return BadRequest("El usuario ya sabe que estas interezado");

            userExchange = new UserExchange
            {
                SourceUserId = sourceUserId,
                TargetUserId = exchangedUser.Id
            };

            sourceUser.ExchangedUsers.Add(userExchange);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to exchange user");
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ExchangeDto>>> GetUserExchanges([FromQuery] ExchangesParams exchangesParams)
        {
            exchangesParams.UserId = User.GetUserId();

            var users = await _unitOfWork.ExchangesRepository.GetUserExchanges(exchangesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }
    }
}