using System.Collections.Generic;
using System.Threading.Tasks;
using app.DTO;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repository;

        public ProjectController(IProjectRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectPayload>>> GetProjectsAsync() =>
            Ok(await _repository.GetAllProjectsAsync());

        [Authorize(Policy = "RequireAdminRoleOrModerator")]
        [HttpPost("{location}")]
        public async Task<ActionResult<ProjectPayload>> AddProjectAsync(string location) =>
            Ok(await _repository.AddProjectAsync(location));

        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> AddProjectAsync(int id)
        {
            await _repository.RemoveProjectAsync(id);
            return NoContent();
        }
    }
}