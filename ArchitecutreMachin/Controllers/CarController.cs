using _2.Application.Context;
using _2.Application.Entities;
using _2.Application.Repositories.Interfaces;
using ArchitecutreMachin.Models;
using ArchitecutreMachin.Models.Cars;
using ArchitecutreMachins.Features.Commands.DeleteCar;
using ArchitecutreMachins.Features.Queries.GetAllCars;
using ArchitecutreMachins.Features.Queries.GetCarById;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ArchitecutreMachins.Features.Commands.CreateCars;
using ArchitecutreMachins.Features.Commands.UpdateCar;

namespace ArchitecutreMachin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class CarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CarController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateCarCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllCarsQuery query)
        {
            //var result = await _mediator.Send(new GetAllCarsQuery());

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetCarByIdQuery { Id = id });

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCarCommand command)
        {
            if (id != command.Id)
                return BadRequest("Id mismatch");

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return Ok("Updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCarCommand { Id = id });

            if (!result)
                return NotFound();

            return Ok("Deleted successfully");
        }
    }
}

