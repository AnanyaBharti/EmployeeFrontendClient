using EmployeeFrontendClient.Model;
using System.Text.Json;

public class EmployeeService
{
    private readonly HttpClient _httpClient;

    public EmployeeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmployeeModel>> GetEmployees()
    {
        try
        {
            await using Stream stream = await _httpClient.GetStreamAsync("api/Employee"); // Now relative
            var employees = await JsonSerializer.DeserializeAsync<List<EmployeeModel>>(stream);
            return employees ?? new List<EmployeeModel>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting employees: {ex.Message}");
            return new List<EmployeeModel>();
        }
    }

    public async Task<EmployeeModel?> GetEmployeeById(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Employee/{id}"); // Now relative
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var employee = JsonSerializer.Deserialize<EmployeeModel>(jsonString, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return employee;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting employee: {ex.Message}");
            return null;
        }
    }

    public async Task<EmployeeModel?> AddEmployee(EmployeeModel employee)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Employee", employee); // Now relative
            response.EnsureSuccessStatusCode();
            var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeModel>();
            return createdEmployee;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding employee: {ex.Message}");
            throw;
        }
    }

    public async Task<bool> DeleteEmployee(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Employee/{id}"); // Now relative
            Console.WriteLine($"Delete response status: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting employee: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateEmployee(int id, EmployeeModel employee)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Employee/{id}", employee); // Now relative
            Console.WriteLine($"Update response status: {response.StatusCode}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating employee: {ex.Message}");
            return false;
        }
    }
}