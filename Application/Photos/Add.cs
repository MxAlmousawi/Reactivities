using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Photo>>
        {
            private readonly DataContext context;
            private readonly IUserAccessor userAccessor;
            private readonly IPhotoAccessor photoAccessor;

            public Handler(
                DataContext context,
                IUserAccessor userAccessor,
                IPhotoAccessor photoAccessor
            )
            {
                this.context = context;
                this.userAccessor = userAccessor;
                this.photoAccessor = photoAccessor;
            }

            public async Task<Result<Photo>> Handle(
                Command request,
                CancellationToken cancellationToken
            )
            {
                var user = await context
                    .Users.Include(p => p.Photos)
                    .FirstOrDefaultAsync(x => x.UserName == userAccessor.GetUsername());
                if (user == null)
                {
                    return null;
                }

                var photoUploadResult = await photoAccessor.AddPhoto(request.File);

                var Photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId,
                };

                if (!user.Photos.Any(x => x.IsMain))
                {
                    Photo.IsMain = true;
                }

                user.Photos.Add(Photo);

                var result = await context.SaveChangesAsync() > 0;

                if (result)
                {
                    return Result<Photo>.Success(Photo);
                }

                return Result<Photo>.Failure("Problem adding photo"); 
            }
        }
    }
}
