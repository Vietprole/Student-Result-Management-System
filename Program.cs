﻿using Microsoft.EntityFrameworkCore;
using Student_Result_Management_System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowLocalHost",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5500")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

builder.Services.AddControllers();

// Swagger setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowLocalHost");

app.UseAuthorization();

app.MapControllers();

app.Run();
