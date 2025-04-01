using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DealClean.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DealClean.Application.Deals.Queries.GetDeals;

public class GetDealsQuery : IRequest<List<DealsVm>>
{

}

public class GetDealsQueryHandler : IRequestHandler<GetDealsQuery, List<DealsVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDealsQueryHandler(IApplicationDbContext Context, IMapper mapper)
    {
        _context = Context;
        _mapper = mapper;
    }
    public async Task<List<DealsVm>> Handle(GetDealsQuery request, CancellationToken cancellationToken)
    {
        var deals = await _context.Deals.ToListAsync();
        // return deals.Select(deal => new DealsVm
        // {
        //     Id = deal.Id,
        //     Name = deal.Name,
        //     Slug = deal.Slug,
        //     Title = deal.Title,
        //     Image = deal.Image,
        //     Video = deal.Video?.Path,
        //     VideoAltText = deal.Video?.AltText,
        // }).ToList();

        return _mapper.Map<List<DealsVm>>(deals);

    }
}