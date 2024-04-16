using Diploma.Enums;
public class EmployeeTask
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? DueDate { get; set; }
    public Employee? Assignee { get; set; }
    public User? CreatedBy { get; set; }
    public EmployeeTaskStatus Status { get; set; }
    public int? HoursSpent { get; set; }
}

