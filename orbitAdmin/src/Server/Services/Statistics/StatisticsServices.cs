using SchoolV01.Core.Entities;
using SchoolV01.Shared.ViewModels.Statistics;
using Microsoft.EntityFrameworkCore;
using SchoolV01.Application.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolV01.Application.Services
{
    public class StatisticsServices : IStatisticsService
    {
        private readonly IUnitOfWork<int> uow;

        public StatisticsServices(IUnitOfWork<int> uow)
        {
            this.uow = uow;
        }
        public  StatisticsViewModel Get()
        {
            var result = new StatisticsViewModel
            {
                Blocks = uow.Query<Block>().ToList() != null ? uow.Query<Block>().Count() : 0,
                Events = uow.Query<Event>().ToList() != null ? uow.Query<Event>().Count() : 0,
                Menus = uow.Query< Menu>().ToList() != null ? uow.Query<Menu>().Count() : 0,
                Pages = uow.Query<Page>().ToList() != null ? uow.Query<Page>().Count() : 0
            };

            return result;
        }

    }
}
