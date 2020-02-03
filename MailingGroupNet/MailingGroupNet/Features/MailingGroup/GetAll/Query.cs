using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;

namespace MailingGroupNet.Features.MailingGroup.GetAll
{
    public class Query : IRequest<ApiResult<List<MailingGroupItemListDto>>>, IUserRequest
    {
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Query, ApiResult<List<MailingGroupItemListDto>>>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<List<MailingGroupItemListDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<MailingGroupItemListDto> mailingGroups = await _context.MailingGroups
                .OrderBy(x=>x.Name)
                .Include(x=>x.Emails)
                .Where(x => x.UserId == request.UserId)
                .Select(x => new MailingGroupItemListDto
                {
                    Name = x.Name,
                    Id = x.Id,
                    NumberOfEmails = x.Emails.Count
                }).ToListAsync(cancellationToken);

            return ApiResult<List<MailingGroupItemListDto>>.Success(mailingGroups);
        }
    }
}
