using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.SQlRepository;
using Alms.DAL.Repository;
using Alms.DAL.ViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Service.Data.DBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Service
{
    public class ProjectService : IProjectService
    {

        public IProcedureManager _procedureManager { get; }

        public ProjectService(IProcedureManager procedureManager)
        {
            _procedureManager = procedureManager;
        }

        /// <summary>
        ///  Retrieves a paginated list of Projects asynchronously using a stored procedure.
        /// </summary>
        /// <param name="projectsStoreProcedureInputModel">Input parameters for the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation, containing the paginated list of Projects.</returns>
        public Task<VmGetProjects> GetAllProjectsAsync(GetProjectsInputModel getProjectsInputModel)
        {
            var spParameters = new GetEmployeesInputModel
            {
                EmployeeIds = getProjectsInputModel.EmployeeIds == null ? "" : getProjectsInputModel.EmployeeIds,
                ProjectIds = getProjectsInputModel.ProjectIds == null ? "" : getProjectsInputModel.ProjectIds,
                Page = getProjectsInputModel.Page,
                PageSize = getProjectsInputModel.PageSize,
                Field = getProjectsInputModel.Field,
                Sort = getProjectsInputModel.Sort
            };

            var projects = _procedureManager.ExecStoreProcedureMulResults<StoredProcedureCommonModel, VmSPGetProjects>(StoredProcedures.GetProjects, spParameters);

            var countData = projects.Item1[0].Count;
            var projectsData = projects.Item2;

            List<VmProjects> vmProjects = new List<VmProjects>();
            if (projectsData != null)
            {
                foreach (var project in projectsData)
                {

                    vmProjects.Add(new VmProjects
                    {
                        Id = project.Id,
                        ProjectId = project.ProjectId,
                        EndDate = project.EndDate,
                        StartDate = project.StartDate,
                        Name = project.Name,
                        Manager = project.Manager,
                        Employees = project.Employees == null ? [] : project.Employees.Split(new string[] { ", " }, StringSplitOptions.None)
                    });
                }
            }
            VmGetProjects getProjects = new VmGetProjects
            {
                Count = (int)countData,
                Projects = vmProjects
            };

            return Task.FromResult(getProjects);

        }

        /// <summary>
        /// Retrieves detailed information about an project asynchronously based on the provided project ID.
        /// sets properties with VmGetProjectDetails
        /// </summary>
        /// <param name="Id">The unique identifier of the project</param>
        /// <returns>A task representing the asynchronous operation, containing detailed information about the project,
        /// including their basic details and associated projects.</returns>

        public Task<VmGetProjectDetails> GetprojectByIdAsync(int Id)
        {
            //Create parameter for stored procedure
            var projectId = new DBSQLParameter("Id", Id);
            List<DBSQLParameter> dBSQLParameters = [projectId];

            // Execute the stored procedure to get a list of employee details
            var project = _procedureManager.ExecStoreProcedure<VmSPGetProjectDetails>(StoredProcedures.GetProjectById, dBSQLParameters);


            var projectDetails = new VmGetProjectDetails();

            //Find the expected Manager for this project
            if (project.Count != 0)
            {
              
                var singleProject = project.FirstOrDefault(e => e.ManagerId != null);

            if (singleProject == null)
            {
                singleProject = project.First(e => e.Id == Id);
            }



            // Copy properties from the singleEmployee to the employeeDetails using an extension method
            TExtentionMethods.CopyProperties(singleProject, projectDetails);


            // Process each employee in the result set and add to Project and Employees List
            
                foreach (var emp in project)
                {

                    if (emp.Employee != null || emp.EmployeeId != null)
                    {
                        var employeeObj = new ProjectEmployeeObj
                        {
                            Employee = emp.Employee,
                            EmployeeId = emp.EmployeeId
                        };

                        bool isEmployeeExistsInList = projectDetails.Employees.Exists(p => p.EmployeeId == employeeObj.EmployeeId);

                        // If the employee is not in the list, add it
                        if (!isEmployeeExistsInList)
                        {
                            projectDetails.Employees.Add(employeeObj);
                        }
                    }
                }
              
            }

            return Task.FromResult(projectDetails);
        }


        public bool AddProject(VmAddProject vmAddProject)
        {
            bool isUpdate = _procedureManager.ExecStoreProcedure(StoredProcedures.ManageProject, vmAddProject);
            return isUpdate;
        }

        /// <summary>
        /// Updates an project with the provided details.
        /// </summary>
        /// <param name="vmUpdateProject">The ViewModel containing the updated project details.</param>
        /// <returns>True if the project was successfully updated, otherwise false.</returns>
        public bool UpdateProject(VmUpdateProject vmUpdateProject)
        {
            bool isUpdate = _procedureManager.ExecStoreProcedure(StoredProcedures.ManageProject, vmUpdateProject);
            return isUpdate;
        }

        /// <summary>
        /// Deletes an project with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the project to delete.</param>
        /// <returns>True if the project was successfully deleted, otherwise false.</returns>
        public bool DeleteProject(int Id)   
        {
            var projectId = new DBSQLParameter("Id", Id);
            List<DBSQLParameter> dBSQLParameters = [projectId];

            bool isDeleted = _procedureManager.ExecStoreProcedure(StoredProcedures.DeleteProject, dBSQLParameters);
            return isDeleted;
        }

        public Task<List<HelperModel>> GetAllProjectsHelper()
        {
            var tableName = new DBSQLParameter("Type", Constants.AllProjects);
            List<DBSQLParameter> dBSQLParameters = [tableName];

            var projects = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(projects);
        }

        public Task<List<HelperModel>> GetUnassignedManagersHelper(int ProjectId)
        {
            var tableName = new DBSQLParameter("Type", Constants.ManagerNotInProject);
            var projectId = new DBSQLParameter("ProjectId", ProjectId);
            List<DBSQLParameter> dBSQLParameters = [tableName, projectId];

            var ManagerNotInProject = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(ManagerNotInProject);
        }

    }
}