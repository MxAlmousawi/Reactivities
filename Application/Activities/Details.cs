using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler(DataContext context, IMapper mapper)
            : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext context = context;
            private readonly IMapper mapper = mapper;

            public async Task<Result<ActivityDto>> Handle(
                Query request,
                CancellationToken cancellationToken
            )
            {
                var activity = await context
                    .Activities.ProjectTo<ActivityDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}
