using System;
using System.Threading.Tasks;
using app.Data;
using app.Interfaces;
using app.Models;

namespace app.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddEmployeeAsync(string name, int projectId)
        {
            _context.Employees.Add(new Employee
            {
                Name = name,
                ProjectId = projectId
            });
            if (await _context.SaveChangesAsync() < 1)
                throw new Exception("something went wrong");
        }

        public async Task RemoveEmployeeAsync(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new Exception("not employee with this id");
            _context.Employees.Remove(employee);
            if (await _context.SaveChangesAsync() < 1)
                throw new Exception("something went wrong");
        }

        public async Task UpdateEmployeeAsync(int employeeId, string name, int projectId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                throw new Exception("not employee with this id");
            employee.Name = name;
            employee.ProjectId = projectId;
            if (await _context.SaveChangesAsync() < 1)
                throw new Exception("something went wrong");
        }
    }
}