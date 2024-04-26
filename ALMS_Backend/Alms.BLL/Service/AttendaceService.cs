using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.SQlRepository;
using Alms.DAL.ViewModels;
using Service.Data.DBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Service
{
    public class AttendaceService : IAttendanceService
    {
        private readonly IProcedureManager _procedureManager;

        public AttendaceService(IProcedureManager procedureManager)
        {
            _procedureManager = procedureManager;
        }
        public Task<VmAttendace> GetAllAttendacesAsync(VmAttendanceInput vmAttendanceInput)
        {
            var attendances = _procedureManager.ExecStoreProcedureMulResults<StoredProcedureCommonModel, VmAttendaceModel>(StoredProcedures.GetAttendances, vmAttendanceInput);

            var countData = attendances.Item1[0].Count;
            var attendancesData = attendances.Item2;

            VmAttendace vmAttendace = new VmAttendace();

            if (attendancesData != null)
            {
                vmAttendace.Count = (int)countData;
                vmAttendace.Attendaces = attendancesData;
            }


            return Task.FromResult(vmAttendace);
        }
        public Task<VmAttendaceDetails> GetAttendanceByIdAsync(VmAttendaceDetailsInput vmAttendaceDetailsInput)
        {
            var attendances = _procedureManager.ExecStoreProcedure<VmAttendaceDetailsResponse>(StoredProcedures.GetAttendanceById, vmAttendaceDetailsInput);

            var singleAttendance = attendances.First(a => a.EmployeeId == vmAttendaceDetailsInput.EmployeeId);

            var attendaceDetails = new VmAttendaceDetails();

            // Copy properties from the singleEmployee to the employeeDetails using an extension method
            TExtentionMethods.CopyProperties(singleAttendance, attendaceDetails);

            // Initialize the Times collection
            attendaceDetails.Times = new List<TimeSpan>();

            foreach (var attendance in attendances)
            {
                attendaceDetails.Times.Add(attendance.Time);
            }

            // Calculate total time
            TimeSpan totalTime = CalculateTotalTime(attendaceDetails.Times);
            attendaceDetails.WorkingHours = totalTime;

            return Task.FromResult(attendaceDetails);

        }

        public bool AddAttendance(VmAddAttendanceInput vmAddAttendanceInput)
        {
            bool isAdded = _procedureManager.ExecStoreProcedure(StoredProcedures.AddAttendance, vmAddAttendanceInput);
            return isAdded;
        }

        private TimeSpan CalculateTotalTime(List<TimeSpan> punchTimes)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            for (int i = 0; i < punchTimes.Count - 1; i += 2)
            {
                TimeSpan punchIn = punchTimes[i];
                TimeSpan punchOut = punchTimes[i + 1];

                TimeSpan difference = punchOut - punchIn;
                totalTime += difference;
            }

            return totalTime;
        }


        public bool RequestAttendance(VmAttendanceRequestModel vmAttendanceRequestModel)
        {
            bool isRequested = _procedureManager.ExecStoreProcedure(StoredProcedures.RequestAttendance, vmAttendanceRequestModel);
            return isRequested;
        }

        public bool UpdateAttendance(VmAttendanceUpdateInput vmAttendanceUpdateInput)
        {
            bool isRequested = _procedureManager.ExecStoreProcedure(StoredProcedures.UpdateAttendance, vmAttendanceUpdateInput);
            return isRequested;
        }

        public Task<VmGetRequestedAttendanceById> GetRequestedAttendanceByIdAsync(VmGetRequestedAttendanceByIdInput vmGetRequestedAttendanceByIdInput)
        {
            var attendances = _procedureManager.ExecStoreProcedure<VmGetRequestedAttendanceByIdResponse>(StoredProcedures.GetRequestedAttendanceById, vmGetRequestedAttendanceByIdInput);

            var singleAttendance = attendances.First(a => a.Id == vmGetRequestedAttendanceByIdInput.AttendanceId);

            var attendaceDetails = new VmGetRequestedAttendanceById();

            // Copy properties from the singleEmployee to the employeeDetails using an extension method
            TExtentionMethods.CopyProperties(singleAttendance, attendaceDetails);

            // Initialize the Times collection
            attendaceDetails.Times = new List<TimeSpan>();

            foreach (var attendance in attendances)
            {
                attendaceDetails.Times.Add(attendance.Time);
            }

            // Calculate total time
            TimeSpan totalTime = CalculateTotalTime(attendaceDetails.Times);
            attendaceDetails.WorkingHours = totalTime;

            return Task.FromResult(attendaceDetails);

        }

        public Task<VmGetAttendanceByAttenndanceId> GetAttendanceByAttenndanceId(VmGetAttendanceByAttenndanceIdInput vmGetAttendanceByAttenndanceIdInput)
        {
            var attendances = _procedureManager.ExecStoreProcedure<VmGetAttendanceByAttenndanceIdResponse>(StoredProcedures.GetAttendanceByAttenndanceId, vmGetAttendanceByAttenndanceIdInput);

            var singleAttendance = attendances.First(a => a.Id == vmGetAttendanceByAttenndanceIdInput.AttendanceId);

            var attendaceDetails = new VmGetAttendanceByAttenndanceId();

            // Copy properties from the singleEmployee to the employeeDetails using an extension method
            TExtentionMethods.CopyProperties(singleAttendance, attendaceDetails);

            // Initialize the Times collection
            attendaceDetails.Times = new List<TimeSpan>();

            foreach (var attendance in attendances)
            {
                attendaceDetails.Times.Add(attendance.Time);
            }

            // Calculate total time
            TimeSpan totalTime = CalculateTotalTime(attendaceDetails.Times);
            attendaceDetails.WorkingHours = totalTime;

            return Task.FromResult(attendaceDetails);
        }
    }
}
