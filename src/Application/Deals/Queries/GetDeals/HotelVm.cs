using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealClean.Application.Deals.Queries.GetDeals;

public class HotelVm
{
    public int HotelId { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public List<HotelMediaVm> Media { get; set; } = new List<HotelMediaVm>();
}