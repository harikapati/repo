using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ATMApplication.Persistence;
using ATMApplication.Services;
using ATMApplication.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<InMemoryDatabase>(provider => {
    var db = new InMemoryDatabase();
    db.Accounts.Add(new Account { Id = 1, Type = AccountType.Checking, Balance = 0 });
    db.Accounts.Add(new Account { Id = 2, Type = AccountType.Savings, Balance = 0 });
    return db;
});
builder.Services.AddSingleton<AccountService>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseRouting();
// Enable CORS for development
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});

// Add default route for root URL
app.MapGet("/", () => "Welcome to the ATM Application API!");
app.Run();