using Alms.DAL.Models;
using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Interfaces
{
    public interface ILeaveService
    {
        Task<VmGetLeaves> GetAllLeavesAsync(GetLeavesInputModel getLeavesInputModel);
        Task<VmGetLeaveDetails> GetLeaveByIdAsync(int Id);

        Task<VmManageLeave> UpdateLeave(VmLeave vmLeave);
        Task<VmManageLeave> AddLeave(VmAddLeave vmAddLeave);
        Task<Leave> GetByIdAsync(int Id);

        Task<Leave> Remove(Leave leave);

        Task<List<HelperModel>> GetAllLeaveTypes();
        Task<List<HelperModel>> GetAllStatus();
    }
}
