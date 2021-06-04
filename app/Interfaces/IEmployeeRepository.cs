using System.Threading.Tasks;

namespace app.Interfaces
{
    public interface IEmployeeRepository
    {
        Task AddEmployeeAsync(string name, int projectId);
        Task UpdateEmployeeAsync(int employeeId, string name, int projectId);
        Task RemoveEmployeeAsync(int employeeId);
    }
}