using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace DealClean.Application.Deals.DTO;

public class HotelDto
{
    public int HotelId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int DealId { get; set; }
    public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
    public List<string> ImageAltTexts { get; set; } = new List<string>();
    public List<IFormFile> VideoFiles { get; set; } = new List<IFormFile>();
    public List<string> VideoAltTexts { get; set; } = new List<string>();
}

