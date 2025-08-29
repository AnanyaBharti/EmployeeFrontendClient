using EmployeeFrontendClient.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontendClient.Pages
{
    public class EditModel : PageModel
    {
        private readonly EmployeeService _service;

        public EditModel(EmployeeService service)
        {
            _service = service;
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; } = new EmployeeModel(0, "", "", "", "");

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var employee = await _service.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                Employee = employee;
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading employee: {ex.Message}");
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Create a new record with updated values (since records are immutable)
                var updatedEmployee = new EmployeeModel(
                    Employee.id,
                    Employee.name,
                    Employee.Email,
                    Employee.Address,
                    Employee.Role
                );

                var result = await _service.UpdateEmployee(Employee.id, updatedEmployee);
                if (result)
                {
                    TempData["SuccessMessage"] = "Employee updated successfully!";
                    return RedirectToPage("EmployeeList");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update employee.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating employee: {ex.Message}");
                ModelState.AddModelError("", $"Error updating employee: {ex.Message}");
                return Page();
            }
        }
    }
}