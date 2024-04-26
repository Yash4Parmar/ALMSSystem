using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.Service;
using Alms.BLL.SQlRepository;
using Alms.DAL.Helper;
using Alms.DAL.Models;
using Alms.DAL.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Data.DBHelper;

namespace Alms.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : BaseApiController<ProjectsController>
    {
        public ILogger<ProjectsController> Logger { get; }
        public IProjectService _projectService { get; }

        private readonly UserManager<ApplicationUser> _userManager;
        public IProcedureManager _procedureManager { get; }

        private readonly IMapper _mapper;

        public ProjectsController(ILogger<ProjectsController> logger, UserManager<ApplicationUser> userManager, IProjectService projectService, IProcedureManager procedureManager, IMapper mapper) : base(logger)
        {
            Logger = logger;
            _projectService = projectService;
            _procedureManager = procedureManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves a list of Projects based on the provided parameters using a stored procedure.
        /// </summary>
        /// </summary>
        /// <param name="getProjectsStoreProcedureInputModel">Input model containing parameters for filtering projects.</param>
        /// <returns> Returns a list of "VmGetProjects representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet]
        public async Task<ActionResult<List<VmGetProjects>>> GetAllProjects([FromQuery] GetProjectsInputModel getProjectsInputModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Employee") || User.IsInRole("Manager"))
            {
                getProjectsInputModel.EmployeeIds = user.EmployeeId.ToString();
            }


            var projects = await _projectService.GetAllProjectsAsync(getProjectsInputModel);

            var mappedProjects = _mapper.Map<VmGetProjects>(projects);
            if (mappedProjects.Projects.Count == 0)
            {
                mappedProjects.Projects = null;
                return Ok(new Response<VmGetProjects>(mappedProjects,true,MESSAGE.DATA_NOT_FOUND));
            }


            return Ok(new Response<VmGetProjects>(mappedProjects, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves details of an Project by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the Project to retrieve</param>
        /// <returns>Returns a "VmGetProjectDetails" representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("{Id}", Name = "GetProjectById")]
        public async Task<ActionResult<VmGetProjectDetails>> GetProjectByIdAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var project = await _projectService.GetprojectByIdAsync(Id);

            if (project.Id == 0 || (User.IsInRole("Employee") && !project.Employees.Any(e => e.EmployeeId == user.EmployeeId)) || (User.IsInRole("Manager") && project.ManagerId != user.EmployeeId))
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

          
            return Ok(new Response<VmGetProjectDetails>(project, true, MESSAGE.LOADED));

        }

        /// <summary>
        /// Updates an project with the specified ID.
        /// </summary>
        /// <param name="vmAddProject">The add project .</param>
        /// <returns>An ActionResult containing the add project if the add was successful,
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "AddProject")]
        public async Task<ActionResult> CreateProject([FromBody] VmAddProjectInt vmAddProjectInt)
        {
            VmAddProject vmAddProject = new VmAddProject()
            {
                ProjectName = vmAddProjectInt.ProjectName,
                StartDate = vmAddProjectInt.StartDate,
                EndDate = vmAddProjectInt.EndDate,
                Description = vmAddProjectInt.Description,
                ManagerId = vmAddProjectInt.ManagerId,
                UId = null,
                EmployeeIds = vmAddProjectInt.EmployeeIds == null ? "" : Helpers.ArrayToString(vmAddProjectInt.EmployeeIds),
            };

            // Attempt to add the project details
            bool isAdded = _projectService.AddProject(vmAddProject);
            if (!isAdded)
            {
                return NotFound(new Response(MESSAGE.NOT_ADDED, false));
            }

            return Ok(new Response(MESSAGE.SAVED, true));
        }

        /// <summary>
        /// Updates an project with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the project to update.</param>
        /// <param name="vmUpdateProject">The updated project details.</param>
        /// <returns>An ActionResult containing the updated project details if the update was successful,
        /// NotFound response with a message if the project is not found or the update failed.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpPut("{Id}", Name = "UpdateProject")]
        public async Task<ActionResult<VmGetProjectDetails>> UpdateProject(int Id, [FromBody] VmManageProjectInt vmMangeProjectInt)
        {
            VmManageProject vmMangeProject = new VmManageProject()
            {
                ProjectName = vmMangeProjectInt.ProjectName,
                StartDate = vmMangeProjectInt.StartDate,
                EndDate = vmMangeProjectInt.EndDate,
                Description = vmMangeProjectInt.Description,
                ManagerId = vmMangeProjectInt.ManagerId,
                UId = null,
                EmployeeIds = vmMangeProjectInt.EmployeeIds == null ? "" : Helpers.ArrayToString(vmMangeProjectInt.EmployeeIds),

            };

            VmUpdateProject vmUpdateProject = new VmUpdateProject();
            TExtentionMethods.CopyProperties(vmMangeProject, vmUpdateProject);
            vmUpdateProject.ProjectId = Id;

            // Retrieve the current project details
            var project = await _projectService.GetprojectByIdAsync(Id);
            if (project == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            // Attempt to update the project details
            bool isUpdated = _projectService.UpdateProject(vmUpdateProject);
            if (!isUpdated)
            {
                return NotFound(new Response(MESSAGE.NOT_UPDATED, false));
            }

            // Retrieve the updated project details
            var updatedProject = await _projectService.GetprojectByIdAsync(Id);
            var mappedProject = _mapper.Map<VmGetProjectDetails>(updatedProject);

            return Ok(new Response<VmGetProjectDetails>(mappedProject, true, MESSAGE.UPDATED));
        }

        /// <summary>
        /// Delete of an project by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the project to retrieve</param>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteProject(int Id)
        {
            var project = await _projectService.GetprojectByIdAsync(Id);
            if (project.Id == 0)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }
            bool isDeleted = _projectService.DeleteProject(Id);
            if (!isDeleted)
            {
                return NotFound(new Response(MESSAGE.NOT_DELETED, false));
            }

            return Ok(new Response(MESSAGE.DELETED, true));
        }

        /// <summary>
        /// Retrieves all projects and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of projects.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("list")]
        public async Task<ActionResult<List<HelperModel>>> GetAllProjectsHelper()
        {
            var projects = await _projectService.GetAllProjectsHelper();
            return Ok(new Response<List<HelperModel>>(projects, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves all manager not in project and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of manager not in project.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("UnassignManager/list")]
        public async Task<ActionResult<List<HelperModel>>> GetUnassignedManagersHelper([FromQuery] int ProjectId)
        {
            var unassignManager = await _projectService.GetUnassignedManagersHelper(ProjectId);
            return Ok(new Response<List<HelperModel>>(unassignManager, true, MESSAGE.LOADED));
        }
    }
}