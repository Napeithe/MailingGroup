using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;

namespace MailingGroupNet.Features.Emails
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        public async Task<ActionResult<ApiResult>> Delete([FromQuery] Delete.Command cmd, CancellationToken token)
        {
            cmd.InjectUserId(User);

            var result = await _mediator.Send(cmd, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<EmailDto>>> Create([FromBody]Create.Command cmd, CancellationToken token)
        {

            cmd.InjectUserId(User);

            ApiResult<EmailDto> result = await _mediator.Send(cmd, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpPut]
        public async Task<ActionResult<ApiResult<EmailDto>>> Update([FromBody]Update.Command updateCommand, CancellationToken token)
        {

            updateCommand.InjectUserId(User);

            ApiResult<EmailDto> result = await _mediator.Send(updateCommand, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }
    }
}
