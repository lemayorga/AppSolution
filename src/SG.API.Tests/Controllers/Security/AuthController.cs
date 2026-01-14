using System.Net;
using SG.API.Tests.Abstractions;
using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Application.Bussiness.Security.Auth.Responses;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Security;

[CollectionDefinition(nameof(AuthController))]
public class AuthController(FunctionalTestWebAppFactory factory, SharedDataTest sharedData) : BaseFunctionalTest(factory), IClassFixture<SharedDataTest>
{
    protected override string _url => "api/auth";
    private readonly SharedDataTest _sharedData = sharedData;

    [Fact, Order(0)]
    public async Task LoginSuccesfull()
    {
        var login = _sharedData.UserLoginDefault;
        var (response, responseResult) = await PostRequest<LoginResponse>($"{_url}/login", login);
        AssertResponseWithContent(response, HttpStatusCode.OK, responseResult);
    }

    [Fact, Order(1)]
    public async Task LoginUserNotFound()
    {
        var login = new LoginRequest("correoUsuarioPrueba@gmail.com", "x123.Adb*-", true);
        var (response, responseResult) = await PostRequest<LoginResponse>($"{_url}/login", login);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(responseResult?.IsSuccess);
    }

    [Fact, Order(2)]
    public async Task LogoutUserUnAuthorized()
    {
        var (response, responseResult) = await PostRequest<LogoutResponse>($"{_url}/logout");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(responseResult?.IsSuccess);
    }

    [Fact, Order(3)]
    public async Task LogoutUserWithGenerateLoginSuccesfull()
    {
        var tokenResp = await GenerateToken(_sharedData.UserLoginDefault);
        _sharedData.SaveTokenResponse(tokenResp);
        this.SetClientToken(_sharedData.GeTokenResponse());
        var (response, responseResult) = await PostRequest<LogoutResponse>($"{_url}/logout");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(responseResult?.IsSuccess);
    }


    [Fact, Order(4)]
    public async Task RefreshTokenUserWithGenerateLoginSuccesfull()
    {
        var tokenResp = await GenerateToken(_sharedData.UserLoginDefault);
        _sharedData.SaveTokenResponse(tokenResp);
        this.SetClientToken(_sharedData.GeTokenResponse());
        var body = new RefreshTokenModel { AccessToken = _sharedData.ClientToken!, RefreshToken = _sharedData.ClientTokenRefresh! };
        var (response, responseResult) = await PostRequest<LogoutResponse>($"{_url}/refreshToken", body);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(responseResult?.IsSuccess);
    }

    
    [Fact, Order(5)]
    public async Task RefreshTokenUserFaileWithToken()
    {
        var body = new RefreshTokenModel { AccessToken = _sharedData.ClientToken!, RefreshToken = _sharedData.ClientTokenRefresh! };
        var (response, responseResult) = await PostRequest<LogoutResponse>($"{_url}/refreshToken", body);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.False(responseResult?.IsSuccess);
    }
}