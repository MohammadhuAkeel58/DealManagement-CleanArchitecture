using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DealClean.Application.Common.Interfaces;

public interface IApplicationDbContext

{
    DbSet<Deal> Deals { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}