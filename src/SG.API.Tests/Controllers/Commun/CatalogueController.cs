using System.Net;
using SG.API.Tests.Abstractions;
using SG.Application.Bussiness.Commun.Dtos;
using SG.Shared.Extensions;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Commun;

[CollectionDefinition(nameof(CatalogueController))]
public class CatalogueController : BaseFunctionalTest
{
    protected override string _url => "api/catalogue";

    public CatalogueController(FunctionalTestWebAppFactory factory) : base(factory) {  } 

    CatalogueCreateDto [] register = 
    {
        new CatalogueCreateDto { Group = "group1", Value = "valor 1", Description = "Demo" },
        new CatalogueCreateDto { Group = "group1", Value = "valor 2", Description = "Demo" },
        new CatalogueCreateDto { Group = "group1", Value = "valor 3", Description = "Demo" },
        new CatalogueCreateDto { Group = "group2", Value = "valor 1", Description = "Demo" },
        new CatalogueCreateDto { Group = "group2", Value = "valor grupo 2", Description = "Demo" },
        new CatalogueCreateDto { Group = "group2", Value = "valor grupo 2" },
        new CatalogueCreateDto { Group = "group3", Value = "valor grupo 3" },
    };
    IEnumerable<int> listIdsRegister { get => Enumerable.Range(1, register.Length + 1); }

    [Fact, Order(0)]
    public async Task Post()
    {
        var body = register.First();
        var (response, responseResult)  = await PostRequest<CatalogueDto>(_url, body);    
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Fact, Order(1)]
    public async Task PostMany()
    {
        var body = register.Skip(1).ToArray();
        var (response, responseResult)  = await PostRequest<List<CatalogueDto>>($"{_url}/addMany", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
    }    

    [Fact, Order(2)]
    public async Task Get()
    {
        var (response, responseResult)  = await GetRequest<CatalogueDto[]>(_url);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Theory(), Order(3), CombinatorialData]
    public async Task GetBydId([CombinatorialRange(from: 1, count: 5)]int id)
    {
        var (response, responseResult)  = await GetRequest<CatalogueDto>($"{_url}/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
        Assert.Equal(id, responseResult!.Value!.Id);    
    }

    [Fact, Order(4)]
    public async Task GetByListIds()
    {
        var parameters = listIdsRegister.Select(x => $"ids={x}");
        var uri = new Uri(_client.BaseAddress!, $"{_url}/getByListIds?").AddQueryParams(parameters);
        var (response, responseResult)  = await GetRequest<CatalogueDto[]>(uri);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);        
    }

    [Theory(), Order(5), CombinatorialData]
    public async Task Put([CombinatorialRange(from: 1, count: 5)]int id)
    {
        var data = register.First();
        var body  = new CatalogueUpdateDto
        {
            Group = data.Group,            
            Value = "Cambio de dato",
            Description = "cambio de dato",
            IsActive = false,
        };
        var (response, responseResult)  = await PutRequest<CatalogueDto>($"{_url}/{id}", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.Equal(id, responseResult!.Value!.Id);    
        Assert.Equal(body.Value, responseResult!.Value!.Value);    
    }

    [Theory(), Order(6), CombinatorialData]
    public async Task Remove([CombinatorialRange(from: 1, count: 5)]int id)
    {
        var (response, responseResult)  = await DeleteRequest<bool>($"{_url}/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.True(responseResult!.Value!);    
    }    
}

//https://code-maze.com/dotnet-test-rest-api-xunit/
/// https://medium.com/@parserdigital/testing-asp-net-core-8-0-apis-a-comprehensive-guide-42dc3b2a751a
/// //https://thecodebuzz.com/order-unit-test-cases-or-integration-testing-guidelines/
/// // https://andrewlock.net/simplifying-theory-test-data-with-xunit-combinatorial/