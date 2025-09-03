using EmployeeFrontendClient.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.

// Add services to the container
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

// Add Session support (minimal configuration)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
});

// Register AuthService with your API
builder.Services.AddHttpClient<AuthService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7090/api/"); // Your API base URL
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
        UseCookies = true
    };
});

// Register EmployeeService 
builder.Services.AddHttpClient<EmployeeService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7090/"); // Employee API base URL
});

// Register Services
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<AuthService>();

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

// Enable Session middleware
app.UseSession();

// Map Razor Pages
app.MapRazorPages();

app.Run();