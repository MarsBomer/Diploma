using Microsoft.EntityFrameworkCore;

namespace Diploma.Services
{
    public interface IEmployeesService
    {
        Task<List<Employee>> GetEmployees();
        Task CreateEmployee(Employee employee);
        Task<Employee> GetEmployee(Guid guid);
    }

    public class EmployeesService : IEmployeesService
    {
        ApplicationContext db;

        public EmployeesService(ApplicationContext db)
        {
            this.db = db;
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await db.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployee(Guid guid)
        {
            return await db.Employees.FindAsync(guid);
        }

        public async Task CreateEmployee(Employee employee)
        {
            employee.Id = Guid.NewGuid();
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
        }
    }
}
