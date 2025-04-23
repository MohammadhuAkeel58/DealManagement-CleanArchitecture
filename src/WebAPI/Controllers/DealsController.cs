using System;
using System.Threading.Tasks;
using DealClean.Application.Deals.Commands.CreateDeal;
using DealClean.Application.Deals.Commands.DeleteDeal;
using DealClean.Application.Deals.Commands.UpdateDeal;
using DealClean.Application.Deals.Commands.UpdateImage;
using DealClean.Application.Deals.Commands.UpdateVideo;
using DealClean.Application.Deals.Queries.GetDealById;
using DealClean.Application.Deals.Queries.GetDeals;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DealsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDeals()
    {
        try
        {
            var deals = await Mediator.Send(new GetDealsQuery());
            return Ok(deals);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting deals: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong getting deals" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDealById(int id)
    {
        try
        {
            var deal = await Mediator.Send(new GetDealByIdQuery { Id = id });
            if (deal == null)
            {
                return NotFound();
            }
            return Ok(deal);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error getting deal: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong getting deal" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> createDeal([FromForm] CreateDealCommand command)
    {
        try
        {
            var deal = await Mediator.Send(command);
            return Ok(deal);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error making deal: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong making deal" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> updateDeal([FromRoute] int id, [FromForm] UpdateDealCommand command)
    {
        try
        {
            command.Id = id;
            var deal = await Mediator.Send(command);

            if (deal == null)
                return NotFound();

            return Ok(deal);
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error updating deal: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong updating deal" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteDeal([FromRoute] int id)
    {
        try
        {
            var deal = await Mediator.Send(new DeleteDealCommand { Id = id });
            if (deal == 0)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error deleting deal: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong deleting deal" });
        }
    }

    [HttpPut("image/{id}")]
    public async Task<IActionResult> updateImage([FromRoute] int id, [FromForm] UpdateImageCommand command)
    {
        try
        {
            command.Id = id;
            var updateImage = await Mediator.Send(command);
            return Ok(updateImage);
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error updating image: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong updating image" });
        }
    }

    [HttpPut("video/{id}")]
    public async Task<IActionResult> updateVideo([FromRoute] int id, [FromForm] UpdateVideoCommand command)
    {
        try
        {
            command.Id = id;
            var updateVideo = await Mediator.Send(command);
            return Ok(updateVideo);
        }

        catch (Exception ex)
        {
            Console.WriteLine("Error updating video: " + ex.Message);
            return StatusCode(500, new { error = "Something went wrong updating video" });
        }
    }
}