using MediatR;
using Microsoft.AspNetCore.Mvc;
using N5Permissions.Application.Commands.PermissionTypes.CreatePermissionType;
using N5Permissions.Application.Commands.PermissionTypes.DeletePermissionType;
using N5Permissions.Application.Commands.PermissionTypes.UpdatePermissionType;
using N5Permissions.Application.DTOs;
using N5Permissions.Application.Queries.PermissionTypes.GetAllPermissionTypes;
using N5Permissions.Application.Queries.PermissionTypes.GetPermissionTypeById;
using System.Threading.Tasks;

namespace N5Permissions.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PermissionTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllPermissionTypesQuery());
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetPermissionTypeByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionTypeCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionTypeCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _mediator.Send(new DeletePermissionTypeCommand { Id = id });

            if (!success)
                return Ok(new { message = "Nenhum dado encontado" });

            return Ok(new { message = "Dado excluído com êxito" });
        }
    }
}
