using System.Collections.Generic;
using System.Threading.Tasks;
using app.DTO;

namespace app.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectPayload>> GetAllProjectsAsync();
        Task<ProjectPayload> AddProjectAsync(string location);
        Task RemoveProjectAsync(int id);
    }
}