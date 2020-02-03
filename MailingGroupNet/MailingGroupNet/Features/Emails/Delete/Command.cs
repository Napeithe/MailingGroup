using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;
using Model.Entity;

namespace MailingGroupNet.Features.Emails.Delete
{
    public class Command : IRequest<ApiResult>, IUserRequest
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int EmailId { get; set; }
        public string UserId { get; set; }
    }

    public class Handler : IRequestHandler<Command,ApiResult >
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }

        public async Task<ApiResult> Handle(Command request, CancellationToken cancellationToken)
        {
            Email email = await _context.Emails
                .Include(x=>x.MailingGroup)
                .Where(x=>x.Id == request.EmailId)
                .Where(x=>x.MailingGroup.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (email is null)
            {
                return ApiResult<EmailDto>.Failed("Email is not exists", 404);
            }

            _context.Remove(email);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult.Success();
        }
    }
}
