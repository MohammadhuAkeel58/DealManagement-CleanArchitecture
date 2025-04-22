using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using MediatR;

namespace DealClean.Application.Deals.Commands.UpdateImage;

public class UpdateImageCommand : IRequest<DealsVm>
{
    public ImageDto ImageDt { get; set; }
}

public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, DealsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UpdateImageCommandHandler(IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<DealsVm> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
    {
        var imageInfo = await _fileUploadService.UploadFileAsync(request.ImageDt.ImageFile, "Images", null, false, cancellationToken);
        var deal = await _context.Deals.FindAsync(request.ImageDt.Id);
        if (deal == null) return null;

        deal.Image = imageInfo.Path;

        return new DealsVm
        {
            Id = deal.Id,
            Name = deal.Name,
            Slug = deal.Slug,
            Title = deal.Title,
            Image = deal.Image,


        };




    }
}