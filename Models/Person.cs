using System.ComponentModel.DataAnnotations.Schema;

public class Person
{
    public Guid Id { get; set; } 
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronimic { get; set; }
    public DateTime BirthDate { get; set; }
    public string Email { get; set; }

    [NotMapped]
    public string FullName => LastName + ' ' + FirstName + ' ' + Patronimic;
}