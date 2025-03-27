using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using UserApi.Application.Features.User.Commands;
using UserApi.Application.Services;

namespace UserApi.Application.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;

        public CreateUserCommandHandler(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Création d’un utilisateur avec Email: {Email}", request.User.Email);
            var userId = await _userService.AddUserAsync(request.User);
            Log.Information("Utilisateur créé avec ID: {UserId}", userId);
            _cache.Remove("GetAllUsersQuery_");
            Log.Information("Initialisation du cache GetAllUsersQuery_");
            return userId;
        }
    }
}
