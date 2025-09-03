using EmployeeFrontendClient.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontendClient.Pages
{
    public class CreateModel : PageModel
    {
        private readonly EmployeeService _service;

        public CreateModel(EmployeeService employeeService)
        {
            _service = employeeService;
        }

        [BindProperty]
        public EmployeeModel Employee { get; set; } = new EmployeeModel(0, "", "", "", "");

        public IActionResult OnGet()
        {
            // Check if user is authenticated
            if (!LoginModel.IsUserLoggedIn(HttpContext))
            {
                return RedirectToPage("/Login");
            }

            // Initialize with empty values
            Employee = new EmployeeModel(0, "", "", "", "");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Create a new record with the form data
                var employeeToAdd = new EmployeeModel(
                    0, // ID will be set by the API
                    Employee.Name,
                    Employee.Email,
                    Employee.Address,
                    Employee.Role
                );

                var result = await _service.AddEmployee(employeeToAdd);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Employee created successfully!";
                    return RedirectToPage("EmployeeList");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create employee.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error creating employee: {ex.Message}");
                return Page();
            }
        }
    }
}