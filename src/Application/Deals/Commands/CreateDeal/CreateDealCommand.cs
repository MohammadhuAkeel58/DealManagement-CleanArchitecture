using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;
using DealClean.Domain.Service;
using MediatR;

namespace DealClean.Application.Deals.Commands.CreateDeal;

public class CreateDealCommand : IRequest<DealsVm>
{
    public DealDto deal { get; set; }
}

public class CreateDealCommandHandler : IRequestHandler<CreateDealCommand, DealsVm>
{
    private readonly IDealService _dealService;

    public CreateDealCommandHandler(IDealService dealService)
    {
        _dealService = dealService;
    }
    public async Task<DealsVm> Handle(CreateDealCommand request, CancellationToken cancellationToken)
    {
        var deal = new Deal
        {
            Name = request.deal.Name,
            Slug = request.deal.Slug,
            Title = request.deal.Title,
        };
        await _dealService.CreateDealAsync(deal);

        return new DealsVm
        {
            Id = deal.Id,
            Name = deal.Name,
            Slug = deal.Slug,
            Title = deal.Title,

        };


    }
}