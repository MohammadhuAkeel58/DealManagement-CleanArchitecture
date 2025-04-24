using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealClean.Domain.Entities;

public class Hotel
{
    public int HotelId { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Description { get; set; }
    public int DealId { get; set; }
    public Deal? Deal { get; set; }
    public List<HotelMediaInfo> Media { get; set; } = new List<HotelMediaInfo>();

}