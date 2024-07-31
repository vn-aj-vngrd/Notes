using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NotesApi.Data;

namespace NotesApi.IntegrationTests;

internal class NotesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("NotesList"));

            var dbContext = services.BuildServiceProvider().GetService<AppDbContext>();
            dbContext?.Database.EnsureDeleted();
        });
    }
}
