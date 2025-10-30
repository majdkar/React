using SchoolV01.Shared.ViewModels.Statistics;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public interface IStatisticsService
    {
      StatisticsViewModel Get();
    }
}
