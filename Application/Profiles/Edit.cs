using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(u => u.DisplayName).NotEmpty();
            }
        }

        public class Handler(IUserAccessor userAccessor, DataContext context)
            : IRequestHandler<Command, Result<Unit>>
        {
            private readonly IUserAccessor userAccessor = userAccessor;
            private readonly DataContext context = context;

            public async Task<Result<Unit>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var user = await context.Users.FirstOrDefaultAsync(u =>
                    u.UserName == userAccessor.GetUsername()
                );
                if (user == null)
                    return null;

                user.DisplayName = request.DisplayName ?? user.DisplayName;
                user.Bio = request.Bio ?? user.Bio;

                var success = await context.SaveChangesAsync() > 0;
                if (success)
                    return Result<Unit>.Success(Unit.Value);
                return Result<Unit>.Failure("Problem updating profile");
            }
        }
    }
}
