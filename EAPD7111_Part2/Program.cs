using EAPD7111_Part2.Models;
using EAPD7111_Part2.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register DbContext with retry logic for SQL Server
builder.Services.AddDbContext<GlmsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,                        // retry up to 5 times
            maxRetryDelay: TimeSpan.FromSeconds(10), // wait up to 10s between retries
            errorNumbersToAdd: null                  // use default transient errors
        )
    )
);

// ✅ Register your services for dependency injection
builder.Services.AddHttpClient<CurrencyS>();   // Currency service uses HttpClient
builder.Services.AddScoped<ServiceRequestS>(); // Service request workflow service

// ✅ Add MVC controllers + views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ✅ Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// ✅ Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
