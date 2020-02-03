using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Model.Database;
using Model.Dto;
using Model.Entity;

namespace MailingGroupNet.Features.Emails.Update
{
    public class Command : IRequest<ApiResult<EmailDto>>, IUserRequest
    {
        public int GroupId { get; set; }
        public int EmailId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }

    public class Handler : IRequestHandler<Command, ApiResult<EmailDto>>
    {
        private readonly MailingGroupContext _context;

        public Handler(MailingGroupContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<EmailDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            Model.Entity.MailingGroup mailingGroup = await _context.MailingGroups
                .Include(x => x.Emails)
                .Where(x => x.UserId == request.UserId)
                .Where(x => x.Id == request.GroupId)
                .FirstOrDefaultAsync(cancellationToken);

            if (mailingGroup is null)
            {
                return ApiResult<EmailDto>.Failed("Mailing group is not exists", 404);
            }

            string newEmail = request.Email.Trim();
            bool isValidEmail = EmailValidator.ValidateEmail(newEmail);

            if (!isValidEmail)
            {
                return ApiResult<EmailDto>.Failed("Email is wrong format", 400);
            }
            Email updatedEmail = mailingGroup.Emails.FirstOrDefault(x => x.Id == request.EmailId);

            if (updatedEmail is null)
            {
                return ApiResult<EmailDto>.Failed("Email is not exists", 404);
            }

            if (mailingGroup.Emails.Where(x=>x.Id != request.EmailId).Any(x => x.Name == request.Email))
            {
                return ApiResult<EmailDto>.Failed("New email is already exists in this group", 409);

            }

            updatedEmail.Name = newEmail;
            _context.Update(updatedEmail);
            await _context.SaveChangesAsync(cancellationToken);
            return ApiResult<EmailDto>.Success(new EmailDto(updatedEmail));
        }
    }
}
