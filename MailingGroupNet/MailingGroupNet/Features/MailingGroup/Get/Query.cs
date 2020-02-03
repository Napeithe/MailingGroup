using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;

namespace MailingGroupNet.Features.MailingGroup.Get
{
    public class Query: IRequest<ApiResult<MailingGroupDto>>, IUserRequest
    {
        public int Id { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, ApiResult<MailingGroupDto>>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<MailingGroupDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            MailingGroupDto mailingGroupDto = await _context.MailingGroups
                .Where(x=>x.Id == request.Id)
                .Where(x=>x.UserId == request.UserId)
                .Include(x=>x.Emails)
                .Select(x=>new MailingGroupDto(x.Id, x.Name)
                {
                    Emails = x.Emails.Select(e=>new EmailDto
                    {
                        Name = e.Name,
                        Id = e.Id
                    }).OrderBy(e=>e.Name).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (mailingGroupDto is null)
            {
                return ApiResult<MailingGroupDto>.Failed("Mail group not exists", 404);
            }

            return ApiResult<MailingGroupDto>.Success(mailingGroupDto);
        }
    }
}
