using Microsoft.Extensions.DependencyInjection;
using NotesClient.Components;
using NotesClient.Services;
using NotesClient.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container before building the app
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Configure HttpClient
builder.Services.AddHttpClient(
    "NotesApi",
    client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["Api:Url"]!, UriKind.Absolute);
    }
);

// Register the NotesService with DI
builder.Services.AddScoped<INotesService, NotesService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
