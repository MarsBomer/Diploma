using Diploma.Models;
using Diploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Diploma.Enums;
using Diploma.Services;

namespace Diploma.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IAuthService _authService;
        private readonly IEmployeesService _employeeService;

        public TasksController(ITaskService taskService, IAuthService authService, IEmployeesService employeeService)
        {
            _taskService = taskService;
            _authService = authService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _taskService.GetAllTasks());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employees = await _employeeService.GetEmployees();
            var model = new CreateTaskViewModel { Employees = employees };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskViewModel model)
        {
            var currentUserId = _authService.GetCurrentUserId(HttpContext);

            await _taskService.CreateEmployeeTask(model, currentUserId);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid taskId, EmployeeTaskStatus status, int? hoursSpent = null)
        {
            var userId = _authService.GetCurrentUserId(HttpContext);

            await _taskService.UpdateTaskStatus(userId, taskId, status, hoursSpent);
            return RedirectToAction("Index");
        }
    }
}
