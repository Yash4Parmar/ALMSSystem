using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.SQlRepository;
using Alms.DAL.Helper;
using Alms.DAL.Models;
using Alms.DAL.TempModels;
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
    public class EmployeesController : BaseApiController<EmployeesController>
    {
        public ILogger<EmployeesController> Logger { get; }
        public IEmployeeService _employeeService { get; }

        private readonly UserManager<ApplicationUser> _userManager;
        public IProcedureManager _procedureManager { get; }

        private readonly IMapper _mapper;

        public EmployeesController(ILogger<EmployeesController> logger, UserManager<ApplicationUser> userManager, IEmployeeService employeeService, IProcedureManager procedureManager, IMapper mapper) : base(logger)
        {
            Logger = logger;
            _employeeService = employeeService;
            _procedureManager = procedureManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Retrieves a list of employees based on the provided parameters using a stored procedure.
        /// </summary>
        /// </summary>
        /// <param name="getEmployeesStoreProcedureInputModel">Input model containing parameters for filtering employees.</param>
        /// <returns> Returns a list of "VmGetEmployees representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet]
        public async Task<ActionResult<List<VmGetEmployees>>> GetAllEmployees([FromQuery] GetEmployeesInputModel getEmployeesInputModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Employee"))
            {
                getEmployeesInputModel.EmployeeIds = user.EmployeeId.ToString();
            }


            if (User.IsInRole("Manager") && getEmployeesInputModel.ManagerData != true)
            {
                getEmployeesInputModel.ManagerIds = user.EmployeeId.ToString();
            }
            if (User.IsInRole("Manager") && getEmployeesInputModel.ManagerData == true)
            {
                getEmployeesInputModel.EmployeeIds = user.EmployeeId.ToString();
            }

            var employees = await _employeeService.GetAllEmployeesAsync(getEmployeesInputModel);

            var mappedEmployees = _mapper.Map<VmGetEmployees>(employees);
            if (employees == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmGetEmployees>(mappedEmployees, true, MESSAGE.LOADED));
        }


        /// <summary>
        /// Retrieves details of an employee by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the employee to retrieve</param>
        /// <returns>Returns a "VmGetEmployeeDetails" representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("{Id}", Name = "GetEmployeeById")]
        public async Task<ActionResult<VmGetEmployeeDetails>> GetEmployeeByIdAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var employee = await _employeeService.GetEmployeeByIdAsync(Id);
            var mappedEmployee = _mapper.Map<VmGetEmployeeDetails>(employee);
            if (employee == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            if (User.IsInRole("Employee") && employee.Id != user.EmployeeId)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }


            return Ok(new Response<VmGetEmployeeDetails>(mappedEmployee, true, MESSAGE.LOADED));

        }

        /// <summary>
        /// Updates an employee with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the employee to update.</param>
        /// <param name="vmUpdateEmployee">The updated employee details.</param>
        /// <returns>An ActionResult containing the updated employee details if the update was successful,
        /// NotFound response with a message if the employee is not found or the update failed.
        /// </returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPut("{Id}", Name = "UpdateEmployee")]
        public async Task<ActionResult<VmGetEmployeeDetails>> UpdateEmployee(int Id, [FromBody] VmUpdateEmployeeInput vmUpdateEmployee)
        {
            // Retrieve the current employee details
            var employee = await _employeeService.GetEmployeeByIdAsync(Id);


            if (employee == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            VmUpdateEmployee updateEmployee = new VmUpdateEmployee
            {
                UpdateUID = employee.Id,
                EmployeeId = Id,
                Email = vmUpdateEmployee.Email ?? employee.Email,
                FirstName = vmUpdateEmployee.FirstName ?? employee.FirstName,
                LastName = vmUpdateEmployee.LastName ?? employee.LastName,
                DOB = vmUpdateEmployee.DOB == null ? null : DateHelper.ConvertToYYYYMMDD(vmUpdateEmployee.DOB.ToString()),
                Gender = vmUpdateEmployee.Gender ?? employee.Gender,
                Address = vmUpdateEmployee.Address ?? employee.Address,
                DateOfJoin = vmUpdateEmployee.DateOfJoin ?? DateHelper.ConvertToYYYYMMDD(employee.DateOfJoin.ToString()),
                PhoneNumber = vmUpdateEmployee.PhoneNumber ?? employee.PhoneNumber,
                ManagerId = vmUpdateEmployee.ManagerId ?? employee.ManagerId,
                EmployeeIds = vmUpdateEmployee.EmployeeIds == null ? "" : Helpers.ArrayToString(vmUpdateEmployee.EmployeeIds),
                ProjectIds = vmUpdateEmployee.ProjectIds == null ? "" : Helpers.ArrayToString(vmUpdateEmployee.ProjectIds),
                RoleName = vmUpdateEmployee.RoleName ?? employee.Role
            };

            // Attempt to update the employee details
            bool isUpdated = _employeeService.UpdateEmployee(updateEmployee);
            if (!isUpdated)
            {
                return NotFound(new Response(MESSAGE.NOT_UPDATED, false));
            }

            // Retrieve the updated employee details
            var updatedEMployee = await _employeeService.GetEmployeeByIdAsync(Id);
            var mappedEmployee = _mapper.Map<VmGetEmployeeDetails>(updatedEMployee);

            return Ok(new Response<VmGetEmployeeDetails>(mappedEmployee, true, MESSAGE.UPDATED));
        }


        /// <summary>
        /// Deletes an employee with the specified ID.
        /// </summary>
        /// <param name="Id">The ID of the employee to delete.</param>
        /// <returns>
        /// An IActionResult representing the result of the deletion operation.
        /// If the employee is not found, returns a NotFound response.
        /// If the deletion fails, returns a NotFound response with Not Deleted message.
        /// If the deletion is successful, returns an Ok response.
        /// </returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteEmployee(int Id)
        {
            // Retrieve the employee with the specified ID
            var employee = await _employeeService.GetEmployeeByIdAsync(Id);
            if (employee == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            // Attempt to delete the employee
            bool isDeleted = _employeeService.DeleteEmployee(Id);
            if (!isDeleted)
            {
                return NotFound(new Response(MESSAGE.NOT_DELETED, false));
            }

            return Ok(new Response(MESSAGE.DELETED, true));
        }

        /// <summary>
        /// Retrieves all employees and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of employees.</returns>
        [HttpGet("EmployeesList")]
        public async Task<ActionResult<List<HelperModel>>> GetAllEmployeesHelper()
        {
            var user = await _userManager.GetUserAsync(User);
            int managerId=0;
            if (User.IsInRole("Manager"))
            {
                managerId = user.EmployeeId;
            }


            var employees = await _employeeService.GetAllEmployeesHelper(managerId);
            return Ok(new Response<List<HelperModel>>(employees, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves all only employees and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of only employees.</returns>
        [HttpGet("OnlyEmployeesList")]
        public async Task<ActionResult<List<HelperModel>>> GetOnlyEmployeesHelper()
        {
            var onlyEmployees = await _employeeService.GetOnlyEmployeesHelper();
            return Ok(new Response<List<HelperModel>>(onlyEmployees, true, MESSAGE.LOADED));
        }


        /// <summary>
        /// Retrieves all only employees and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of only employees.</returns>
        [HttpGet("EmployeesByManagerId/{Id}")]
        public async Task<ActionResult<List<VmEmployee>>> GetEmployeesByManagerHelper(int Id, [FromQuery] bool IsManagerIdReq)
        {
            var employees = await _employeeService.GetEmployeesByManagerHelper(Id,IsManagerIdReq);
            return Ok(new Response<List<VmEmployee>>(employees, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves all managers and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of managers.</returns>
        [HttpGet("ManagersList")]
        public async Task<ActionResult<List<HelperModel>>> GetAllManagersHelper()
        {
            var managers = await _employeeService.GetAllManagersHelper();
            return Ok(new Response<List<HelperModel>>(managers, true, MESSAGE.LOADED));
        }


    }
}
