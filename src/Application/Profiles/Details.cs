using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profiles;

public class Details
{
    public class Query : IRequest<Result<Profile>>
    {
        public string Username { get; set; }
    }

    public class Hadnler : IRequestHandler<Query, Result<Profile>>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Hadnler(DataContext db, IMapper mapper, IUserAccessor userAccessor)
        {
            _db = db;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currenUsername = _userAccessor.GetUsername() }).SingleOrDefaultAsync(x => x.Username == request.Username);

            return Result<Profile>.Success(user);
        }
    }
}
