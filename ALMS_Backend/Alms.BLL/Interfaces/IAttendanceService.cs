using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Interfaces
{
    public interface IAttendanceService
    {
        Task<VmAttendace> GetAllAttendacesAsync(VmAttendanceInput vmAttendanceInput);

        Task<VmAttendaceDetails> GetAttendanceByIdAsync(VmAttendaceDetailsInput vmAttendaceDetailsInput);

        bool AddAttendance(VmAddAttendanceInput vmAddAttendanceInput);
        bool RequestAttendance(VmAttendanceRequestModel vmAttendanceRequestModel);

        bool UpdateAttendance(VmAttendanceUpdateInput vmAttendanceUpdateInput);

        Task<VmGetRequestedAttendanceById> GetRequestedAttendanceByIdAsync(VmGetRequestedAttendanceByIdInput vmGetRequestedAttendanceByIdInput);

        Task<VmGetAttendanceByAttenndanceId> GetAttendanceByAttenndanceId(VmGetAttendanceByAttenndanceIdInput vmGetAttendanceByAttenndanceIdInput);

    }
}
