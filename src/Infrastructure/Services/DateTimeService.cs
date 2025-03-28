using DealClean.Application.Common.Interfaces;

namespace DealClean.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
