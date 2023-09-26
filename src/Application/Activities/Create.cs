using Domain;
using MediatR;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities;

public class Create
{
    public class Command : IRequest
    {
        public Activity Activity { get; set; }
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
            _db.Activities.Add(request.Activity);

            await _db.SaveChangesAsync();
        }
    }
}
