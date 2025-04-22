using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DealClean.Infrastructure.Services;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public FileUploadService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<VideoInfo?> UploadFileAsync(IFormFile file, string folderPath, string? altText, bool isVideo, CancellationToken cancellationToken)
    {

        if (file == null || file.Length == 0)
            return null;
        folderPath = folderPath.TrimStart('/');

        var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov" };
        var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

        var extension = Path.GetExtension(file.FileName).ToLower();

        if (isVideo)
        {
            if (!allowedVideoExtensions.Contains(extension))
                throw new ArgumentException("Invalid video format");
            if (file.Length > 200 * 1024 * 1024)
                throw new ArgumentException("Video file size exceeds 200MB");
        }
        else
        {
            if (!allowedImageExtensions.Contains(extension))
                throw new ArgumentException("Invalid image format");
            if (file.Length > 5 * 1024 * 1024)
                throw new ArgumentException("Image file size exceeds 5MB");
        }
        try
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            Directory.CreateDirectory(uploadsFolder);

            var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string uniqueFileName = $"{originalFileName}-{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream, cancellationToken);
            }

            var fileUrl = string.IsNullOrEmpty(folderPath) ? $"/{uniqueFileName}" : $"/{folderPath}/{uniqueFileName}";
            var resolvedAltText = isVideo ? altText : null;

            return new VideoInfo
            {
                Path = fileUrl,
                AltText = resolvedAltText
            };

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
            throw;
        }

    }


}