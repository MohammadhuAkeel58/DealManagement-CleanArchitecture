using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Deals.Commands.CreateDeal;

public class CreateDealCommand : IRequest<DealsVm>
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }
    public IFormFile? ImageFile { get; set; }
    public IFormFile? VideoFile { get; set; }
    public string? VideoAltText { get; set; }
    public ICollection<Hotel>? Hotels { get; set; } = new List<Hotel>();
}

public class CreateDealCommandHandler : IRequestHandler<CreateDealCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public CreateDealCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
    }
    public async Task<DealsVm> Handle(CreateDealCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var imageInfo = await _fileUploadService.UploadFileAsync(request.ImageFile, "Images", null, false, cancellationToken);
            VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.VideoFile, "Videos", request.VideoAltText, true, cancellationToken);
            var deal = new Deal
            {
                Name = request.Name,
                Slug = request.Slug,
                Title = request.Title,
                Image = imageInfo?.Path,
                Video = videoInfo,
                Hotels = request.Hotels?.Select(h => new Hotel
                {
                    Name = h.Name,
                    Location = h.Location,
                    Description = h.Description,

                }).ToList()
            };


            _context.Deals.Add(deal);

            await _context.SaveChangesAsync(cancellationToken);

            return new DealsVm
            {
                Id = deal.Id,
                Name = deal.Name,
                Slug = deal.Slug,
                Title = deal.Title,
                Image = deal.Image,
                Video = deal.Video?.Path,
                VideoAltText = deal.Video?.AltText,
                Hotels = deal.Hotels.Select(x => new HotelVm
                {
                    Name = x.Name,
                    Location = x.Location,
                    Description = x.Description
                }).ToList()

            };

        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while creating the deal.", ex);
        }


    }
}