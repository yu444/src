public class ApiKeyAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly List<string> _validApiKeys = new List<string> { "your-api-key" };

    public ApiKeyAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var apiKey = context.Request.Headers["ApiKey"].FirstOrDefault();
        if (string.IsNullOrEmpty(apiKey) || !_validApiKeys.Contains(apiKey))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("1 Invalid API key:" + apiKey);
            return;
        }

        await _next(context);
    }
}