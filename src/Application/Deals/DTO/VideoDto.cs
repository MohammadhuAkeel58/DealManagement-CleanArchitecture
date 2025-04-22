using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Deals.DTO
{
    public class VideoDto
    {
        public int Id { get; set; }
        public IFormFile? VideoFile { get; set; }
        public string? AltText { get; set; }
    }
}