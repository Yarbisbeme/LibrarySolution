using Library.Infrastructure.Extensions;
using Library.Application.Interfaces;
using Library.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IBookService, LibroService>();
builder.Services.AddScoped<ILoanService, PrestamoService>();

//Controladores y los endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddInfrastructureServices(builder.Configuration);
// JWT y Auth se agregan en Fase 3

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
