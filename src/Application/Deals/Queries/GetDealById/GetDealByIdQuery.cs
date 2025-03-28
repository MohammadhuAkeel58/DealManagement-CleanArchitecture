using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Service;
using MediatR;

namespace DealClean.Application.Deals.Queries.GetDealById;

public class GetDealByIdQuery : IRequest<DealsVm>
{

    public int Id { get; set; }
}

public class GetDealByIdQueryHandler : IRequestHandler<GetDealByIdQuery, DealsVm>
{
    private readonly IDealService _dealService;
    private readonly IMapper _mapper;

    public GetDealByIdQueryHandler(IDealService dealService, IMapper mapper)
    {
        _dealService = dealService;
        _mapper = mapper;
    }
    public async Task<DealsVm> Handle(GetDealByIdQuery request, CancellationToken cancellationToken)
    {

        var deal = await _dealService.GetDealByIdAsync(request.Id);
        return _mapper.Map<DealsVm>(deal);

    }
}