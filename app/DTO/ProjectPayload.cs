using System.Collections.Generic;

namespace app.DTO
{
    public class ProjectPayload
    {
        public int ProjectId { get; set; }
        public string Location { get; set; }
        public ICollection<EmployeePayload> Employees { get; set; } = new List<EmployeePayload>();
    }
}