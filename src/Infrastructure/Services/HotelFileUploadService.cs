using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DealClean.Application.Common.Interfaces;
using DealClean.Application.Deals.DTO;
using DealClean.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace DealClean.Infrastructure.Services;

public class HotelFileUploadService : IHotelFileUploadService
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public HotelFileUploadService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<List<HotelMediaInfo>> UploadHotelMediaAsync(
         List<IFormFile> imageFiles,
         List<string> imageAltTexts,
         List<IFormFile> videoFiles,
         List<string> videoAltTexts,
         CancellationToken cancellationToken)
    {
        var result = new List<HotelMediaInfo>();


        if (imageFiles != null && imageFiles.Any())
        {
            for (int i = 0; i < imageFiles.Count; i++)
            {
                var altText = i < imageAltTexts.Count ? imageAltTexts[i] : null;
                var mediaItem = await UploadSingleFileAsync(imageFiles[i], "HotelImages", altText, false, cancellationToken);
                if (mediaItem != null)
                {
                    result.Add(mediaItem);
                }
            }
        }


        if (videoFiles != null && videoFiles.Any())
        {
            for (int i = 0; i < videoFiles.Count; i++)
            {
                var altText = i < videoAltTexts.Count ? videoAltTexts[i] : null;
                var mediaItem = await UploadSingleFileAsync(videoFiles[i], "HotelVideos", altText, true, cancellationToken);
                if (mediaItem != null)
                {
                    result.Add(mediaItem);
                }
            }
        }

        return result;
    }

    private async Task<HotelMediaInfo> UploadSingleFileAsync(
        IFormFile file,
        string folderPath,
        string? altText,
        bool isVideo,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return null;

        folderPath = folderPath.TrimStart('/');

        var allowedVideoExtensions = new[] { ".mp4", ".avi", ".mov" };
        var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

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

            return new HotelMediaInfo
            {
                Path = fileUrl,
                AltText = altText,
                IsVideo = isVideo
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file: {ex.Message}");
            throw;
        }
    }
}