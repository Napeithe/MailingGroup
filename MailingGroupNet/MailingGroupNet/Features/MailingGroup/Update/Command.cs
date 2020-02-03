using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;

namespace MailingGroupNet.Features.MailingGroup.Update
{
    public class Command : MailingGroupDto, IRequest<ApiResult<MailingGroupDto>>
    {
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
            IdentityUser user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == request.UserId, cancellationToken);

            if (user is null)
            {
                return ApiResult<MailingGroupDto>.Failed("User not found", 404);
            }

            string newName = request.Name.Trim();

            if (string.IsNullOrEmpty(newName))
            {
                return ApiResult<MailingGroupDto>.Failed("Name is required", 400);
            }
            
            var updatedGroup = await _context.MailingGroups
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (updatedGroup is null)
            {
                return ApiResult<MailingGroupDto>.Failed("This mailing group is not exist", 404);
            }

            bool nameIsTaken = await _context.MailingGroups
                .Where(x=>x.UserId ==request.UserId)
                .Where(x=>x.Id != request.Id)
                .Where(x=>x.Name == newName).AnyAsync(cancellationToken);

            if (nameIsTaken)
            {
                return ApiResult<MailingGroupDto>.Failed("This mailing group name is already exists", 401);
            }


            updatedGroup.Name = newName;

            _context.Update(updatedGroup);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResult<MailingGroupDto>.Success(request);
        }
    }
}
