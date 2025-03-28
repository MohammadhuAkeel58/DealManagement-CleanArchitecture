using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Domain.Service;
using MediatR;

namespace DealClean.Application.Deals.Commands.DeleteDeal;

public class DeleteDealCommand : IRequest<int>
{

    public int Id { get; set; }

}

public class DeleteDealCommandHandler : IRequestHandler<DeleteDealCommand, int>
{
    private readonly IDealService _dealService;

    public DeleteDealCommandHandler(IDealService dealService)
    {
        _dealService = dealService;
    }
    public async Task<int> Handle(DeleteDealCommand request, CancellationToken cancellationToken)
    {
        return await _dealService.DeleteDealAsync(request.Id);
    }
}
