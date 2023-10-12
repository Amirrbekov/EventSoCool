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
        public PagingParams Params { get; set; }
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
            var query = _db.Activities.OrderBy(d => d.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currenUsername = _userAccessor.GetUsername() }).AsQueryable();

            return Result<PageList<ActivityDto>>.Success(
                await PageList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, request.Params.PageSize)    
            );
        }
    }
}
