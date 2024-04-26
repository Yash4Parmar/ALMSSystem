using Alms.BLL.Interfaces;
using Alms.DAL.Models;
using Alms.DAL.Repository;
using Alms.DAL.ViewModels;
using Microsoft.AspNetCore.Http.Timeouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Alms.BLL.Service
{
    public class HolidayService : IHolidayService
    {
        public IDBRepository<Holiday> _holidayRepo { get; }

        public HolidayService(IDBRepository<Holiday> holidayRepo)
        {
            _holidayRepo = holidayRepo;
        }


        public async Task<Holiday> AddAsync(Holiday holiday)
        {
            return await _holidayRepo.AddAsync(holiday);
        }

        public async Task<IEnumerable<Holiday>> GetAllAsync()
        {
            var allHolidays = await _holidayRepo.GetAllAsync();
            var currentYearHolidays = allHolidays.Where(holiday => holiday.StartDate.Year == DateTime.Now.Year && holiday.IsDeleted != true);
            return currentYearHolidays;
        }

        public async Task<Holiday> GetByIdAsync(int Id)
        {
            return await _holidayRepo.GetByIdAsync(Id);
        }

        public Task<Holiday> Remove(Holiday holiday)
        {
            return _holidayRepo.Remove(holiday);
        }
    }
}
