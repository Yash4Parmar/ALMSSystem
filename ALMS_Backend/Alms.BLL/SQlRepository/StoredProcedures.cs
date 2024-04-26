using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.SQlRepository
{
    public static class StoredProcedures
    {
        public const string GetEmployees = "[usp_GetEmployees]";
        public const string GetEmployeeById = "[usp_GetEmployeeById]";
        public const string GetEmployeeByManagerId = "[usp_GetEmployeeByManagerId]";
        public const string DeleteEmployee = "[usp_DeleteEmployee]";
        public const string GetLeaves = "[usp_GetLeaves]";
        public const string GetLeaveById = "[usp_GetLeaveById]";
        public const string UpdateLeave = "[usp_UpdateLeave]";
        public const string AddLeave = "[usp_AddLeave]";
        public const string FilteredData = "[usp_GetFilterData]";
        public const string GetProjects = "[usp_GetProjects]";
        public const string GetProjectById = "[usp_GetProjectById]";
        public const string DeleteProject = "[usp_DeleteProject]";

        //below manage project :- add project Or update project
        public const string ManageProject = "[usp_ManageProject]";


        public const string UpdateEmployee = "[usp_UpdateEmployee]";


        public const string GetAttendanceById = "[usp_GetAttendanceById]";
        public const string AddAttendance = "[usp_AddAttendance]";
        public const string GetAttendances = "[usp_GetAttendances]";
        public const string RequestAttendance = "[usp_RequestAttendance]";
        public const string UpdateAttendance = "[usp_UpdateAttendance]";
        public const string GetRequestedAttendanceById = "[usp_GetRequestedAttendanceById]";
        public const string GetAttendanceByAttenndanceId = "[usp_GetAttendanceByAttenndanceId]";
    }

}