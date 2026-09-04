using backend.Data;
using Microsoft.EntityFrameworkCore;

using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(
        new JsonStringEnumConverter()
    );
});
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
    );
});

builder.Services.AddProblemDetails();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    DbSeeder.Seed(context);
}

app.UseExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Angular");

app.UseAuthorization();

app.MapControllers();

app.Run();
