using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
namespace DealClean.Domain.Entities;

public class HotelMediaInfo
{
    public string? Path { get; set; }
    public string? AltText { get; set; }
    public bool IsVideo { get; set; }
}