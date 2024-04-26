using Alms.DAL.Models;
using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Interfaces
{
    public interface IHolidayService
    {
        Task<Holiday> GetByIdAsync(int id);
        Task<IEnumerable<Holiday>> GetAllAsync();
        Task<Holiday> AddAsync(Holiday holiday);
        Task<Holiday> Remove(Holiday holiday);
    }
}
