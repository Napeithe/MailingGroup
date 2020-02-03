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

namespace MailingGroupNet.Features.Emails.Create
{
    public class Command: IRequest<ApiResult<EmailDto>>, IUserRequest
    {
        [Required]
        public int GroupId { get; set; }
        [Required]
        public string Email { get; set; }

        public string UserId { get; set; }
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
                .Include(x=>x.Emails)
                .Where(x=>x.UserId == request.UserId)
                .Where(x=>x.Id == request.GroupId)
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

            if (mailingGroup.Emails.Any(x => x.Name == newEmail))
            {
                return ApiResult<EmailDto>.Failed("Email is already exists",409);
            }

            Email email = new Email
            {
                Name = newEmail,
                MailingGroup = mailingGroup,
                MailingGroupId = mailingGroup.Id
            };

            await _context.AddAsync(email, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ApiResult<EmailDto>.Success(new EmailDto(email));
        }
    }
}
