using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;
using Model.Entity;

namespace MailingGroupNet.Features.MailingGroup.Create
{
    public class Command : IRequest<ApiResult<MailingGroupDto>>, IUserRequest
    {
        [Required]
        public string Name { get; set; }
        public string UserId { get; set; }   
    }

    public class Handler : IRequestHandler<Command, ApiResult<MailingGroupDto>>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<MailingGroupDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return ApiResult<MailingGroupDto>.Failed("User not found", 404);
            }

            string newName = request.Name.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                return ApiResult<MailingGroupDto>.Failed("Name is required", 400);
            }
            
            bool isExist = await _context.MailingGroups
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Name == newName)
                .AnyAsync(cancellationToken);

            if (isExist)
            {
                return ApiResult<MailingGroupDto>.Failed("This mailing group is already exists", 401);
            }

            Model.Entity.MailingGroup newEntity = new Model.Entity.MailingGroup
            {
                Name = newName,
                UserId = request.UserId,
                User =  user
            };

            await _context.AddAsync(newEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResult<MailingGroupDto>.Success(new MailingGroupDto(newEntity.Id, newEntity.Name));
        }
    }
}
