namespace FakeApi.Services;

public class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCurrentUserId()
    {
        // Attempt to get the client's IP address from various sources
        var ipAddress = _httpContextAccessor.HttpContext!.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ipAddress) && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
        {
            // Retrieve IP address from forwarded headers
            ipAddress = _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"];
        }

        if (ipAddress == null)
            throw new InvalidOperationException();

        return ipAddress;
    }
}