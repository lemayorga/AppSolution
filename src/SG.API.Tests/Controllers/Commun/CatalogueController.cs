using System.Net;
using Bogus;
using SG.API.Tests.Abstractions;
using SG.Application.Behaviours.Commun.Catalogue;
using SG.Application.Bussiness.Commun.Catalogues.Requests;
using SG.Shared.Extensions;
using SG.Shared.Responses;
using Xunit.Extensions.Ordering;

namespace SG.API.Tests.Controllers.Commun;

[CollectionDefinition(nameof(CatalogueController))]
public class CatalogueController : BaseFunctionalTest, IClassFixture<SharedDataTest>
{
    protected override string _url => "api/catalogue";
    private readonly SharedDataTest _sharedData;
    public CatalogueController(FunctionalTestWebAppFactory factory, SharedDataTest sharedData) : base(factory)
    {
          _sharedData = sharedData;
     } 

    List<CatalogueCreateRequest> InicializateCreationData(int totalRegister)
    {
        var dataRule = new Faker<CatalogueCreateRequest>()
            .RuleFor(u => u.Group, f => f.Name.FirstName())
            .RuleFor(u => u.Value, f => f.Name.FirstName())
            .RuleFor(u => u.Description, f => f.Name.FirstName());

        var rowsData = dataRule.Generate(totalRegister);
        return rowsData;
    }

    [Fact, Order(0)]
    public async Task Post()
    {
        var body = InicializateCreationData(1).First();
        var (response, responseResult) = await PostRequest<SuccessWithIdResponse>(_url, body);
        _sharedData.AddIdsToListIdData(responseResult.Value); 
        AssertResponseWithContent(response, HttpStatusCode.OK, responseResult);

    }

    [Fact, Order(1)]
    public async Task PostMany()
    {
        var body = InicializateCreationData(3);
        var (response, responseResult) = await PostRequest<List<SuccessWithIdResponse>>($"{_url}/addMany", body);
        _sharedData.AddIdsToListIdData(responseResult.Value); 
        AssertResponseWithContent(response, HttpStatusCode.OK, responseResult); 
    }    

    [Fact, Order(2)]
    public async Task Get()
    {
        var (response, responseResult)  = await GetRequest<CatalogueResponse[]>(_url);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
    }

    [Theory(), Order(3), CombinatorialData]
    public async Task GetBydId([CombinatorialRange(from: 1, count: 2)]int id)
    {
        var (response, responseResult)  = await GetRequest<CatalogueResponse>($"{_url}/{id}");
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);
        Assert.Equal(id, responseResult!.Value!.Id);    
    }

    [Fact, Order(4)]
    public async Task GetByListIds()
    {
        var parameters = _sharedData.GetListIdDataValueQueryParameter("ids");
        var uri = new Uri(_client.BaseAddress!, $"{_url}/getByListIds?").AddQueryParams(parameters);
        var (response, responseResult)  = await GetRequest<CatalogueResponse[]>(uri);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult);        
    }

    [Theory(), Order(5), CombinatorialData]
    public async Task Put([CombinatorialRange(from: 1, count: 3)]int id)
    {
        var body  = new CatalogueUpdateRequest
        {
            Group = "CambioGrupo",            
            Value = $"Cambio de dato _{id}",
            Description = "cambio de dato",
            IsActive = false,
        };
        var (response, responseResult)  = await PutRequest<SuccessWithIdResponse>($"{_url}/{id}", body);
        AssertResponseWithContent(response,HttpStatusCode.OK, responseResult); 
        Assert.Equal(id, responseResult!.Value!.Id);    
    }

    [Theory(), Order(6), CombinatorialData]
    public async Task Remove([CombinatorialRange(from: 1, count: 3)]int id)
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