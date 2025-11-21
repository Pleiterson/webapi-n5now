using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Permissions.Application.Commands.Permissions.CreatePermission;
using N5Permissions.Application.Commands.Permissions.DeletePermission;
using N5Permissions.Application.Commands.Permissions.UpdatePermission;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Queries.Permissions.GetAllPermissions;
using N5Permissions.Application.Queries.Permissions.GetPermissionById;
using System.Threading.Tasks;

namespace N5Permissions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPermissionsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPermissionByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionCommand command)
        {
            var id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");
            var result =await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeletePermissionCommand { Id = id });

            return Ok(new { message = "Dado excluído com êxito" });
        }
    }
}
