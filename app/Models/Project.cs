using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace app.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string Location { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}