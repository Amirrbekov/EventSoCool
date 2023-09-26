using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly DataContext _db;
        public Handler(DataContext db)
        {
            _db = db;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var activity = await _db.Activities.FindAsync(request.Id);

            _db.Remove(activity);

            await _db.SaveChangesAsync();
        }
    }
}
