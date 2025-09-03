using EmployeeFrontendClient.Model;
using EmployeeFrontendClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontendClient.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(AuthService authService, ILogger<LoginModel> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [BindProperty]
        public LoginViewModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public void OnGet(string? returnUrl = null)
        {
            // Clear any existing session data on GET
            HttpContext.Session.Clear();

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                // Call AuthService for login
                var authResult = await _authService.LoginAsync(Input);

                if (authResult.Success)
                {
                    // Store user data in session ONLY
                    HttpContext.Session.SetString("IsAuthenticated", "true");
                    HttpContext.Session.SetString("UserId", authResult.UserId.ToString());
                    HttpContext.Session.SetString("Username", authResult.Username ?? Input.Email);
                    HttpContext.Session.SetString("Email", Input.Email);

                    // Store token if API returns one
                    if (!string.IsNullOrEmpty(authResult.Token))
                    {
                        HttpContext.Session.SetString("AuthToken", authResult.Token);
                    }

                    // Store login time
                    HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString());

                    // Set success message
                    HttpContext.Session.SetString("LoginSuccess", $"Welcome back, {authResult.Username ?? Input.Email}!");

                    _logger.LogInformation("User {Email} logged in successfully via session.", Input.Email);

                    return LocalRedirect(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, authResult.Message ?? "Login failed.");
                    _logger.LogWarning("Login failed for user {Email}: {Message}", Input.Email, authResult.Message);
                }
            }

            return Page();
        }

        // Helper method to check if user is logged in (can be used in other pages)
        public static bool IsUserLoggedIn(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString("IsAuthenticated")) &&
                   context.Session.GetString("IsAuthenticated") == "true";
        }
    }
}