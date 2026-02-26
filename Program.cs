using Microsoft.EntityFrameworkCore;
using TodoApi.Repository;
using TodoApi.Service;
using TodoApi.Common;
//using Microsoft.IdentityModel.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        LogHelper.SetConfig();

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpLogging(o => { });

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        builder.Services.AddControllers();

        builder.Services.AddDbContext<TodoDb>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ITodoRepository, TodoRepository>();
        builder.Services.AddScoped<ITodoService, TodoService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });
        
        var app = builder.Build();

        app.UseHttpLogging();

        //app.Logger.LogInformation("Adding Routes");
        LogHelper.loginfo.Info("Entering application.");
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.MapControllers();
        app.UseCors("AllowAll");
        app.UseFileServer();
        app.Logger.LogInformation("Starting the app");
        await app.RunAsync();
    }
}
