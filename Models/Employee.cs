public class Employee : Person
{
    public EmployeeStatus Status { get; set; }
    public Guid? HeadId { get; set; }
    public string Position { get; set; }
    public DateTime StartDate { get; set; }
}