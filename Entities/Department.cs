using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Department
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; }

    public string Logo { get; set; }

    public int? ParentDepartmentId { get; set; }
    public Department ParentDepartment { get; set; }
    public ICollection<Department> SubDepartments { get; set; }
}
