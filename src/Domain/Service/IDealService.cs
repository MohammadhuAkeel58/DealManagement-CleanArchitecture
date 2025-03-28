using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DealClean.Domain.Entities;

namespace DealClean.Domain.Service;

public interface IDealService
{
    Task<List<Deal>> GetAllDealsAsync();
    Task<Deal> GetDealByIdAsync(int id);
    Task<Deal> CreateDealAsync(Deal deal);
    Task<Deal> UpdateDealAsync(int id);
    Task<Deal> UpdateImageAsync(int id, Deal deal);
    Task<Deal> UpdateVideoAsync(int id, Deal deal);
    Task<int> DeleteDealAsync(int id);
}