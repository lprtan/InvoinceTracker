using InvoinceModule.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace InvoinceTracker.Execptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IProblemDetailsService _problemDetailsService;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IProblemDetailsService problemDetailsService)
        {
            _next = next;
            _logger = logger;
            _problemDetailsService = problemDetailsService;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Bir hata yakalandı: {Message}", ex.Message);

                var (statusCode, title) = ex switch
                {
                    DomainException dex => (dex.StatusCode, "İş Kuralı Hatası"),
                    UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Yetkisiz Erişim"),
                    ArgumentNullException => (StatusCodes.Status400BadRequest, "Eksik veya hatalı veri"),
                    _ => (StatusCodes.Status500InternalServerError, "Sunucu Hatası")
                };

                var problem = new ProblemDetails
                {
                    Title = title,
                    Detail = ex.Message,
                    Status = statusCode,
                    Instance = context.Request.Path
                };

                context.Response.StatusCode = statusCode;

                await _problemDetailsService.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = context,
                    ProblemDetails = problem
                });
            }
        }
    }

}
