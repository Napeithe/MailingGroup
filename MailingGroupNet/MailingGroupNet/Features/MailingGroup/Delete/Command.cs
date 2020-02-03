using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;

namespace MailingGroupNet.Features.MailingGroup.Delete
{
    public class Command : IRequest<ApiResult>, IUserRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class Handler :IRequestHandler<Command, ApiResult>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var mailingGroup = await _context.MailingGroups
                .Where(x => x.Id == request.Id)
                .Where(x => x.UserId == request.UserId).FirstOrDefaultAsync(cancellationToken);

            if (mailingGroup is null)
            {
                return ApiResult<MailingGroupDto>.Failed("Mail group not exists", 404);
            }

            _context.Remove(mailingGroup);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult.Success();
        }
    }
}
