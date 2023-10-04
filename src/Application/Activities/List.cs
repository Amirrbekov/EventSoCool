using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Application.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<Result<List<ActivityDto>>>
    {

    }

    public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;

        public Handler(DataContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var activities = await _db.Activities.ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return Result<List<ActivityDto>>.Success(activities);
        }
    }
}
