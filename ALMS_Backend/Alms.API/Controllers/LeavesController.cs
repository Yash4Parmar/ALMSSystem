
using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.Service;
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
using System.Drawing;
using System.Linq;
using System.Security.Claims;

namespace Alms.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class LeavesController : BaseApiController<LeavesController>
    {
        public ILogger<LeavesController> Logger { get; }
        public ILeaveService _leaveService { get; }
        public IEmployeeService _employeeService { get; }

        private readonly UserManager<ApplicationUser> _userManager;

        public IProcedureManager _procedureManager { get; }

        private readonly IMapper _mapper;


        public LeavesController(ILogger<LeavesController> logger, UserManager<ApplicationUser> userManager, ILeaveService leaveService, IEmployeeService employeeService, IProcedureManager procedureManager, IMapper mapper) : base(logger)

        {
            Logger = logger;
            _leaveService = leaveService;
            _employeeService = employeeService;
            _procedureManager = procedureManager;
            _mapper = mapper;
            _userManager = userManager;
        }


        /// <summary>
        /// Retrieves a list of Leaves based on the provided parameters using a stored procedure.
        /// </summary>
        /// </summary>
        /// <param name="getLeavesInputModel">Input model containing parameters for filtering leaves.</param>
        /// <returns> Returns a list of "VmGetLeaves representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("GetLeaves")]
        public async Task<ActionResult<List<VmGetLeaves>>> GetAllEmployees([FromQuery] GetLeavesInputModel getLeavesInputModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Employee"))
            {
                getLeavesInputModel.EmployeeIds = user.EmployeeId.ToString();
            }

            else if (User.IsInRole("Manager") && getLeavesInputModel.ManagerLeave != true)
            {
                getLeavesInputModel.ManagerIds = user.EmployeeId.ToString();
            }

            else if (User.IsInRole("Manager") && getLeavesInputModel.ManagerLeave == true)
            {
                getLeavesInputModel.EmployeeIds = user.EmployeeId.ToString();
            }

            var leaves = await _leaveService.GetAllLeavesAsync(getLeavesInputModel);

            var mappedLeaves = _mapper.Map<VmGetLeaves>(leaves);

            if (mappedLeaves.Count == 0)
            {
                mappedLeaves.Leaves = null;
                return Ok(new Response<VmGetLeaves>(mappedLeaves, true, MESSAGE.DATA_NOT_FOUND));
            }

            return Ok(new Response<VmGetLeaves>(mappedLeaves, true, MESSAGE.LOADED));
        }


        /// <summary>
        /// Retrieves details of an leave by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the leave to retrieve</param>
        /// <returns>Returns a "VmGetLeaveDetails" representing the result of the operation.</returns>
        [Authorize(Roles = "Employee, Manager, Admin")]
        [HttpGet("{Id}", Name = "GetLeaveById")]
        public async Task<ActionResult<VmGetLeaveDetails>> GetLeaveByIdAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var leave = await _leaveService.GetLeaveByIdAsync(Id);

            if (leave == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            var mappedLeave = _mapper.Map<VmGetLeaveDetails>(leave);

            if (User.IsInRole("Employee") && leave.EmployeeId != user.EmployeeId)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            //below logic whene role is manager and get leave is this manager or whitch user under this manager
            Task<List<VmEmployee>> employees = _employeeService.GetEmployeesByManagerHelper(user.EmployeeId, false);
            var v = !employees.Result.Any(e => e.Id == (int)leave.EmployeeId);

            if (User.IsInRole("Manager") && (leave.EmployeeId != user.EmployeeId && !employees.Result.Any(e => e.Id == (int)leave.EmployeeId)))
            {

                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmGetLeaveDetails>(mappedLeave, true, MESSAGE.LOADED));

        }


        /// <summary>
        /// Retrieves details of an leave by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the leave to update</param>
        /// <returns>Returns a "VmUpdateLeave" representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPost]
        public async Task<ActionResult<VmManageLeave>> UpdateLeave([FromBody] VmAddLeaveCommon vmAddLeaveCommon)
        {
            var user = await _userManager.GetUserAsync(User);

            // below check if available user is not this user's and apply for approved Status for this leave , then data is not valid
            // statusId 2 means approved
            //if (User.IsInRole("Employee") && (vmAddLeaveCommon.EmployeeId != user.EmployeeId))
            //{
            //    return BadRequest(new Response<object>(MESSAGE.NOT_VALID, false));
            // }


            VmAddLeave vmAddLeave = new VmAddLeave()
            {
                EmployeeId = vmAddLeaveCommon.EmployeeId == null || vmAddLeaveCommon.EmployeeId == 0 ? user.EmployeeId : vmAddLeaveCommon.EmployeeId,
                StartDate = vmAddLeaveCommon.StartDate,
                EndDate = vmAddLeaveCommon.EndDate,
                Reason = vmAddLeaveCommon.Reason,
                LeaveTypeId = vmAddLeaveCommon.LeaveTypeId,
                StatusId = vmAddLeaveCommon.StatusId == 0 ? 1 : vmAddLeaveCommon.StatusId, // if status id null then by default is 1 for pending
                NoOfDays = Helpers.CalculateDateDifference(vmAddLeaveCommon.StartDate, vmAddLeaveCommon.EndDate),
                UID = user.EmployeeId
            };

            if ((User.IsInRole("Employee") || (User.IsInRole("Manager") && vmAddLeave.EmployeeId == user.EmployeeId)) && vmAddLeaveCommon.StatusId == 2)
            {
                vmAddLeave.StatusId = 1; // status id 1 means leave status is pending
            }

            //below logic whene role is manager and add leave is this manager or whitch user under this manager
            Task<List<VmEmployee>> employees = _employeeService.GetEmployeesByManagerHelper(user.EmployeeId, false);

            if (vmAddLeave.NoOfDays <= 0 || (User.IsInRole("Employee") && user.EmployeeId != vmAddLeave.EmployeeId) ||
                (User.IsInRole("Manager") && (vmAddLeave.EmployeeId != user.EmployeeId && !employees.Result.Any(e => e.Id == (int)vmAddLeave.EmployeeId)))
                )
            {
                return BadRequest(new Response<object>(MESSAGE.NOT_VALID, false));
            }

            VmManageLeave addLeave = await _leaveService.AddLeave(vmAddLeave);

            if (addLeave.IsValid == false)
            {
                return BadRequest(new Response(addLeave.Message, false));
            }


            return Ok(new Response(MESSAGE.SAVED, true));

        }


        /// <summary>
        /// Retrieves details of an leave by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the leave to update</param>
        /// <returns>Returns a "VmUpdateLeave" representing the result of the operation.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPut("{Id}")]
        public async Task<ActionResult<VmManageLeave>> UpdateLeave(int Id, [FromBody] VmLeaveCommon vmLeaveCommon)
        {
            var user = await _userManager.GetUserAsync(User);

            Leave leave = await _leaveService.GetByIdAsync(Id);
            if (leave == null)
            {
                return NotFound(new Response<object>(MESSAGE.DATA_NOT_FOUND, false));
            }

            // below check if available leave is not this user's and apply for approved Status for this leave , then data is not valid
            // statusId 2 means approved
            if (User.IsInRole("Employee") && (leave.EmployeeId != user.EmployeeId || leave.StatusId == 2 || vmLeaveCommon.StatusId == 2))
            {
                return BadRequest(new Response(MESSAGE.NOT_VALID, false));
            }

            //below logic whene role is manager and add leave is this manager or whitch user under this manager
            Task<List<VmEmployee>> employees = _employeeService.GetEmployeesByManagerHelper(user.EmployeeId, false);

            // below check if available leave is not this user's and apply for approved Status for this leave , then data is not valid
            // statusId 2 means approved
            if (User.IsInRole("Manager") && (((leave.EmployeeId != user.EmployeeId || vmLeaveCommon.StatusId == 2) && !employees.Result.Any(e => e.Id == (int)leave.EmployeeId)) || leave.StatusId == 2))
            {
                return BadRequest(new Response(MESSAGE.NOT_VALID, false));
            }


            VmLeave vmLeave = new VmLeave()
            {
                Id = Id,
                StartDate = vmLeaveCommon.StartDate,
                EndDate = vmLeaveCommon.EndDate,
                Reason = vmLeaveCommon.Reason,
                LeaveTypeId = vmLeaveCommon.LeaveTypeId,
                StatusId = vmLeaveCommon.StatusId == 0 ? leave.StatusId : vmLeaveCommon.StatusId, // if status id null then by default is 1 for pending
                NoOfDays = Helpers.CalculateDateDifference(vmLeaveCommon.StartDate, vmLeaveCommon.EndDate),
                UID = user.EmployeeId
            };

            if (vmLeave.NoOfDays <= 0)
            {
                return BadRequest(new Response<object>(MESSAGE.NOT_VALID, false));
            }

            VmManageLeave updateLeave = await _leaveService.UpdateLeave(vmLeave);

            if (updateLeave.IsValid == false)
            {
                return BadRequest(new Response(updateLeave.Message, false));
            }


            return Ok(new Response(MESSAGE.UPDATED, true));

        }

        /// <summary>
        /// Delete of an Leave by their unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the Leave to retrieve</param>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpDelete("{Id}")]
        public async Task<ActionResult<Leave>> DeleteLeaveByIdAsync(int Id)
        {
            var user = await _userManager.GetUserAsync(User);

            Leave leave = await _leaveService.GetByIdAsync(Id);
            if (leave == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            // below check if available leave is not this user's and apply for approved Status for this leave , then data is not valid
            // statusId 2 means approved
            if (User.IsInRole("Employee") && (leave.EmployeeId != user.EmployeeId || leave.StatusId == 2))
            {
                return BadRequest(new Response(MESSAGE.NOT_DELETED, false));
            }

            // below check if available leave is not this user's and apply for approved Status for this leave , then data is not valid
            // statusId 2 means approved
            if (User.IsInRole("Manager") && (leave.EmployeeId != user.EmployeeId || leave.StatusId == 2))
            {
                return BadRequest(new Response(MESSAGE.NOT_DELETED, false));
            }

            Leave deleteLeave = await _leaveService.Remove(leave);


            if (deleteLeave == null)
            {
                return NotFound(new Response(MESSAGE.NOT_DELETED, false));
            }

            return Ok(new Response(MESSAGE.DELETED, true));

        }

        /// <summary>
        /// Retrieves all leaveTypes and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of LeaveTypes.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("leaveTypes/list")]
        public async Task<ActionResult<List<HelperModel>>> GetAllLeaveTypes()
        {
            var leaveTypes = await _leaveService.GetAllLeaveTypes();
            return Ok(new Response<List<HelperModel>>(leaveTypes, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves all Status and returns them as a list of HelperModel objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of Status.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("/api/status/list")]
        public async Task<ActionResult<List<HelperModel>>> GetAllStatus()
        {
            var status = await _leaveService.GetAllStatus();
            return Ok(new Response<List<HelperModel>>(status, true, MESSAGE.LOADED));
        }

    }
}

