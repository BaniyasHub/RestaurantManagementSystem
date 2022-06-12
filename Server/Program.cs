using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Server.Models;
using Server.Services;
using Server.Tools;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<Database>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<ITimeTool, TimeTool>();
builder.Services.AddTransient<IRestaurantTool, RestaurantTool>();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader()
           .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

var app = builder.Build();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.UseRouting();
app.UseGrpcWeb();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<RestaurantService>().EnableGrpcWeb()
                                              .RequireCors("AllowAll");
});

app.Run();