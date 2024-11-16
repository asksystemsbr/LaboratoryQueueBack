using Microsoft.EntityFrameworkCore;
using laboratoryqueue.Data;
using laboratoryqueue.Interfaces;
using laboratoryqueue.Services;
using laboratoryqueue.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers(); // Adicionar suporte a Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Registrar serviços
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IPrinterService, PrinterService>();

// Adicionar SignalR
builder.Services.AddSignalR();

// Configurar CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        builder =>
//        {
//            builder
//                .AllowAnyMethod()
//                .AllowAnyHeader()
//                .AllowCredentials()
//                .SetIsOriginAllowed(_ => true); // Cuidado: em produção, especifique as origens permitidas
//        });
//});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder => builder
            .WithOrigins("http://localhost:8080") // URL do seu app Vue
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usar CORS
app.UseCors("AllowAll");

// Adicionar suporte a roteamento
app.UseRouting();

app.UseAuthorization();

// Mapear controllers e hub do SignalR
app.MapControllers();
app.MapHub<QueueHub>("/queueHub");

app.Run();