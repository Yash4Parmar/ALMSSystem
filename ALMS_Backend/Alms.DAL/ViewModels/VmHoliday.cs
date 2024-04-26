using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.DAL.ViewModels
{
    public class VmHoliday
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

        public string? StartDate { get; set; }

        public string? EndDate { get; set; }
    }

    public class VmAddHoliday
    {
        public string? Name { get; set; }

        public string StartDate { get; set; } = String.Empty;

        public string EndDate { get; set; } = String.Empty;
    }
}
