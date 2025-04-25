using System.Net;
using SG.API.Tests.Abstractions;
using SG.Application.Bussiness.Security.Dtos;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Security;

[CollectionDefinition(nameof(UserController))]
public class UserController  : BaseFunctionalTest
{
    protected override string _url => "api/user";
    public UserController(FunctionalTestWebAppFactory factory) : base(factory) {  } 

    [Fact, Order(0)]
    public async Task Post()
    {
        var body = new  UserCreateDto
        {
            Username = "NewUserTest",
            Email = "demo-test@gmail.com",
            Firstname = "MyFirstName",
            Lastname = "MyLastName",
            Password = "MiAppTest123**-"
        };
        var (response, responseResult)  = await PostRequest<RoleDto>(_url, body);    
        AssertResponseWithContent(response, HttpStatusCode.OK, responseResult);
    }

    [Fact, Order(1)]
    public async Task PostMany()
    {
        var  body  = Enumerable.Range(1, 4).Select(i => new UserCreateDto 
        {  
            Username = $"NewUserTest_{i}",
            Email = $"demo-test_{i}@gmail.com",
            Firstname = $"MyFirstName_{i}",
            Lastname = $"MyLastName_{i}",
            Password = "MiAppTest123**-"
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
    public async Task Put([CombinatorialRange(from: 1, count: 2)]int id)
    {
        var body  = new UserUpdateDto
        {
            Username = "NewUserTest",
            Email = "demo-test@gmail.com",
            Firstname = "MyFirstName_Change",
            Lastname = "MyLastName_Change",
            IsLocked = false,
            IsActive = true
        };
        var (response, responseResult)  = await PutRequest<UserDto>($"{_url}/{id}", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.Equal(id, responseResult!.Value!.Id);    ;    
    }
}
