using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Projetc.TechChallenge.FIAP.Data;
using Projetc.TechChallenge.FIAP.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do repositório com a interface.
builder.Services.AddScoped<IContatctRepository, ContactRepository>();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
