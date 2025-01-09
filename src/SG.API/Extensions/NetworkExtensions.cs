using System;

namespace SG.API.Extensions;

public static class NetworkExtensions
{
    public static string GetIpAddress(IHttpContextAccessor httpContextAccessor)
    {
         var ip = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
         return ip!;
    }
}
