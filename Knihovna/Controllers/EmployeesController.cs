using Knihovna.DTO;
using Knihovna.Services;
using Microsoft.AspNetCore.Mvc;

namespace Knihovna.Controllers
{
    public class EmployeesController : Controller
    {
        private EmployeeService _employeeService;

        public EmployeesController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        //*******************************
        //********* INDEX  ************
        //*******************************
        public async Task<IActionResult> Index()
        {
            var allEmployees = await _employeeService.GetAllAsync();
            return View(allEmployees);
        }
        //*******************************
        //********* CREATE START ************
        //*******************************
        public IActionResult Create()
        {
            return View();
        }
        //*******************************
        //********* CREATE END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> CreateAsync(EmployeeDto employeeDto)
        {
           await _employeeService.CreateAsync(employeeDto);
            return Redirect("Index");
        }
        //*******************************
        //********* EDIT START  ************
        //*******************************
        public async Task<IActionResult> EditAsync(int id)
        {
            var employeeToEdit = await _employeeService.GetByIdAsync(id);
            if (employeeToEdit == null)
            {
                return View("NotFound");
            }
            return View(employeeToEdit);
        }
        //*******************************
        //********* EDIT END  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(EmployeeDto employeeDto)
        {
            await _employeeService.EditAsync(employeeDto);
            return Redirect("Index");
        }
        //*******************************
        //********* DELETE  ************
        //*******************************
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            EmployeeDto employeeToDelete = await _employeeService.GetByIdAsync(id);
            if (employeeToDelete == null)
            {
                return View("NotFound");
            }
            await _employeeService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
