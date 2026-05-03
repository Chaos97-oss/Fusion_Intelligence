using Microsoft.Extensions.Primitives;

namespace Fusion.API.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // Only protect write endpoints (POST, PUT, PATCH, DELETE)
        if (context.Request.Method == HttpMethod.Post.Method ||
            context.Request.Method == HttpMethod.Put.Method ||
            context.Request.Method == HttpMethod.Patch.Method ||
            context.Request.Method == HttpMethod.Delete.Method)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out StringValues extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided. (Header: X-Api-Key)");
                return;
            }

            var appSettingsApiKey = configuration.GetValue<string>("ApiKey");

            if (!appSettingsApiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }
        }

        await _next(context);
    }
}
