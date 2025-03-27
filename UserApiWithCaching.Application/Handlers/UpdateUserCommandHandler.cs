using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using UserApi.Application.Features.User.Commands;
using UserApi.Application.Services;
using UserApi.Domain.Entities;

namespace UserApi.Application.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly IMemoryCache _cache;
        public UpdateUserCommandHandler(IUserService userService, IMemoryCache cache)
        {
            _userService = userService;
            _cache = cache;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            Log.Information("Modification d’un utilisateur");
            var existingUser = await _userService.GetUserByIdAsync(request.Id);

            if (existingUser == null)
            {
                return false; // L'utilisateur n'existe pas
            }

            // Mise à jour des champs
            existingUser.FirstName = request.UserDto.FirstName;
            existingUser.LastName = request.UserDto.LastName;
            existingUser.Email = request.UserDto.Email;
            existingUser.TextPresentation = request.UserDto.TextPresentation;

            await _userService.UpdateUserAsync(request.Id, existingUser);

            // Supprimer du cache pour forcer une mise à jour
            var cacheKey = $"{request.GetType().Name}_{request.Id}";
            Log.Information($"Initialisation du cache {cacheKey}");
            _cache.Remove(cacheKey); // init Cache par Id
            Log.Information("Initialisation du cache GetAllUsersQuery_");
            _cache.Remove("GetAllUsersQuery_"); // int cache Get All

            return true;
        }
    }
}
