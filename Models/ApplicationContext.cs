using Microsoft.EntityFrameworkCore;

public class ApplicationContext : DbContext
{
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<EmployeeTask> Tasks { get; set; } = null!;

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
        Database.EnsureCreated();   // создаем базу данных при первом обращении

        InitializeAdminUser().GetAwaiter().GetResult();
    }

    private async Task InitializeAdminUser()
    {
        var user = await Users.FirstOrDefaultAsync(u => u.Login == "Admin");
        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Login = "admin",
                Password = "admin",
                Person = new Employee
                {
                    Position = "Admin",
                    FirstName = "Admin",
                    LastName = "A.",
                    Patronimic = "B.",
                    BirthDate = DateTime.Now,
                    StartDate = DateTime.Now,
                    Email = "adm@adm",
                },
                Roles = new List<Role> { Role.Admin },
                CreationDate = DateTime.Now,
            };

            await Users.AddAsync(user);
            await SaveChangesAsync();
        }
    }
}