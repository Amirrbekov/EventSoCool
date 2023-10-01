using Application.Core;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _db;
        public Handler(DataContext db)
        {
            _db = db;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _db.Activities.FindAsync(request.Id);

            //if (activity == null) return null;

            _db.Remove(activity);

            var result = await _db.SaveChangesAsync() > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete the activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
