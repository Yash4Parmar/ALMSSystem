using Alms.BLL.Interfaces;
using Alms.BLL.SQlRepository;
using Alms.DAL.Models;
using Alms.DAL.Repository;
using Service.Data.DBHelper;
using Alms.DAL.ViewModels;
using Alms.BLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IDBRepository<AspNetUser> _employeeRepo;
        public IProcedureManager _procedureManager { get; }

        public EmployeeService(IDBRepository<AspNetUser> employeeRepo, IProcedureManager procedureManager)
        {
            _employeeRepo = employeeRepo;
            _procedureManager = procedureManager;
        }

        /// <summary>
        ///  Retrieves a paginated list of employees asynchronously using a stored procedure.
        /// </summary>
        /// <param name="GetEmployeesInputModel">Input parameters for the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation, containing the paginated list of employees.</returns>
        public Task<VmGetEmployees> GetAllEmployeesAsync(GetEmployeesInputModel getEmployeesInputModel)
        {
            var spParameters = new GetEmployeesInputModel
            {

                EmployeeIds = getEmployeesInputModel.EmployeeIds ?? "",
                ProjectIds = getEmployeesInputModel.ProjectIds ?? "",
                ManagerIds = getEmployeesInputModel.ManagerIds ?? "",
                Page = getEmployeesInputModel.Page,
                PageSize = getEmployeesInputModel.PageSize,
                Field = getEmployeesInputModel.Field,
                Sort = getEmployeesInputModel.Sort

            };

            var employees = _procedureManager.ExecStoreProcedureMulResults<StoredProcedureCommonModel, VmSPGetEmployees>(StoredProcedures.GetEmployees, spParameters);

            var countData = employees.Item1[0].Count;
            var employeesData = employees.Item2;


            VmGetEmployees getEmployees = new VmGetEmployees
            {
                Count = (int)countData,
                Employees = employeesData
            };
            return Task.FromResult(getEmployees);

        }

        /// <summary>
        /// Retrieves detailed information about an employee asynchronously based on the provided employee ID.
        /// sets properties with VmGetEMployeeDetails
        /// </summary>
        /// <param name="Id">The unique identifier of the employee</param>
        /// <returns>A task representing the asynchronous operation, containing detailed information about the employee,
        /// including their basic details and associated projects.</returns>
        public Task<VmGetEmployeeDetails> GetEmployeeByIdAsync(int Id)
        {
            //Create parameter for stored procedure
            var employeeId = new DBSQLParameter("Id", Id);
            List<DBSQLParameter> dBSQLParameters = [employeeId];

            // Execute the stored procedure to get a list of employee details
            var employee = _procedureManager.ExecStoreProcedure<VmSPGetEmployeeDetails>(StoredProcedures.GetEmployeeById, dBSQLParameters);

            //Find the expected employee
            var singleEmployee = employee.First(e => e.Id == Id);

            var employeeDetails = new VmGetEmployeeDetails();

            // Copy properties from the singleEmployee to the employeeDetails using an extension method
            TExtentionMethods.CopyProperties(singleEmployee, employeeDetails);


            // Process each employee in the result set and add to Project and Employees List
            foreach (var emp in employee)
            {
                if (emp.Project != null || emp.ProjectId != null)
                {
                    var projectObj = new ProjectObj
                    {
                        Project = emp.Project,
                        ProjectId = emp.ProjectId
                    };

                    bool isProjectExistsInList = employeeDetails.Projects.Exists(p => p.ProjectId == projectObj.ProjectId);

                    // If the project is not in the list, add it
                    if (!isProjectExistsInList)
                    {
                        employeeDetails.Projects.Add(projectObj);
                    }
                }

                if (emp.Employee != null || emp.EmployeeId != null)
                {
                    var employeeObj = new EmployeeObj
                    {
                        Employee = emp.Employee,
                        EmployeeId = emp.EmployeeId
                    };

                    bool isEmployeeExistsInList = employeeDetails.Employees.Exists(p => p.EmployeeId == employeeObj.EmployeeId);

                    // If the employee is not in the list, add it
                    if (!isEmployeeExistsInList)
                    {
                        employeeDetails.Employees.Add(employeeObj);
                    }
                }
            }

            return Task.FromResult(employeeDetails);
        }

        /// <summary>
        /// Updates an employee with the provided details.
        /// </summary>
        /// <param name="vmUpdateEmployee">The ViewModel containing the updated employee details.</param>
        /// <returns>True if the employee was successfully updated, otherwise false.</returns>
        public bool UpdateEmployee(VmUpdateEmployee vmUpdateEmployee)
        {
            bool isUpdate = _procedureManager.ExecStoreProcedure(StoredProcedures.UpdateEmployee, vmUpdateEmployee);
            return isUpdate;
        }

        /// <summary>
        /// Deletes an employee with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the employee to delete.</param>
        /// <returns>True if the employee was successfully deleted, otherwise false.</returns>
        public bool DeleteEmployee(int Id)
        {
            var employeeId = new DBSQLParameter("Id", Id);
            List<DBSQLParameter> dBSQLParameters = [employeeId];
            bool isDeleted = _procedureManager.ExecStoreProcedure(StoredProcedures.DeleteEmployee, dBSQLParameters);
            return isDeleted;
        }

        public Task<List<HelperModel>> GetAllEmployeesHelper(int managerId)
        {
            var tableName = new DBSQLParameter("Type", Constants.AllEmployees);
            var manager = new DBSQLParameter("ManagerId", managerId);
            if (managerId != 0)
            {
                tableName = new DBSQLParameter("Type", Constants.ManagerEmployees);
            }

            List<DBSQLParameter> dBSQLParameters = [tableName,manager];
            var employees = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(employees);
        }

        public Task<List<HelperModel>> GetOnlyEmployeesHelper()
        {
            var tableName = new DBSQLParameter("Type", Constants.OnlyEmployees);
            List<DBSQLParameter> dBSQLParameters = [tableName];

            var employees = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(employees);
        }

        public Task<List<HelperModel>> GetAllManagersHelper()
        {
            var tableName = new DBSQLParameter("Type", Constants.AllManagers);
            List<DBSQLParameter> dBSQLParameters = [tableName];

            var managers = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(managers);
        }

        public Task<List<VmEmployee>> GetEmployeesByManagerHelper(int id,bool IsManagerIdReq)
        {
            var managerId = new DBSQLParameter("Id",id);
            var managerIdReq = new DBSQLParameter("IsManagerIdReq", IsManagerIdReq);
            List<DBSQLParameter> dBSQLParameters = [managerId, managerIdReq];

            var employees = _procedureManager.ExecStoreProcedure<VmEmployee>(StoredProcedures.GetEmployeeByManagerId, dBSQLParameters);

            return Task.FromResult(employees);
        }
    }
}
