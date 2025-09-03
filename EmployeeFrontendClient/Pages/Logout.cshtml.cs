using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeFrontendClient.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger)
        {
            _logger = logger;
        }

        public string? Username { get; set; }
        public bool IsLoggedIn { get; set; }

        public void OnGet()
        {
            // Check if user is logged in and get username for display
            IsLoggedIn = !string.IsNullOrEmpty(HttpContext.Session.GetString("IsAuthenticated")) &&
                        HttpContext.Session.GetString("IsAuthenticated") == "true";

            Username = HttpContext.Session.GetString("Username");
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            // Get user info before clearing session (for logging)
            var username = HttpContext.Session.GetString("Username");
            var email = HttpContext.Session.GetString("Email");
            var userId = HttpContext.Session.GetString("UserId");

            // Clear all session data
            HttpContext.Session.Clear();

            // Set logout message in session (after clearing, so it persists)
            HttpContext.Session.SetString("LogoutMessage",
                $"You have been logged out successfully. See you soon{(!string.IsNullOrEmpty(username) ? ", " + username : "")}!");

            // Log the logout action
            _logger.LogInformation("User {Username} (ID: {UserId}) logged out successfully.",
                username ?? "Unknown", userId ?? "Unknown");

            // Redirect to return URL or home page
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }

        // Helper method to check if user is logged in
        public static bool IsUserLoggedIn(HttpContext context)
        {
            return !string.IsNullOrEmpty(context.Session.GetString("IsAuthenticated")) &&
                   context.Session.GetString("IsAuthenticated") == "true";
        }

        // Helper method to get current user info from session
        public static class SessionHelper
        {
            public static string? GetUsername(HttpContext context)
            {
                return context.Session.GetString("Username");
            }

            public static string? GetEmail(HttpContext context)
            {
                return context.Session.GetString("Email");
            }

            public static string? GetUserId(HttpContext context)
            {
                return context.Session.GetString("UserId");
            }

            public static string? GetAuthToken(HttpContext context)
            {
                return context.Session.GetString("AuthToken");
            }

            public static void ClearSession(HttpContext context)
            {
                context.Session.Clear();
            }
        }
    }
}