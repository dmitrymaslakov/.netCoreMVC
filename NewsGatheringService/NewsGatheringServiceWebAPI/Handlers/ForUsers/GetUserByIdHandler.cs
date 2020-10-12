using MediatR;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.DAL.Entities;
using NewsGatheringService.UOW.DAL.Interfaces;
using NewsGatheringServiceWebAPI.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NewsGatheringServiceWebAPI.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
    {
        private readonly IRepository<User> _userRepository;

        public GetUserByIdHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _userRepository
                .FindBy(u => u.Id.CompareTo(request.Id) == 0)
                .FirstOrDefault();
            
            return Task.FromResult(user);
        }
    }
}
