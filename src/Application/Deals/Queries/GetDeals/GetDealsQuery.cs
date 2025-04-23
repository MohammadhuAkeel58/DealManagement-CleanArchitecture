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

        try
        {
            var deals = await _context.Deals.Include(x => x.Hotels).ToListAsync();
            return _mapper.Map<List<DealsVm>>(deals);
        }
        catch (Exception ex)
        {

            throw new Exception("Error getting deals", ex);
        }


    }
}