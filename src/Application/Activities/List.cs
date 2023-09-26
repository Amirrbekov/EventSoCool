﻿using MediatR;
using Domain;
using Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Activities;

public class List
{
    public class Query : IRequest<List<Activity>>
    {

    }

    public class Handler : IRequestHandler<Query, List<Activity>>
    {
        private readonly DataContext _db;
        public Handler(DataContext db)
        {
            _db = db;
        }
        public async Task<List<Activity>> Handle(Query request, CancellationToken cancellationToken)
        {

            return await _db.Activities.ToListAsync();
        }
    }
}
