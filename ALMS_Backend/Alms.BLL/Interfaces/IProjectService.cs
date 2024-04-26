using Alms.DAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Interfaces
{
    public interface IProjectService
    {
        Task<VmGetProjects> GetAllProjectsAsync(GetProjectsInputModel getProjectsInputModel);

        Task<VmGetProjectDetails> GetprojectByIdAsync(int Id);

        bool AddProject(VmAddProject vmAddProject);
        bool UpdateProject(VmUpdateProject vmUpdateProject);
        bool DeleteProject(int Id);

        Task<List<HelperModel>> GetAllProjectsHelper();
        Task<List<HelperModel>> GetUnassignedManagersHelper(int ProjectId);

    }
}