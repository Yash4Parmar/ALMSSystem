using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmManagerProjectCommon
    {
        public string? ProjectName { get; set; }

        public string? Description { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }

        public int? ManagerId { get; set; }

        public int? UId { get; set; }
    }

    public class VmManageProject : VmManagerProjectCommon
    {

        public string? EmployeeIds { get; set; }

    }

    public class VmManageProjectInt : VmManagerProjectCommon
    {

        public int[]? EmployeeIds { get; set; }

    }

    public class VmUpdateProject : VmManageProject
    {
        public int? ProjectId { get; set; }
       
    }

    public class VmAddProject : VmManageProject
    {

    }

    public class VmAddProjectInt : VmManageProjectInt
    {

    }
}