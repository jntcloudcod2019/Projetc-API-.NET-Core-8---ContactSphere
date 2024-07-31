using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Interfaces;
using Projetc.TechChallenge.FIAP.Services;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do repositório com a interface.
builder.Services.AddScoped<IContatctRepository, ContactRepository>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddMetrics();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ILogService, LogService>();

// Registro do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contatcs", Version = "v1" });

    // Customizando os exemplos do Swagger
    c.MapType<ContactCreateDto>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            ["name"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("string") },
            ["phone"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("string") },
            ["email"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("user@example.com") },
            ["ddd"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("string") },
        },
        Required = new HashSet<string> { "name", "phone", "email", "ddd" }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tech Chagelland - F2");
        c.RoutePrefix = string.Empty;  // Isso fará com que o Swagger UI seja acessível na URL base
    });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseHttpMetrics();
app.UseMetricServer();
app.UseHttpMetrics();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
