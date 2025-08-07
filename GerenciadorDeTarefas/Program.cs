using Microsoft.EntityFrameworkCore;
using GerenciadorDeTarefas.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TarefaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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