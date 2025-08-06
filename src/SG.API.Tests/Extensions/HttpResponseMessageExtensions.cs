using Newtonsoft.Json;
using SG.Application.Base.Responses;

namespace SG.API.Tests.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task<OperationResult<T>> GetAndDeserialize<T>(this HttpResponseMessage response)
    {
        if(!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Succesfull response");
        }

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var resp = JsonConvert.DeserializeObject<OperationResult<T>>(result);
        return resp!;
    }
}