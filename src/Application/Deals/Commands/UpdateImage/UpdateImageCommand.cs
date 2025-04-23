using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Application.Deals.Queries.GetDeals;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Deals.Commands.UpdateImage;

public class UpdateImageCommand : IRequest<DealsVm>
{
    public int Id { get; set; }
    public string? Image { get; set; } // for to store the path
    public IFormFile? ImageFile { get; set; } // for file upload
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

        try
        {
            var deal = await _context.Deals.FindAsync(request.Id);
            if (deal == null) return null;
            var imageInfo = await _fileUploadService.UploadFileAsync(request.ImageFile, "Images", null, false, cancellationToken);

            deal.Image = imageInfo.Path;

            await _context.SaveChangesAsync(cancellationToken);

            return new DealsVm
            {
                Id = deal.Id,
                Name = deal.Name,
                Slug = deal.Slug,
                Title = deal.Title,
                Image = deal.Image,


            };
        }
        catch (Exception ex)
        {

            throw new Exception("Error updating image", ex);
        }


    }
}