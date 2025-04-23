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

namespace DealClean.Application.Deals.Commands.UpdateDeal;

public class UpdateDealCommand : IRequest<DealsVm>
{
    public int Id { get; set; } // Added Id property to identify the deal to update
    public string Name { get; set; }        // Flattened properties
    public string Slug { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; } // for to store the path
    public IFormFile? ImageFile { get; set; }
    public IFormFile? VideoFile { get; set; }
    public string? VideoAltText { get; set; }
}

public class UpdateDealCommandHandler : IRequestHandler<UpdateDealCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UpdateDealCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {

        _context = context;
        _fileUploadService = fileUploadService;
    }
    public async Task<DealsVm> Handle(UpdateDealCommand request, CancellationToken cancellationToken)
    {

        var deal = await _context.Deals.FindAsync(request.Id);
        if (deal == null) return null;
        var imageInfo = await _fileUploadService.UploadFileAsync(request.ImageFile, "Images", null, false, cancellationToken);
        VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.VideoFile, "Videos", request.VideoAltText, true, cancellationToken);

        deal.Name = request.Name;
        deal.Slug = request.Slug;
        deal.Title = request.Title;
        deal.Image = imageInfo.Path;
        deal.Video = videoInfo;

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

        };


    }
}

