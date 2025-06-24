using FluentValidation;
using FluentValidation.AspNetCore;
using InvoinceModule.Application.Mapping;
using InvoinceModule.Application.Ports.In;
using InvoinceModule.Application.Ports.Out;
using InvoinceModule.Application.UseCases;
using InvoinceModule.Application.Validation;
using InvoinceModule.Infrastructure.Adapters.Email;
using InvoinceModule.Infrastructure.Adapters.Invoince;
using InvoinceModule.Infrastructure.Adapters.Jobs;
using InvoinceModule.Infrastructure.Context;
using InvoinceTracker.Execptions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog yapýlandýrmasý
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();
builder.Logging.ClearProviders();

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

// SMTP ayarlarý
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));


// Email gönderim servisi
builder.Services.AddTransient<IEmailSenderOutPort, SmtpEmailSender>();

// Use Case
builder.Services.AddTransient<EmailNotificationUseCase>();

builder.Services.AddHostedService<InvoiceNotificationBackgroundService>();

builder.Services.AddProblemDetails();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
