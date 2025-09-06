using brandiagaAPI2.Dtos;
using brandiagaAPI2.Interfaces.RepositoryInterfaces;
using Microsoft.Extensions.Logging;

namespace brandiagaAPI2.Middleware
{
    public class ApiKeyAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiKeyAuthenticationMiddleware> _logger;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next, ILogger<ApiKeyAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            _logger.LogInformation($"Request Path: {path}, Method: {context.Request.Method}, Response.HasStarted: {context.Response.HasStarted}");

            // Bypass API key check for these paths
            if (path != null && (
                path.StartsWith("/upload", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/auth/google", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/auth/facebook", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/auth/twitter", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/Auth/google/callback", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/Auth/facebook/callback", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/api/Auth/twitter/callback", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/signin-google", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/error", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
                path.StartsWith("/livechatHub", StringComparison.OrdinalIgnoreCase)
            ))
            {
                await _next(context);
                return;
            }

            // Check for API key in header
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(ResponseDTO<object>.Error("API Key is missing."));
                return;
            }

            var configuredApiKey = context.RequestServices.GetRequiredService<IConfiguration>()["ApiKey"];
            if (string.IsNullOrEmpty(configuredApiKey) || extractedApiKey != configuredApiKey)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsJsonAsync(ResponseDTO<object>.Error("Invalid API Key."));
                return;
            }

            await _next(context);
        }

    }

    public static class ApiKeyAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAuthenticationMiddleware>();
        }
    }
}