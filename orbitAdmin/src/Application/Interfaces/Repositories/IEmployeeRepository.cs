using System;
using System.Threading.Tasks;

namespace SchoolV01.Application.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<bool> IsNationUsed(int nationId);
    }
}