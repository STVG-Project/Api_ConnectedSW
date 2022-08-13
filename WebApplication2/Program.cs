using Microsoft.EntityFrameworkCore;
using WebApplication2.APIs;
using WebApplication2.Model;
using System.Net.Http.Json;

public class Program
{
    public static DuongPhoAPI duongphos = new DuongPhoAPI();

    public static KhachHangAPI khachHangs = new KhachHangAPI();

    public static async Task Main(string[] args)
    {
        await duongphos.GetDuongPhoAsync();
        await khachHangs.GetKhachHangAsync();
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.
        builder.Services.AddDbContext<SqlDbContex>(options => options.UseNpgsql(SqlDbContex.configSql));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<SqlDbContex>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        using (var scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            SqlDbContex datacontext = services.GetRequiredService<SqlDbContex>();
            datacontext.Database.EnsureCreated();
            await datacontext.Database.MigrateAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }


}