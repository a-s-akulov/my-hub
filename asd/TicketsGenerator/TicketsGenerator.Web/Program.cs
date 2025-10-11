using TicketsGenerator.Web;
using TicketsGenerator.Web.Components;
using TicketsGenerator.ServiceDefaults.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();

//builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new("http://apiservice"));

var apiUri = builder.Environment.IsDevelopment() ? "http://localhost:5525" : "https://my-hub-api.onrender.com";
builder.Services.AddHttpClient<TicketsGeneratorApiClient>((services, client) => client.BaseAddress = new(apiUri));

// WakeUp
builder.Services.AddAppWakeUpWorker(apiUri);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
