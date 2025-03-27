using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using UserApi.Application.DTOs;
using UserApi.Application.Features.User.Queries;
using UserApi.Application.Services;

namespace UserApi.Application.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserService _userService;

        public GetUserByIdQueryHandler(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            // Récupérer depuis la base de données
            Log.Information("Récupération d’un utilisateur By Id de la BDD");
            var user = await _userService.GetUserByIdAsync(request.Id);
            if (user == null) return null;            

            return user;
        }
    }
}
