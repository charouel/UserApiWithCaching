using MediatR;
using Serilog;
using UserApi.Application.Features.User.Queries;
using UserApi.Application.Services;
using UserApi.Domain.Entities;

namespace UserApi.Application.Handlers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<User>>
    {
        private readonly IUserService _userService;
        public GetAllUsersQueryHandler(IUserService userservice)
        {
            _userService = userservice;
        }
        public async Task<List<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            Log.Information("Récupération de tout les utilisateurs de la BDD");
            return await _userService.GetAllUsersAsync();
        }
    }
}
