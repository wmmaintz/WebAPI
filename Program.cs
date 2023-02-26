using Microsoft.EntityFrameworkCore;
using WebApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoContext>(opt =>
	opt.UseInMemoryDatabase("TodoList"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "WebApi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
	// app.UseSwaggerUI(c =>
	// {
	// 	c.SwaggerEndpoint("/swagger/v1/swagger.json", "MaintzAPI");
	// });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
