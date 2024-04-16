namespace Diploma.Models
{
    public class CreateTaskViewModel
    {
        public List<Employee> Employees { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid? AssigneeId { get; set; }
    }
}
