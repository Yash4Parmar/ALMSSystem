using Alms.DAL.Models;
using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alms.DAL.ViewModels;

namespace Alms.BLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<VmGetEmployees> GetAllEmployeesAsync(GetEmployeesInputModel getEmployeesInputModel);

        Task<VmGetEmployeeDetails> GetEmployeeByIdAsync(int Id);

        bool UpdateEmployee(VmUpdateEmployee vmUpdateEmployee);

        bool DeleteEmployee(int Id);

        Task<List<HelperModel>> GetAllEmployeesHelper(int managerId);
        Task<List<VmEmployee>> GetEmployeesByManagerHelper(int id,bool IsManagerIdReq);
        Task<List<HelperModel>> GetOnlyEmployeesHelper();
        Task<List<HelperModel>> GetAllManagersHelper();

    }
}
