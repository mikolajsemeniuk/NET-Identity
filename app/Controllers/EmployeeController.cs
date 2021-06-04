using System.Threading.Tasks;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [Authorize(Policy = "RequireModerateRole")]
        [HttpPost("{name}/{projectId}")]
        public async Task<ActionResult> AddProjectAsync(string name, int projectId)
        {
            await _repository.AddEmployeeAsync(name, projectId);
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRoleOrModerator")]
        [HttpPut("{employeeId}/{name}/{projectId}")]
        public async Task<ActionResult> UpdateProjectAsync(int employeeId, string name, int projectId)
        {
            await _repository.UpdateEmployeeAsync(employeeId, name, projectId);
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{employeeId}")]
        public async Task<ActionResult> AddProjectAsync(int employeeId)
        {
            await _repository.RemoveEmployeeAsync(employeeId);
            return NoContent();
        }
    }
}