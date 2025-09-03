using EmployeeFrontendClient.Model;
using EmployeeFrontendClient.Models;
using EmployeeFrontendClient.Models.ViewModels;
using System.Text;
using System.Text.Json;

namespace EmployeeFrontendClient.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient httpClient, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AuthResult> RegisterAsync(RegisterViewModel model)
        {
            try
            {
                var registerData = new
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                var json = JsonSerializer.Serialize(registerData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/register", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    AuthApiResponse? result = null;

                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        // Handle empty response body by creating a default success response
                        result = new AuthApiResponse
                        {
                            Success = true,
                            Message = "Registration completed",
                            Username = model.Username,
                            UserId = 0
                        };
                    }
                    else
                    {
                        result = JsonSerializer.Deserialize<AuthApiResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }

                    return new AuthResult
                    {
                        Success = result?.Success ?? false,
                        Message = result?.Message ?? "Registration completed",
                        Username = result?.Username ?? model.Username,
                        UserId = result?.UserId ?? 0
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Registration failed: {StatusCode} - {Content}", response.StatusCode, errorContent);

                    return new AuthResult
                    {
                        Success = false,
                        Message = $"Registration failed: {response.StatusCode}"
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during registration");
                return new AuthResult
                {
                    Success = false,
                    Message = "Unable to connect to the server. Please try again."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration");
                return new AuthResult
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }

        public async Task<AuthResult> LoginAsync(LoginViewModel model)
        {
            try
            {
                var loginData = new
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    AuthApiResponse? result = null;

                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        // Handle empty response indicating success (adjust as per backend)
                        result = new AuthApiResponse
                        {
                            Success = true,
                            Message = "Login completed",
                            Username = model.Email,
                            UserId = 0
                        };
                    }
                    else
                    {
                        result = JsonSerializer.Deserialize<AuthApiResponse>(responseContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    }

                    return new AuthResult
                    {
                        Success = result?.Success ?? false,
                        Message = result?.Message ?? "Login completed",
                        Username = result?.Username ?? model.Email,
                        UserId = result?.UserId ?? 0,
                        Token = result?.Token
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Login failed: {StatusCode} - {Content}", response.StatusCode, errorContent);

                    return new AuthResult
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during login");
                return new AuthResult
                {
                    Success = false,
                    Message = "Unable to connect to the server. Please try again."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login");
                return new AuthResult
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again."
                };
            }
        }
    }

    // Response models
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Username { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
    }

    public class AuthApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Username { get; set; }
        public int? UserId { get; set; }
        public string? Token { get; set; }
    }
}