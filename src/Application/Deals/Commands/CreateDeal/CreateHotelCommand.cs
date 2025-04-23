using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DealClean.Application.Deals.Commands.CreateDeal;

public class CreateHotelCommand : IRequest<HotelVm>
{
    public int HotelId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int DealId { get; set; }
}

public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, HotelVm>
{
    private readonly IApplicationDbContext _context;

    public CreateHotelCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<HotelVm> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existDeal = await _context.Deals.FirstOrDefaultAsync(x => x.Id == request.DealId);
            if (existDeal == null)
            {
                return null;
            }
            var hotel = new Hotel
            {
                Name = request.Name,
                Location = request.Location,
                Description = request.Description,
                DealId = request.DealId
            };

            _context.Hotels.AddAsync(hotel);
            await _context.SaveChangesAsync(cancellationToken);

            return new HotelVm
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Location = hotel.Location,
                Description = hotel.Description
            };
        }
        catch (Exception)
        {

            throw new Exception("Error creating hotel");
        }
    }
}