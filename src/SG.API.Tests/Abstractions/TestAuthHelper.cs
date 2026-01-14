using System.Text;
using Newtonsoft.Json;
using SG.API.Tests.Extensions;
using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Application.Bussiness.Security.Auth.Responses;
using SG.Infrastructure.Auth.JwtAuthentication.Models;

namespace SG.API.Tests.Abstractions;

public  class TestAuthHelper
{
    private readonly HttpClient _authClient;
    private readonly string _encoding;

    public TestAuthHelper(HttpClient authClient, string encoding)
    {
        _authClient = authClient;
        _encoding = encoding;
    }


    private StringContent? GetJsonStringContent<T>(T? dado)
    {
        StringContent? body = dado is not null ? new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, _encoding) : null;
        return body;
    }
    
    public async Task<TokenResponse> GetAccessTokenAsync(string requestUri, string username, string password)
    {
        var tokenResponse = await GetUserAccessTokenAsync(requestUri, new LoginRequest(userName: username, password: password, evaluateEmail: true));
        return tokenResponse;
    }
    
    private async Task<TokenResponse> GetUserAccessTokenAsync(string requestUri,LoginRequest login)
    {
        var responseHttp = await _authClient.PostAsync(requestUri, GetJsonStringContent(login));
        var responseResult = await responseHttp.GetAndDeserialize<LoginResponse>();

        if (responseResult.IsFailed)
            throw new Exception($"Token request error: {string.Join(",", responseResult.Errors ?? [])}");

        return responseResult.Value!.Tokens;
    }
}
