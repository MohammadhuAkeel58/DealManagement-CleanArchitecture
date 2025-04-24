using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Deals.DTO;
using DealClean.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Common.Interfaces;

public interface IHotelFileUploadService
{
    Task<List<HotelMediaInfo>> UploadHotelMediaAsync(
       List<IFormFile> imageFiles,
       List<string> imageAltTexts,
       List<IFormFile> videoFiles,
       List<string> videoAltTexts,
       CancellationToken cancellationToken);
}