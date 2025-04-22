using System;
using System.Collections.Generic;
using System.Linq;
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
public class Deals : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDeals()
    {
        var deals = await Mediator.Send(new GetDealsQuery());
        return Ok(deals);
    }



    [HttpGet("{id}")]
    public async Task<IActionResult> GetDealById(int id)
    {
        var deal = await Mediator.Send(new GetDealByIdQuery { Id = id });
        if (deal == null)
        {
            return NotFound();
        }
        return Ok(deal);
    }

    [HttpPost]
    public async Task<IActionResult> createDeal([FromForm] CreateDealCommand command)
    {
        var deal = await Mediator.Send(command);
        return Ok(deal);
    }



    [HttpPut("{id}")]
    public async Task<IActionResult> updateDeal([FromRoute] int id, [FromBody] UpdateDealCommand command)
    {
        if (id != command.deal.Id)
        {
            return BadRequest();
        }
        await Mediator.Send(command);
        return NoContent();

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteDeal([FromRoute] int id, DeleteDealCommand command)
    {
        await Mediator.Send(new DeleteDealCommand { Id = id });
        return NoContent();
    }


    [HttpPut("image/{id}")]
    public async Task<IActionResult> updateImage([FromRoute] int id, [FromForm] UpdateImageCommand command)

    {
        command.ImageDt.Id = id;
        var updateImage = await Mediator.Send(command);
        return Ok(updateImage);

    }

    [HttpPut("video/{id}")]
    public async Task<IActionResult> updateVideo([FromRoute] int id, [FromForm] UpdateVideoCommand command)

    {
        command.VideoDt.Id = id;
        var updateVideo = await Mediator.Send(command);
        return Ok(updateVideo);

    }



}