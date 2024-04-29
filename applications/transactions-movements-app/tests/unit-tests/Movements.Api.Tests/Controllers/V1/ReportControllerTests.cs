using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movements.Api.Controllers.V1;
using Movements.Application.Queries;
using Movements.Domain.Entities;
using Xunit;

namespace Movements.Api.Tests.Controllers.V1;

public class ReportControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    
    private readonly ReportController _controller;

    public ReportControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        
        _controller = new ReportController(_mediatorMock.Object);
    }

    [Fact]
    public async Task GetReportAsync_WhenFoundAccount_ShouldReturnOkResponse()
    {
        // Arrange
        const string accountId = "12341-1";

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAccountMovementsReportQuery>(), CancellationToken.None))
            .ReturnsAsync(new MovementReport(new List<Movement>
            {
                new(Guid.NewGuid(), "1231-1", DateTime.Today, 56_00, "XPTO", "")
            }));

        // Act
        var result = await _controller.GetReportAsync(accountId);
        
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
    
    [Fact]
    public async Task GetReportAsync_WhenNotFoundAccount_ShouldReturnNotFound()
    { 
        // Arrange
        const string accountId = "12341-1";

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<GetAccountMovementsReportQuery>(), CancellationToken.None))
            .ReturnsAsync((MovementReport) null);

        // Act
        var result = await _controller.GetReportAsync(accountId);
        
        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}