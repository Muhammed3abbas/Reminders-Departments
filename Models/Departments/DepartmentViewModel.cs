using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace RingoMediaReminder.Models.Departments
{
    public class DepartmentViewModel
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Logo { get; set; }
        public int? ParentDepartmentId { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; }
    }

}
