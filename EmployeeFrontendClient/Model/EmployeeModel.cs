using System.Text.Json.Serialization;

namespace EmployeeFrontendClient.Model
{
    public record class EmployeeModel(
        [property:JsonPropertyName("id")] int id,
         [property: JsonPropertyName("name")] string name,
          [property: JsonPropertyName("email")] string Email,
           [property: JsonPropertyName("address")] string Address,
         [property: JsonPropertyName("role")] string Role
        ) 
    {
         

    }
}
