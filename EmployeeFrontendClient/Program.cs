var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Register HttpClient with base address
builder.Services.AddHttpClient<EmployeeService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7090/");
});

// Or register EmployeeService as scoped
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();