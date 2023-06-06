using StocksAppWithConfiguration.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<FinnhubService>();

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.UseStaticFiles();

app.Run();
