using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;
using MediatR;

namespace DealClean.Application.Deals.Commands.UpdateDeal;

public class UpdateDealCommand : IRequest<DealsVm>
{
    public DealDto deal { get; set; }
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
        var imageInfo = await _fileUploadService.UploadFileAsync(request.deal.ImageFile, "", null, false, cancellationToken);
        VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.deal.VideoFile, "Videos", request.deal.VideoAltText, false, cancellationToken);
        var deal = await _context.Deals.FindAsync(request.deal.Id);
        if (deal == null) return null;

        deal.Name = request.deal.Name;
        deal.Slug = request.deal.Slug;
        deal.Title = request.deal.Title;
        deal.Image = imageInfo.Path;
        deal.Video = videoInfo;

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

