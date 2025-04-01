using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.Queries.GetDeals;
using MediatR;

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

        var deal = await _context.Deals.FindAsync(request.Id);
        return _mapper.Map<DealsVm>(deal);

    }
}