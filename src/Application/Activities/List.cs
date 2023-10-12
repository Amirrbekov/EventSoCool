using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Interfaces;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<Result<PageList<ActivityDto>>>
    {
        public ActivityParams Params { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PageList<ActivityDto>>>
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
        public async Task<Result<PageList<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _db.Activities
                .Where(d => d.Date >= request.Params.StartDate)
                .OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currenUsername = _userAccessor.GetUsername() }).AsQueryable();

            if (request.Params.IsGoind && !request.Params.IsHost)
            {
                query = query.Where(x => x.Attendees.Any(a => a.Username == _userAccessor.GetUsername()));
            }
            if (request.Params.IsHost && !request.Params.IsGoind)
            {
                query = query.Where(x => x.HostUsername == _userAccessor.GetUsername());
            }

            return Result<PageList<ActivityDto>>.Success(
                await PageList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)    
            );
      
        }
    }
}
