public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Person Person { get; set; }
    public List<Role> Roles { get; set; }
    public DateTime CreationDate { get; set; }
}