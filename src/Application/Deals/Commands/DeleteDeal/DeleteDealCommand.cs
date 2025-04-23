using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using MediatR;

namespace DealClean.Application.Deals.Commands.DeleteDeal;

public class DeleteDealCommand : IRequest<int>
{

    public int Id { get; set; }

}

public class DeleteDealCommandHandler : IRequestHandler<DeleteDealCommand, int>
{
    private readonly IApplicationDbContext _context;

    public DeleteDealCommandHandler(IApplicationDbContext context)
    {

        _context = context;
    }
    public async Task<int> Handle(DeleteDealCommand request, CancellationToken cancellationToken)
    {
        var deal = await _context.Deals.FindAsync(request.Id);
        if (deal == null) return 0;
        _context.Deals.Remove(deal);
        return await _context.SaveChangesAsync(cancellationToken);

    }
}
