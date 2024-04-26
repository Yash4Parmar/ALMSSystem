using Alms.BLL.Interfaces;
using Alms.BLL.Service;
using Alms.DAL.Repository;
using Service.Data.DBHelper;

namespace Alms.API.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDBRepository<>), typeof(DBRepository<>)); // Register DBRepository<TEntity>
        }

        public static void AddRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ILeaveService,LeaveService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProcedureManager, ProcedureManager>();
            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IAttendanceService, AttendaceService>();
        }
    }
}