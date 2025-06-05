using System.Net;
using SG.API.Tests.Abstractions;
using SG.Application.Bussiness.Security.Dtos;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Security;

[CollectionDefinition(nameof(UserController))]
public class UserController  : BaseFunctionalTest
{
    protected override string _url => "api/user";
    private readonly string USER_NAME  = "NewUserTest";
    private readonly string USER_EMAIL  = "demo-test@gmail.com";
    private readonly string USER_PASSWORD  = "MiAppTest123**-";

    public UserController(FunctionalTestWebAppFactory factory) : base(factory) {  } 

    [Fact, Order(0)]
    public async Task Post()
    {
        var body = new  UserCreateDto
        {
            Username = USER_NAME,
            Email = USER_EMAIL,
            Firstname = "MyFirstName",
            Lastname = "MyLastName",
            Password = USER_PASSWORD
        };
        var (response, responseResult)  = await PostRequest<RoleDto>(_url, body);    
        AssertResponseWithContent(response, HttpStatusCode.OK, responseResult);
    }

    [Fact, Order(1)]
    public async Task PostMany()
    {
        var  body  = Enumerable.Range(1, 4).Select(i => new UserCreateDto 
        {  
            Username = $"{USER_NAME}_{i}",
            Email = $"demo-test_{i}@gmail.com",
            Firstname = $"MyFirstName_{i}",
            Lastname = $"MyLastName_{i}",
            Password = USER_PASSWORD
        });

        var (response, responseResult)  = await PostRequest<List<RoleDto>>($"{_url}/addMany", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
    }

    [Fact, Order(2)]
    public async Task Get()
    {
        var (response, responseResult)  = await GetRequest<UserDto[]>(_url);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Theory(), Order(3), CombinatorialData]
    public async Task GetBydId([CombinatorialRange(from: 1, count: 2)]int id)
    {
        var (response, responseResult)  = await GetRequest<UserDto>($"{_url}/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
        Assert.Equal(id, responseResult!.Value!.Id);    
    }

    [Theory(), Order(4), CombinatorialData]
    public async Task Put([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var body  = new UserUpdateDto
        {
            Username =$"{USER_NAME}_{id}",
            Email = $"demo-test_{id}@gmail.com",
            Firstname =  $"MyFirstName_{id}_Change",
            Lastname = $"MyLastName_{id}_Change",
            IsLocked = false,
            IsActive = true
        };
        var (response, responseResult)  = await PutRequest<UserDto>($"{_url}/{id}", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.Equal(id, responseResult!.Value!.Id);    ;    
    }

    [Theory(), Order(5), CombinatorialData]
    public async Task ChangePassword([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var body  = new UserChangePassword
        {
              UserName = $"{USER_NAME}_{id}",
              CurrentPassword = USER_PASSWORD,
              NewPassword = $"{USER_PASSWORD}**",
              EvaluateEmail = true
        };
        var (response, responseResult)  = await PutRequest<bool>($"{_url}/changePassword", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);   
    }

    [Theory(), Order(6), CombinatorialData]
    public async Task ResetPasswordBydIdUser([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var (response, responseResult)  = await PutRequest<bool>($"{_url}/resetPasswordBydIdUser/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);    ;    
    }

    [Theory(), Order(7), CombinatorialData]
    public async Task UpdateStatusIsLockedToFalse([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var (response, responseResult)  = await PutRequest<bool>($"{_url}/updateStatusIsLocked/{id}?status={false}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);    ;    
    }

    [Theory(), Order(8), CombinatorialData]
    public async Task UpdateStatusIsInactived([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var (response, responseResult)  = await PutRequest<bool>($"{_url}/updateStatusIsActived/{id}?status={false}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);    ;    
    }

    [Theory(), Order(10), CombinatorialData]
    public async Task UpdateStatusIsActived([CombinatorialRange(from: 2, count: 1)]int id)
    {
        var (response, responseResult)  = await PutRequest<bool>($"{_url}/updateStatusIsActived/{id}?status={true}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);    ;    
    }
}
