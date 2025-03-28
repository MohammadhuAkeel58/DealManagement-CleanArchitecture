using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Application.Common.Mappings;
using DealClean.Domain.Entities;

namespace DealClean.Application.Deals.Queries.GetDeals;

public class DealsVm : IMapFrom<Deal>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }
    public string? Video { get; set; }    // From VideoInfo.Path
    public string? VideoAltText { get; set; }
}