using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Security
{
    public class IsHostRequirment : IAuthorizationRequirement { }

    public class IsHostRequirmentHandler(
        IHttpContextAccessor httpContextAccessor,
        DataContext dbContext
    ) : AuthorizationHandler<IsHostRequirment>
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly DataContext dbContext = dbContext;

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsHostRequirment requirement
        )
        {
            var userId = httpContextAccessor.HttpContext.User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (userId == null)
                return Task.CompletedTask;

            var activityId = Guid.Parse(
                httpContextAccessor
                    .HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id")
                    .Value?.ToString()
            );

            var attendee = dbContext
                .ActivityAttendees.AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId)
                .Result;

            if (attendee == null)
                return Task.CompletedTask;

            if (attendee.IsHost)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
