using System.Text;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;
        private readonly BikeStoresDbContext _dbContext;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger, BikeStoresDbContext dbContext)
        {
            _next = next;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                var requestBody = await ReadRequestBodyAsync(context.Request);

                _logger.LogInformation($"API Request: {context.Request.Method} {context.Request.Path}");
                _logger.LogInformation($"Request Body: {requestBody}");

                var originalBodyStream = context.Response.Body;
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;
                    await _next(context);

                    responseBody.Seek(0, SeekOrigin.Begin);
                    var responseText = await new StreamReader(responseBody).ReadToEndAsync();
                    _logger.LogInformation($"Response: {responseText}");
                    responseBody.Seek(0, SeekOrigin.Begin);

                    await LogToDatabase(context, requestBody, responseText);
                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using (var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                request.Body.Position = 0;
                return body;
            }
        }

        private async Task LogToDatabase(HttpContext context, string requestBody, string responseText)
        {
            var auditLog = new AuditLogs
            {
                EventType = "API Request",
                UserId = "Sys",
                RequestPath = context.Request.Path,
                RequestMethod = context.Request.Method,
                RequestBody = requestBody,
                ResponseBody = responseText,
                EventDate = DateTime.UtcNow
            };

            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}

