using Application.Core;
using Application.Interfaces;
using Application.Profiles;
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

namespace Application.Followers;

public class List
{
    public class Query : IRequest<Result<List<Profiles.Profile>>>
    {
        public string Predicate { get; set; }
        public string Username { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<Profiles.Profile>>>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext db, IMapper mapper, IUserAccessor userAccessor)
        {
            _db = db;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Result<List<Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var profiles = new List<Profiles.Profile>();

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await _db.UserFollowings.Where(x => x.Target.UserName == request.Username)
                        .Select(u => u.Observer).ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currenUsername = _userAccessor.GetUsername() }).ToListAsync();
                    break;

                case "following":
                    profiles = await _db.UserFollowings.Where(x => x.Observer.UserName == request.Predicate)
                        .Select(u => u.Target).ProjectTo<Profiles.Profile>(_mapper.ConfigurationProvider, new { currenUsername = _userAccessor.GetUsername() }).ToListAsync();
                    break;
            }

            return Result<List<Profiles.Profile>>.Success(profiles);
        }
    }
}
