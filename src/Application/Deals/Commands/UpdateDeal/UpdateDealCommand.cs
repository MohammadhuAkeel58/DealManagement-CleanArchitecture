using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using MediatR;

namespace DealClean.Application.Deals.Commands.UpdateDeal;

public class UpdateDealCommand : IRequest<DealsVm>
{
    public DealDto deal { get; set; }
}

public class UpdateDealCommandHandler : IRequestHandler<UpdateDealCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;

    public UpdateDealCommandHandler(IApplicationDbContext context)
    {

        _context = context;
    }
    public async Task<DealsVm> Handle(UpdateDealCommand request, CancellationToken cancellationToken)
    {
        var deal = await _context.Deals.FindAsync(request.deal.Id);
        if (deal == null) return null;

        deal.Name = request.deal.Name;
        deal.Slug = request.deal.Slug;
        deal.Title = request.deal.Title;

        return new DealsVm
        {
            Id = deal.Id,
            Name = deal.Name,
            Slug = deal.Slug,
            Title = deal.Title,

        };


    }
}

