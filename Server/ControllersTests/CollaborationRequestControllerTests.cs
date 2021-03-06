using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

public class CollaborationRequestControllerTests
{

    private readonly Mock<ICollaborationRequestRepository> _repoMock;
    private readonly CollaborationRequestController _controller;

    public CollaborationRequestControllerTests()
    {
        _repoMock = new Mock<ICollaborationRequestRepository>();
        var logMock = new Mock<ILogger<CollaborationRequestController>>();
        _controller = new CollaborationRequestController(logMock.Object,  _repoMock.Object);
    }


    [Fact]
    public async Task FindByStudent_returns_students_requests_from_repo()
    {
        var cb1 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 2,
            Application = "Science",
            Status = CollaborationRequestStatus.Waiting
        };
        var cb2 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 4,
            Application = "Not Science",
            Status = CollaborationRequestStatus.Waiting
        };
        var requests = new List<CollaborationRequestDetailsDTO> { cb1, cb2 }.AsReadOnly();

        _repoMock.Setup(m => m.FindUserType(1)).ReturnsAsync(false);

        _repoMock.Setup(m => m.FindRequestsByStudentAsync(1)).ReturnsAsync(requests);

        var result = await _controller.GetRequestsByUser(1);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(cb1, result.ElementAt(0));
        Assert.Equal(cb2, result.ElementAt(1));
    }

    [Fact]
    public async Task GetRequestsByUser_given_SupervisorId_returns_find_requests_by_supervisor()
    {

        var cb1 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 4,
            Application = "Science",
            Status = CollaborationRequestStatus.Waiting
        };

        var cb2 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 4,
            Application = "Not Science",
            Status = CollaborationRequestStatus.Waiting
        };

        _repoMock.Setup(m => m.FindUserType(4)).ReturnsAsync(true);

        _repoMock.Setup(m => m.FindRequestsBySupervisorAsync(4))
            .ReturnsAsync(new List<CollaborationRequestDetailsDTO> { cb1, cb2 }.AsReadOnly());

        var result = await _controller.GetRequestsByUser(4);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(cb1, result.ElementAt(0));
        Assert.Equal(cb2, result.ElementAt(1));
    }


    [Fact]
    public async Task Create_creates_request()
    {
        var cb1 = new CollaborationRequestDetailsDTO
        {
            StudentId = 1,
            SupervisorId = 2,
            Application = "Science",
            Status = CollaborationRequestStatus.Waiting
        };
        var toCreate = new CollaborationRequestCreateDTO();
        _repoMock.Setup(m => m.CreateAsync(toCreate)).ReturnsAsync(cb1);


        var result = await _controller.Post(toCreate);

        Assert.Equal(cb1, result);
        Assert.NotNull(result);
    }


    [Fact]
    public async Task Delete_given_valid_id_returns_it()
    {
        _repoMock.Setup(m => m.DeleteAsync(1)).ReturnsAsync(1);
        var result = await _controller.Delete(1);
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task Delete_given_invalid_id_returns_null()
    {
        int? number = null;
        _repoMock.Setup(m => m.DeleteAsync(9999)).ReturnsAsync(number);
        var result = await _controller.Delete(9999);
        Assert.Null(result);
    }

    [Fact]
    public async Task Put_returns_what()
    {
        var id = 1;
        var cb = new CollaborationRequestUpdateDTO();
        _repoMock.Setup(m => m.UpdateAsync(id, cb))
            .ReturnsAsync(new CollaborationRequestDetailsDTO { Application = "Sweet" });

        var result = await _controller.Put(id, cb);
        Assert.NotNull(result);
        Assert.Equal("Sweet", result.Application);
    }

    [Fact]
    public async Task GetByIdeaId_returns_ideas()
    {
        var cb1 = new CollaborationRequestDetailsDTO();
        var cb2 = new CollaborationRequestDetailsDTO();
        _repoMock.Setup(m => m.FindRequestsByIdeaAsync(1)).ReturnsAsync(new List<CollaborationRequestDetailsDTO>
                {cb1, cb2}.AsReadOnly());

        var result = await _controller.GetByIdeaId(1);
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(cb1, result.ElementAt(0));
        Assert.Equal(cb2, result.ElementAt(1));
    }
}
