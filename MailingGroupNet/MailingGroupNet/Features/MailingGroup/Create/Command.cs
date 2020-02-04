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
    public class Command : IRequest<ApiResult<MailingGroupItemListDto>>, IUserRequest
    {
        [Required]
        public string Name { get; set; }
        public string UserId { get; set; }   
    }

    public class Handler : IRequestHandler<Command, ApiResult<MailingGroupItemListDto>>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<MailingGroupItemListDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            AppUser user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return ApiResult<MailingGroupItemListDto>.Failed("User not found", 404);
            }

            string newName = request.Name.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                return ApiResult<MailingGroupItemListDto>.Failed("Name is required", 400);
            }
            
            bool isExist = await _context.MailingGroups
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Name == newName)
                .AnyAsync(cancellationToken);

            if (isExist)
            {
                return ApiResult<MailingGroupItemListDto>.Failed("This mailing group is already exists", 409);
            }

            Model.Entity.MailingGroup newEntity = new Model.Entity.MailingGroup
            {
                Name = newName,
                UserId = request.UserId,
                User =  user
            };

            await _context.AddAsync(newEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResult<MailingGroupItemListDto>.Success(new MailingGroupItemListDto(newEntity));
        }
    }
}
