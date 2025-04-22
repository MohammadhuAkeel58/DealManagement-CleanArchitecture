using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Common.Interfaces;

public interface IFileUploadService
{
    Task<VideoInfo?> UploadFileAsync(IFormFile file, string folderPath, string? altText, bool isVideo, CancellationToken cancellationToken);

}