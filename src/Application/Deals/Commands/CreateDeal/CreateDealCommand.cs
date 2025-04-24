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
    public ICollection<HotelDto>? Hotels { get; set; } = new List<HotelDto>();
}

public class CreateDealCommandHandler : IRequestHandler<CreateDealCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;
    private readonly IHotelFileUploadService _hotelFileUploadService;

    public CreateDealCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService, IHotelFileUploadService hotelFileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
        _hotelFileUploadService = hotelFileUploadService;
    }
    public async Task<DealsVm> Handle(CreateDealCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var imageInfo = await _fileUploadService.UploadFileAsync(request.ImageFile, "Images", null, false, cancellationToken);
            VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.VideoFile, "Videos", request.VideoAltText, true, cancellationToken);

            var hotels = new List<Hotel>();

            if (request.Hotels != null && request.Hotels.Any())
            {
                foreach (var hotelDto in request.Hotels)
                {

                    var hotelMedia = await _hotelFileUploadService.UploadHotelMediaAsync(
                        hotelDto.ImageFiles,
                        hotelDto.ImageAltTexts,
                        hotelDto.VideoFiles,
                        hotelDto.VideoAltTexts,
                        cancellationToken);

                    var hotel = new Hotel
                    {
                        Name = hotelDto.Name,
                        Location = hotelDto.Location,
                        Description = hotelDto.Description,
                        Media = hotelMedia
                    };

                    hotels.Add(hotel);
                }
            }

            var deal = new Deal
            {
                Name = request.Name,
                Slug = request.Slug,
                Title = request.Title,
                Image = imageInfo?.Path,
                Video = videoInfo,
                Hotels = hotels
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
                    HotelId = x.HotelId,
                    Name = x.Name,
                    Location = x.Location,
                    Description = x.Description,
                    Media = x.Media.Select(m => new HotelMediaVm
                    {
                        Path = m.Path,
                        AltText = m.AltText,
                        IsVideo = m.IsVideo
                    }).ToList()
                }).ToList()
            };
        }
        catch (Exception ex)
        {

            throw new Exception("An error occurred while creating the deal.", ex);
        }


    }
}