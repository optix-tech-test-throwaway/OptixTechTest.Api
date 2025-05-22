using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OptixTechTest.Api.Middleware;
using OptixTechTest.Api.v1;
using OptixTechTest.Api.v1.Validators;
using OptixTechTest.Core.Models;
using OptixTechTest.Core.Services;
using OptixTechTest.Domain;
using OptixTechTest.Domain.Services;
using Scalar.AspNetCore;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContextPool<MoviesDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    opts.UseSeeding((context, _) => SeedData.Seed(context));
    opts.UseAsyncSeeding((context, _, ct) => SeedData.SeedAsync(context, ct));
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IValidator<MovieSearchInput>, MovieSearchInputValidator>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetService<MoviesDbContext>();

db?.Database.MigrateAsync();

app.MapOpenApi();
app.MapScalarApiReference();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler();

app
    .MapGroup("/api/v1/movies")
    .MapMoviesApiV1()
    .ProducesProblem(StatusCodes.Status500InternalServerError);

app.UseHttpsRedirection();
app.Run();

public partial class Program;
