using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using DealClean.Domain.Entities;
using MediatR;

namespace DealClean.Application.Deals.Commands.UpdateVideo;

public class UpdateVideoCommand : IRequest<DealsVm>
{
    public VideoDto VideoDt { get; set; }
}

public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UpdateVideoCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<DealsVm> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
    {

        VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.VideoDt.VideoFile, "Videos", request.VideoDt.AltText, false, cancellationToken);
        var deal = _context.Deals.Find(request.VideoDt.Id);
        if (deal == null) return null;

        deal.Video = videoInfo;

        return new DealsVm
        {
            Id = deal.Id,
            Name = deal.Name,
            Slug = deal.Slug,
            Title = deal.Title,
            Video = deal.Video?.Path,
            VideoAltText = deal.Video?.AltText,

        };

    }
}