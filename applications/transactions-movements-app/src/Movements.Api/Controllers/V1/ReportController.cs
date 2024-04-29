using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movements.Api.Models.Report.Responses;
using Movements.Application.Queries;

namespace Movements.Api.Controllers.V1;

[ApiController]
[ApiVersion(version: "1")]
[Route("V{version:apiVersion}/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{accountId}")]
    public async Task<ActionResult> GetReportAsync(string accountId)
    {
        var query = new GetAccountMovementsReportQuery(accountId);

        var report = await _mediator.Send(query);

        if (report is null)
            return BadRequest("Not found movements for account");

        return Ok(report.ToResponse());
    }
}