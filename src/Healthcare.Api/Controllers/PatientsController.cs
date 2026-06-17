using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Healthcare.Application.Features.Patients.Commands;
using Healthcare.Application.Features.Patients.Queries;


namespace Healthcare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly ISender _sender;

        public PatientsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn(CheckInCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients([FromQuery] GetPatientsQuery query,CancellationToken cancellationToken)
        {
            var result = await _sender.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatientById(Guid Id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetPatientByIdQuery(Id), cancellationToken);
            return Ok(result);
        }
    }
}
