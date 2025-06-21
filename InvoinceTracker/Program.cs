using FluentValidation;
using FluentValidation.AspNetCore;
using InvoinceModule.Application.Mapping;
using InvoinceModule.Application.Ports.In;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Application.UseCases;
using InvoinceModule.Application.Validation;
using InvoinceModule.Infrastructure.Adapters.Concrete;
using InvoinceModule.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(InvoinceMappingProfile));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<InvoiceDtoValidator>();

// Repository ve Adapter Baðýmlýlýklarý
builder.Services.AddScoped<IInvoiceRepositoryOutPort, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceNumberGeneratorOutPort, InvoiceNumberGeneratorRepository>();

// UseCase (Input Port) Kaydý
builder.Services.AddScoped<IInvoiceHeaderInputPort, InvoiceHeaderUseCase>();

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
