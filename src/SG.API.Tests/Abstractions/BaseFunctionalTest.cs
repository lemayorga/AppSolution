using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SG.API.Tests.Extensions;
using SG.Application.Base.Responses;
using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using SG.Shared.Responses;
namespace SG.API.Tests.Abstractions;

public abstract class BaseFunctionalTest : IClassFixture<FunctionalTestWebAppFactory>
{
    #region  Properties

    protected HttpClient _client { get; init; }
    protected abstract string _url { get; }
    private const string _jsonMediaType = "application/json";
    private readonly System.Text.Json.JsonSerializerOptions _jsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };
    protected readonly TestAuthHelper _authHelper;
    private readonly string _requestUriLogin = "api/Auth/login";

    #endregion

    #region  Builder
    public BaseFunctionalTest(FunctionalTestWebAppFactory factory)
    {
        _client = factory.CreateClient();
        _authHelper = new TestAuthHelper(_client, _jsonMediaType);
    }
    #endregion

    #region  Operations Token


    protected async Task<TokenResponse> GenerateToken(LoginRequest UserLogin)
    {
        var (userName, password) = UserLogin;
        var tokenResp = await _authHelper.GetAccessTokenAsync($"{_requestUriLogin}", userName, password);
        return tokenResp;
    }

    protected HttpClient SetClientToken(TokenResponse tokenResp)
    {
        return SetClientToken(tokenResp?.AccessToken ?? string.Empty);
    }

    protected HttpClient SetClientToken(string token)
    {
        if(!string.IsNullOrWhiteSpace(token))
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return _client;
    }

    #endregion

    #region  Operations  HttpClient

    private StringContent? GetJsonStringContent<T>(T? dado)
    {
        StringContent? body = dado is not null ? new StringContent(JsonConvert.SerializeObject(dado), Encoding.UTF8, _jsonMediaType) : null;
        return body;
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> GetRequest<T>(string requestUri)
    {
        var response = await _client.GetAsync(requestUri);
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> GetRequest<T>(Uri requestUri)
    {
        var response = await _client.GetAsync(requestUri);
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }


    protected async Task<(HttpResponseMessage, OperationResult<T>)> PostRequest<T>(string requestUri, object? data = null)
    {
        var response = await _client.PostAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> PostRequest<T>(Uri requestUri, object? data = null)
    {
        var response = await _client.PostAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> DeleteRequest<T>(string requestUri)
    {
        var response = await _client.DeleteAsync(requestUri);
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> DeleteRequest<T>(Uri requestUri)
    {
        var response = await _client.DeleteAsync(requestUri);
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> PutRequest<T>(string requestUri, object? data = null)
    {
        var response = await _client.PutAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> PutRequest<T>(Uri requestUri, object? data = null)
    {
        var response = await _client.PutAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> PathRequest<T>(string requestUri, object? data = null)
    {
        var response = await _client.PatchAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    protected async Task<(HttpResponseMessage, OperationResult<T>)> PathRequest<T>(Uri requestUri, object? data = null)
    {
        var response = await _client.PatchAsync(requestUri, GetJsonStringContent(data));
        var responseResult = await response.GetAndDeserialize<T>();
        return (response, responseResult);
    }

    #endregion

    #region  Operations  AssertResponse

    protected void AssertResponseWithContent(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode)
    {
        AssertCommonResponseParts(response, expectedStatusCode);
        Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
    }
    protected void AssertResponseWithContent<T>(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode, OperationResult<T> expectedContent)
    {
        AssertCommonResponseParts(response, expectedStatusCode);
        Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
        Assert.Equal(true, expectedContent?.IsSuccess);
    }

    protected async Task AssertResponseWithContentAsync<T>(HttpResponseMessage response, System.Net.HttpStatusCode expectedStatusCode, T? expectedContent)
    {
        AssertCommonResponseParts(response, expectedStatusCode);
        Assert.Equal(_jsonMediaType, response.Content.Headers.ContentType?.MediaType);
        if (expectedContent is not null)
            Assert.Equal(expectedContent, await System.Text.Json.JsonSerializer.DeserializeAsync<T?>(await response.Content.ReadAsStreamAsync(), _jsonSerializerOptions));
    }

    private static void AssertCommonResponseParts(HttpResponseMessage response, HttpStatusCode expectedStatusCode)
    {
        Assert.Equal(expectedStatusCode, response.StatusCode);
    }

    #endregion
}