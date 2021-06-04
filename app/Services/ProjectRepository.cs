using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Data;
using app.DTO;
using app.Interfaces;
using app.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace app.Services
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DataContext _context;

        public ProjectRepository(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IEnumerable<ProjectPayload>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(project => project.Employees)
                .Select(project => new ProjectPayload
                {
                    ProjectId = project.ProjectId,
                    Location = project.Location,
                    Employees = project.Employees.Select(employee => new EmployeePayload
                    {
                        EmployeeId = employee.EmployeeId,
                        Name = employee.Name,
                        ProjectId = project.ProjectId
                    }).ToList()
                })
                .ToListAsync();
        }

        [Authorize]
        public async Task<ProjectPayload> AddProjectAsync(string location)
        {
            var project = new Project
            {
                Location = location
            };
            _context.Projects.Add(project);
            if (await _context.SaveChangesAsync() < 1)
                throw new Exception("something went wrong");
            return new ProjectPayload
            {
                ProjectId = project.ProjectId,
                Location = project.Location
            };
        }

        [Authorize]
        public async Task RemoveProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                throw new Exception("invalid id");
            _context.Projects.Remove(project);
            if (await _context.SaveChangesAsync() < 1)
                throw new Exception("something went wrong");
        }
    }
}