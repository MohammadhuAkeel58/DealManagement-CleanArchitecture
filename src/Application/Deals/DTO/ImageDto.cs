using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Deals.DTO;

public class ImageDto
{
    public int Id { get; set; }
    public string? Image { get; set; } // for to store the path
    public IFormFile? ImageFile { get; set; } // for file upload
}