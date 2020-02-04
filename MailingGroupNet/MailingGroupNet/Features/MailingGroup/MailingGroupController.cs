using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MailingGroupNet.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;

namespace MailingGroupNet.Features.MailingGroup
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MailingGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MailingGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<List<MailingGroupItemListDto>>>> GetAll(CancellationToken token)
        {
            var getAll = new GetAll.Query();
            getAll.InjectUserId(User);
            ApiResult<List<MailingGroupItemListDto>> result = await _mediator.Send(getAll, token);
            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResult<MailingGroupDto>>> Get([FromRoute] int id, CancellationToken token)
        {
            var get = new Get.Query
            {
                Id = id
            };
            get.InjectUserId(User);
            ApiResult<MailingGroupDto> result = await _mediator.Send(get, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResult>> Delete([FromRoute] int id, CancellationToken token)
        {
            var cmd = new Delete.Command
            {
                Id = id
            };
            cmd.InjectUserId(User);

            var result = await _mediator.Send(cmd, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResult<MailingGroupItemListDto>>> Create([FromBody]Create.Command cmd, CancellationToken token)
        {

            cmd.InjectUserId(User);

            ApiResult<MailingGroupItemListDto> result = await _mediator.Send(cmd, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }

        [HttpPut]
        public async Task<ActionResult<ApiResult<MailingGroupDto>>> Update([FromBody]Update.Command updateCommand, CancellationToken token)
        {

            updateCommand.InjectUserId(User);

            ApiResult<MailingGroupDto> result = await _mediator.Send(updateCommand, token);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, result.Message);
            }

            return result;
        }
    }
}
