using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.Queries.GetDeals;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DealClean.Application.Deals.Queries.GetDealById;

public class GetDealByIdQuery : IRequest<DealsVm>
{

    public int Id { get; set; }
}

public class GetDealByIdQueryHandler : IRequestHandler<GetDealByIdQuery, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDealByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {

        _context = context;
        _mapper = mapper;
    }
    public async Task<DealsVm> Handle(GetDealByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var deal = await _context.Deals.Include(d => d.Hotels).FirstOrDefaultAsync(d => d.Id == request.Id);
            if (deal == null) return null;
            return _mapper.Map<DealsVm>(deal);
        }
        catch (Exception ex)
        {

            throw new Exception("Error getting deal by id", ex);
        }


    }
}