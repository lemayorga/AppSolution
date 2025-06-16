using System.Net;
using SG.API.Tests.Abstractions;
using SG.Application.Bussiness.Security.Dtos;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Security;

[CollectionDefinition(nameof(RoleController))]
public class RoleController  : BaseFunctionalTest
{
    protected override string _url => "api/role";
    public RoleController(FunctionalTestWebAppFactory factory) : base(factory) {  } 


    [Fact, Order(0)]
    public async Task Post()
    {
        var body = new  RoleCreateDto
        {
            CodeRol = "CODE_ROL_NEW_TEST",
            Name = "Soy el nuevo rol creado temporalmente"
        };
        var (response, responseResult)  = await PostRequest<RoleDto>(_url, body);    
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Fact, Order(1)]
    public async Task PostMany()
    {
        var  body  = Enumerable.Range(1, 5).Select(i => new RoleCreateDto 
        {  
            CodeRol = $"CODE_{i}_ROL_NEW_TEST",  
            Name = $"Soy el rol #_{i} creado temporalmente" 
        });

        var (response, responseResult)  = await PostRequest<List<RoleDto>>($"{_url}/addMany", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
    } 

    [Fact, Order(2)]
    public async Task Get()
    {
        var (response, responseResult)  = await GetRequest<RoleDto[]>(_url);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Theory(), Order(3), CombinatorialData]
    public async Task GetBydId([CombinatorialRange(from: 1, count: 5)]int id)
    {
        var (response, responseResult)  = await GetRequest<RoleDto>($"{_url}/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
        Assert.Equal(id, responseResult!.Value!.Id);    
    }

    [Theory(), Order(4), CombinatorialData]
    public async Task Put([CombinatorialRange(from: 1, count: 2)]int id)
    {
        var body  = new RoleUpdateDto
        {
            CodeRol = $"CODE_ROL_UPDATE_TEST",  
            Name = $"Soy el rol  cambiado temporalmente",
            IsActive = false
        };
        var (response, responseResult)  = await PutRequest<RoleDto>($"{_url}/{id}", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.Equal(id, responseResult!.Value!.Id);    
        Assert.Equal(body.IsActive, responseResult!.Value!.IsActive);    
    }

    [Fact, Order(5)]
    public async Task GetUsersWithRoles()
    {
        var body = new FilterUsersRoles
        {
            CodeRol = $"CODE_{3}_ROL_NEW_TEST",  
        };
        var (response, responseResult)  = await PostRequest<IEnumerable<UserRolesDto>>($"{_url}/usersWithRoles", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }
}
