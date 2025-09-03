using EmployeeFrontendClient.Models;
using EmployeeFrontendClient.Models.ViewModels;
using EmployeeFrontendClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontendClient.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(AuthService authService, ILogger<RegisterModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public RegisterViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            // Clear any existing session data on GET
            HttpContext.Session.Clear();

            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Call AuthService for registration
                var authResult = await _authService.RegisterAsync(Input);

                if (authResult.Success)
                {
                    // Automatically log in user after successful registration
                    // Store user data in session ONLY
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    HttpContext.Session.SetString("UserId", authResult.UserId.ToString());
                  //  HttpContext.Session.SetString("Username", authResult.Username ?? Input.Username);
                    HttpContext.Session.SetString("Email", Input.Email);

                    // Store token if API returns one
                    if (!string.IsNullOrEmpty(authResult.Token))
                    {
                        HttpContext.Session.SetString("AuthToken", authResult.Token);
                    }

                    // Store registration time
                    HttpContext.Session.SetString("RegisterTime", DateTime.Now.ToString());

                    // Set success message
                    HttpContext.Session.SetString("RegistrationSuccess",
                        $"Welcome {Input.Username}! Your account has been created and you're now logged in.");

                    _logger.LogInformation("User {Username} registered and logged in successfully via session.", Input.Username);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, authResult.Message ?? "Registration failed.");
                    _logger.LogWarning("Registration failed for user {Username}: {Message}", Input.Username, authResult.Message);
                }
            }

            return Page();
        }

        // Helper method to check if user is logged in (same as in Login)
        public static bool IsUserLoggedIn(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString("IsAuthenticated")) &&
                   context.Session.GetString("IsAuthenticated") == "true";
        }
    }
}