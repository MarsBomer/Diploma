using Diploma.Models;
using Microsoft.EntityFrameworkCore;
using Diploma.Enums;
using Diploma.Services;

namespace Diploma.Services
{
    public interface ITaskService
    {
        Task CreateEmployeeTask(CreateTaskViewModel model, Guid currentUserId);
        Task<List<EmployeeTask>> GetAllTasks();
        Task<List<EmployeeTask>> GetTasksByEmployee(Guid employeeId);
        Task<List<EmployeeTask>> GetTasksWithoutAssignee();
        Task<List<EmployeeTask>> GetTasksCreatedByCurrentUser(Guid currentUserId);
        Task UpdateTaskStatus(Guid userId, Guid taskId, EmployeeTaskStatus status, int? hoursSpent);
    }

    public class TaskService : ITaskService
    {
        ApplicationContext _context;
        private readonly IEmployeesService _employeesService;

        public TaskService(ApplicationContext context, IEmployeesService employeesService)
        {
            _context = context;
            _employeesService = employeesService;
        }

        public async Task<List<EmployeeTask>> GetAllTasks()
        {
            return await _context.Tasks.Include(t => t.Assignee).ToListAsync();
        }

        public async Task<List<EmployeeTask>> GetTasksByEmployee(Guid employeeId)
        {
            return await _context.Tasks.Where(t => t.Assignee != null && t.Assignee.Id == employeeId).ToListAsync();
        }

        public async Task<List<EmployeeTask>> GetTasksWithoutAssignee()
        {
            return await _context.Tasks.Where(t => t.Assignee == null).ToListAsync();
        }

        public async Task<List<EmployeeTask>> GetTasksCreatedByCurrentUser(Guid currentUserId)
        {
            return await _context.Tasks.Where(t => t.CreatedBy != null && t.CreatedBy.Id == currentUserId).ToListAsync();
        }

        public async Task CreateEmployeeTask(CreateTaskViewModel model, Guid currentUserId)
        {
            var task = new EmployeeTask();

            task.DueDate = model.DueDate;
            task.Title = model.Title;
            task.Description = model.Description;
            task.CreationDate = DateTime.Now;
            task.CreatedBy = _context.Users.FirstOrDefault(e => e.Id == currentUserId);

            if (model.AssigneeId != null)
                task.Assignee = _context.Employees.FirstOrDefault(e => e.Id == model.AssigneeId);

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskStatus(Guid userId, Guid taskId, EmployeeTaskStatus status, int? hoursSpent)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
            {
                throw new Exception("Задача не найдена!");
            }

            if (status == EmployeeTaskStatus.Done && !hoursSpent.HasValue)
            {
                throw new Exception("Необходимо указать затраченное время!");
            }

            task.Status = status;
            
            if (status == EmployeeTaskStatus.InProgress)
            {
                task.Assignee = await _employeesService.GetEmployee(userId);
            }

            if (hoursSpent.HasValue)
            {
                task.HoursSpent = hoursSpent.Value;
            }

            await _context.SaveChangesAsync();
        }
    }
}
