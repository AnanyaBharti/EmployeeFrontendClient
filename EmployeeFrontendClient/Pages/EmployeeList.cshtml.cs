using EmployeeFrontendClient.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontEndClient.Pages
{
    public class EmployeeListModel : PageModel
    {
        private readonly EmployeeService _service;

        public EmployeeListModel(EmployeeService employeeService)
        {
            _service = employeeService;
        }

        public List<EmployeeModel> EmployeeModels { get; set; } = new List<EmployeeModel>();

        public async Task OnGetAsync()
        {
            try
            {
                EmployeeModels = await _service.GetEmployees() ?? new List<EmployeeModel>();
            }
            catch (Exception ex)
            {
                EmployeeModels = new List<EmployeeModel>();
                // You might want to add error handling/logging here
                Console.WriteLine($"Error loading employees: {ex.Message}");
            }
        }

        // Add this delete handler
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var result = await _service.DeleteEmployee(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Employee deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to delete employee.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting employee: {ex.Message}";
            }

            return RedirectToPage(); // Refresh the page after delete
        }
    }
}