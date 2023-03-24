using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sense_Capital_XOGameApi.Controllers;
using Sense_Capital_XOGameApi.Data;
using Sense_Capital_XOGameApi.Interfaces;
using Sense_Capital_XOGameApi.Models;
using Sense_Capital_XOGameApi.Repositories;
using Sense_Capital_XOGameApi.RequestModels;
using Sense_Capital_XOGameApi.Services;
using Sense_Capital_XOGameApi.Validation;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApiContext>(options => options.UseMySQL(
    builder.Configuration.GetConnectionString("DefoultConnection")
    ));

// Repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMoveRepository, MoveRepository>();
builder.Services.AddScoped<IGameService, GameService>();

// FluentValidation
builder.Services.AddScoped<IValidator<Player>, PlayerValidator>();
builder.Services.AddScoped<IValidator<RqstCreateGame>, RqstCreateGameValidator>();
builder.Services.AddScoped<IValidator<RqstMakeMove>, RqstMoveValidator>();
builder.Services.AddScoped<IValidator<Game>, GameInitialStateValidator>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "XO game API",
        Description = "API для разработки сайта и мобильного приложения для игры в крестики нолики 3x3 для двух игроков",
        //TermsOfService = new Uri("https://t.me/dielectric_impact"),
        Contact = new OpenApiContact
        {
            Name = "Александр",
            Url = new Uri("https://t.me/dielectric_impact")
        },
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
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
