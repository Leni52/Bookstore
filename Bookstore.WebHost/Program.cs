using AutoMapper;
using Bookstore.Application;
using Bookstore.Application.Mappings;
using Bookstore.Application.Requests;
using Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsAuthors;
using Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsBooks;
using Bookstore.Application.Requests.Commands.Validators.ValidatorsCommandsOrders;
using Bookstore.Application.Requests.Queries.Validators.ValidatorsQueriesAuthors;
using Bookstore.Application.Requests.Queries.Validators.ValidatorsQueriesBooks;
using Bookstore.Application.Requests.Queries.Validators.ValidatorsQueriesOrders;
using Bookstore.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

//cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("MVCApp",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

//validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateBookCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateBookCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(DeleteBookCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(FetchAllBooksQueryValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(FetchAllAuthorsQueryValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CreateAuthorCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(UpdateAuthorCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(MakeOrderCommandValidator)));
builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(FetchAllOrdersQueryValidator)));

//builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//automapper 
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<BookProfile>();
    cfg.AddProfile<OrderProfile>();
});
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MVCApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
