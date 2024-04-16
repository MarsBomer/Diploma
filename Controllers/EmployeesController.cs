using System.Diagnostics;
using Diploma.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Diploma.Models;

namespace Diploma.Controllers;

[Authorize]
public class EmployeesController : Controller
{
    ApplicationContext db;
    private readonly IEmployeesService _employeesService;

    public EmployeesController(ApplicationContext context, IEmployeesService employeesService)
    {
        db = context;
        _employeesService = employeesService;
    }


    public async Task<IActionResult> Index()
    {
        return View(await db.Employees.ToListAsync());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Employee employee)
    {
        await _employeesService.CreateEmployee(employee);

        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
