using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Followers;

public class FollowToggle
{
    public class Command : IRequest<Result<Unit>>
    {
        public string TargerUsername { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly DataContext _db;
        private readonly IUserAccessor _userAccessor;
        public Handler(DataContext db, IUserAccessor userAccessor)
        {
            _db = db;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var observer = await _db.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            var target = await _db.Users.FirstOrDefaultAsync(x => x.UserName == request.TargerUsername);

            var following = await _db.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target,
                };

                _db.UserFollowings.Add(following);
            }
            else
            {
                _db.UserFollowings.Remove(following);
            }

            var success = await _db.SaveChangesAsync() > 0;
            if (success) return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Failed to update following");
        }
    }
}
