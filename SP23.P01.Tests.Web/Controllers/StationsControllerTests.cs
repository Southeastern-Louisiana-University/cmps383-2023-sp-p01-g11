using System.Net;
using FluentAssertions;
using SP23.P01.Tests.Web.Dtos;
using SP23.P01.Tests.Web.Helpers;

namespace SP23.P01.Tests.Web.Controllers;

[TestClass]
public class StationsControllerTests
{
    private WebTestContext context = new();

    [TestInitialize]
    public void Init()
    {
        context = new WebTestContext();
    }

    [TestCleanup]
    public void Cleanup()
    {
        context.Dispose();
    }

    [TestMethod]
    public async Task ListAllStations_Returns200AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();

        //act
        var httpResponse = await webClient.GetAsync("/api/stations");

        //assert
        await httpResponse.AssertTrainStationListAllFunctions();
    }

    [TestMethod]
    public async Task GetStationById_Returns200AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var target = await webClient.GetTrainStation();
        if (target == null)
        {
            Assert.Fail("Make List All stations work first");
            return;
        }

        //act
        var httpResponse = await webClient.GetAsync($"/api/stations/{target.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK, "we expect an HTTP 200 when calling GET /api/stations/{id} ");
        var resultDto = await httpResponse.Content.ReadAsJsonAsync<TrainStationDto>();
        resultDto.Should().BeEquivalentTo(target, "we expect get product by id to return the same data as the list all product endpoint");
    }

    [TestMethod]
    public async Task GetStationById_NoSuchId_Returns404()
    {
        //arrange
        var webClient = context.GetStandardWebClient();

        //act
        var httpResponse = await webClient.GetAsync("/api/stations/999999");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling GET /api/stations/{id} with an invalid id");
    }

    [TestMethod]
    public async Task CreateStation_NoName_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Address = "asd",
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/stations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/stations with no name");
    }

    [TestMethod]
    public async Task CreateStation_NameTooLong_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a".PadLeft(121, '0'),
            Address = "asd",
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/stations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/stations with a name that is too long");
    }

    [TestMethod]
    public async Task CreateStation_NoAddress_ReturnsError()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var target = await webClient.GetTrainStation();
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }
        var request = new TrainStationDto
        {
            Name = "asd",
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/stations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/stations with no description");
    }

    [TestMethod]
    public async Task CreateStation_Returns201AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a",
            Address = "asd",
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/stations", request);

        //assert
        await httpResponse.AssertCreateTrainStationFunctions(request, webClient);
    }

    [TestMethod]
    public async Task UpdateStation_NoName_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a",
            Address = "desc",
        };
        await using var target = await webClient.CreateTrainStation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Name = null;

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/stations/{request.Id}", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/stations/{id} with a missing name");
    }

    [TestMethod]
    public async Task UpdateStation_NameTooLong_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a",
            Address = "desc",
        };
        await using var target = await webClient.CreateTrainStation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Name = "a".PadLeft(121, '0');

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/stations/{request.Id}", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/stations/{id} with a name that is too long");
    }

    [TestMethod]
    public async Task UpdateStation_NoAddress_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a",
            Address = "desc",
        };
        await using var target = await webClient.CreateTrainStation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Address = null;

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/stations/{request.Id}", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/stations/{id} with a missing description");
    }

    [TestMethod]
    public async Task UpdateStation_Valid_Returns200()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Name = "a",
            Address = "desc",
        };
        await using var target = await webClient.CreateTrainStation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Address = "cool new description";

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/stations/{request.Id}", request);

        //assert
        await httpResponse.AssertTrainStationUpdateFunctions(request, webClient);
    }

    [TestMethod]
    public async Task DeleteStation_NoSuchItem_ReturnsNotFound()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Address = "asd",
            Name = "asd"
        };
        await using var itemHandle = await webClient.CreateTrainStation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        var httpResponse = await webClient.DeleteAsync($"/api/stations/{request.Id + 21}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling DELETE /api/stations/{id} with an invalid Id");
    }

    [TestMethod]
    public async Task DeleteStation_ValidItem_ReturnsOk()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Address = "asd",
            Name = "asd",
        };
        await using var itemHandle = await webClient.CreateTrainStation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        var httpResponse = await webClient.DeleteAsync($"/api/stations/{request.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK, "we expect an HTTP 200 when calling DELETE /api/stations/{id} with a valid id");
    }

    [TestMethod]
    public async Task DeleteStation_SameItemTwice_ReturnsNotFound()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new TrainStationDto
        {
            Address = "asd",
            Name = "asd",
        };
        await using var itemHandle = await webClient.CreateTrainStation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        await webClient.DeleteAsync($"/api/stations/{request.Id}");
        var httpResponse = await webClient.DeleteAsync($"/api/stations/{request.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling DELETE /api/stations/{id} on the same item twice");
    }
}
