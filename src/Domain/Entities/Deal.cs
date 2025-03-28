using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DealClean.Domain.Entities;

public class Deal
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Title { get; set; }
    public string? Image { get; set; }

    public VideoInfo? Video { get; set; }


}