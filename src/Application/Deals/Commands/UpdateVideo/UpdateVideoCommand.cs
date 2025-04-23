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

namespace DealClean.Application.Deals.Commands.UpdateVideo;

public class UpdateVideoCommand : IRequest<DealsVm>
{
    public int Id { get; set; }
    public IFormFile? VideoFile { get; set; }
    public string? VideoAltText { get; set; }
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
        try
        {
            VideoInfo? videoInfo = await _fileUploadService.UploadFileAsync(request.VideoFile, "Videos", request.VideoAltText, true, cancellationToken);
            var deal = _context.Deals.Find(request.Id);
            if (deal == null) return null;

            deal.Video = videoInfo;

            await _context.SaveChangesAsync(cancellationToken);

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
        catch (Exception ex)
        {

            throw new Exception("Error updating video", ex);
        }


    }
}