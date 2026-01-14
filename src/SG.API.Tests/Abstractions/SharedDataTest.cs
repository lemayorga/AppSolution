using SG.Application.Bussiness.Security.Auth.Requests;
using SG.Infrastructure.Auth.JwtAuthentication.Models;
using SG.Shared.Responses;

namespace SG.API.Tests.Abstractions;

public class SharedDataTest : IDisposable
{
    #region  Properties  
    public List<SuccessWithIdResponse>? ListIdsData { get; private set; }
    public string? ClientToken { get; private set; }
    public string? ClientTokenRefresh { get; private set; }
    public LoginRequest? UserLogin { get; set; }
    public LoginRequest UserLoginDefault { get => new LoginRequest("sadmin@gmail.com", "PeopleApp.2024", true); }

    #endregion

    #region  Constructor
    public SharedDataTest()
    {
        ListIdsData = new List<SuccessWithIdResponse>();
    }

    public void Dispose()
    {
        ListIdsData = null;
    }

    #endregion

    #region  Operations About ListIdsData 

    public List<SuccessWithIdResponse> AddIdsToListIdData(SuccessWithIdResponse? model)
    {
        ListIdsData ??= [];
        if (model is not null) { ListIdsData.Add(model); }
        return ListIdsData;
    }

    public List<SuccessWithIdResponse> AddIdsToListIdData(List<SuccessWithIdResponse>? model)
    {
        ListIdsData ??= [];
        if (model is not null) { ListIdsData.AddRange(model); }
        return ListIdsData;
    }
    public IEnumerable<int> GetListIdDataValue()
         => (ListIdsData ??= new List<SuccessWithIdResponse>()).Select(x => x.Id) ?? [];

    public IEnumerable<string> GetListIdDataValueQueryParameter(string nameParameter)
        => GetListIdDataValue().Select(x => $"{nameParameter}={x}");

    #endregion

    #region  Opertions Token Client
    public void SaveTokenResponse(TokenResponse tokenResp)
    {
        ClientToken = tokenResp.AccessToken;
        ClientTokenRefresh = tokenResp.RefreshToken;
    }

    public TokenResponse GeTokenResponse()
    => (string.IsNullOrWhiteSpace(ClientToken) && string.IsNullOrWhiteSpace(ClientTokenRefresh)) ? new TokenResponse()
        : new TokenResponse { AccessToken = ClientToken!, RefreshToken = ClientTokenRefresh! };
        

    public void  RemoveTokenResponse()
    {
        ClientToken = null;
        ClientTokenRefresh = null;
    }


    #endregion
}
