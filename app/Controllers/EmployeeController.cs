using System;
using System.Security.Claims;
using System.Threading.Tasks;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(IEmployeeRepository repository, ILogger<EmployeeController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [Authorize(Policy = "RequireModerateRole")]
        [HttpPost("{name}/{projectId}")]
        public async Task<ActionResult> AddEmployeeAsync(string name, int projectId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nickname = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation($"[AddEmployeeAsync] id of current user is: {id}, and nickname is: {nickname}");
            await _repository.AddEmployeeAsync(name, projectId);
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRoleOrModerator")]
        [HttpPut("{employeeId}/{name}/{projectId}")]
        public async Task<ActionResult> UpdateEmployeeAsync(int employeeId, string name, int projectId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nickname = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation($"[UpdateEmployeeAAsync] id of current user is: {id}, and nickname is: {nickname}");
            await _repository.UpdateEmployeeAsync(employeeId, name, projectId);
            return Ok();
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{employeeId}")]
        public async Task<ActionResult> RemoveEmployeeAsync(int employeeId)
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nickname = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation($"[RemoveEmployeeAsync] id of current user is: {id}, and nickname is: {nickname}");
            await _repository.RemoveEmployeeAsync(employeeId);
            return NoContent();
        }
    }
}