using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.Service;
using Alms.DAL.Helper;
using Alms.DAL.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Data.DBHelper;

namespace Alms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AttendancesController : BaseApiController<AttendancesController>
    {
        public ILogger<AttendancesController> Logger { get; }
        public IProcedureManager _procedureManager { get; }

        private readonly IAttendanceService _attendanceService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public AttendancesController(ILogger<AttendancesController> logger, IAttendanceService attendanceService, IProcedureManager procedureManager, IMapper mapper, UserManager<ApplicationUser> userManager) : base(logger)
        {
            Logger = logger;
            _attendanceService = attendanceService;
            _procedureManager = procedureManager;
            _mapper = mapper;
            _userManager = userManager;
        }


        /// <summary>
        /// Retrieves all attendances based on specified filters.
        /// </summary>
        /// <param name="getAttendancesInputModel">Input model containing filter parameters. </param>
        /// <returns>An ActionResult containing the list of attendances.</returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("GetAllAttendance")]
        public async Task<ActionResult<VmAttendace>> GetAllAttendances([FromQuery] GetAttendancesInputModel getAttendancesInputModel)
        {
            VmAttendanceInput vmAttendanceInput = new VmAttendanceInput
            {
                EmployeeIds = getAttendancesInputModel.EmployeeIds,
                ManagerIds = getAttendancesInputModel.ManagerIds,
                fromDate = getAttendancesInputModel.fromDate,
                toDate = getAttendancesInputModel.toDate,
                Field = getAttendancesInputModel.Field,
                Page = getAttendancesInputModel.Page,
                PageSize = getAttendancesInputModel.PageSize,
                Sort = getAttendancesInputModel.Sort,
                StatusIds = getAttendancesInputModel.StatusIds
            };

            var user = await _userManager.GetUserAsync(User);

            if (User.IsInRole("Employee"))
            {
                vmAttendanceInput.EmployeeIds = user.EmployeeId.ToString();
            }
            else if (User.IsInRole("Manager") && getAttendancesInputModel.ManagerAttendance == false)
            {
                vmAttendanceInput.ManagerIds = user.EmployeeId.ToString();
            }
            else if (User.IsInRole("Manager") && getAttendancesInputModel.ManagerAttendance == true)
            {
                vmAttendanceInput.EmployeeIds = user.EmployeeId.ToString();
            }

            var attendances = await _attendanceService.GetAllAttendacesAsync(vmAttendanceInput);

            return Ok(new Response<VmAttendace>(attendances, true, MESSAGE.LOADED));
        }


        /// <summary>
        /// This endpoint asynchronously retrieves attendance details based on the provided ID.
        /// </summary>
        /// <param name="vmAttendaceDetailsInput">The input model containing attendance details.</param>
        /// <returns>
        /// Returns an ActionResult representing the HTTP response.
        /// - If the attendance details are found, returns 200 OK with the details.
        /// - If the details are not found, returns 404 Not Found with an error message.
        /// </returns>
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet(Name = "GetAttendaceById")]
        public async Task<ActionResult<VmAttendaceDetails>> GetAttendaceByIdAsync([FromQuery] VmAttendaceDetailsInput vmAttendaceDetailsInput)
        {
            var attendacne = await _attendanceService.GetAttendanceByIdAsync(vmAttendaceDetailsInput);
            var mappedAttendance = _mapper.Map<VmAttendaceDetails>(attendacne);


            if (attendacne == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmAttendaceDetails>(mappedAttendance, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// This endpoint is responsible for adding a new project attendance based on the provided input.
        /// </summary>
        /// <param name="vmAddAttendanceInput">The input model containing attendance details.</param>
        /// <returns>
        /// Returns an ActionResult representing the HTTP response.
        /// - If the project attendance is successfully added, returns 200 OK with a success message.
        /// - If the addition fails, returns 404 Not Found with an error message.
        /// </returns>
        /// 
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPost]
        public ActionResult AddAttendance([FromBody] VmAddAttendanceInput vmAddAttendanceInput)
        {
            //// Attempt to add the project details
            bool isAdded = _attendanceService.AddAttendance(vmAddAttendanceInput);
            if (!isAdded)
            {
                return NotFound(new Response(MESSAGE.NOT_ADDED, false));
            }

            return Ok(new Response(MESSAGE.SAVED, true));
        }

        /// <summary>
        /// This endpoint is used to request attendance based on the provided input.
        /// </summary>
        /// <param name="vmRequestAttendanceInput">The input model containing attendance request details.</param>
        /// <returns>
        /// Returns an ActionResult representing the HTTP response.
        /// - If the attendance request is successfully processed, returns 200 OK with a success message.
        /// - If the request fails, returns 404 Not Found with an error message.
        /// </returns
        /// 
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPost("RequestAttendance")]
        public ActionResult RequestAttendance([FromBody] VmRequestAttendanceInput vmRequestAttendanceInput)
        {
            VmAttendanceRequestModel vmAttendanceRequestModel = new VmAttendanceRequestModel
            {
                AttendanceId = vmRequestAttendanceInput.AttendanceId,
                Times = Helpers.ConvertToString(vmRequestAttendanceInput.Times)
            };

            bool isRequested = _attendanceService.RequestAttendance(vmAttendanceRequestModel);
            if (!isRequested)
            {
                return NotFound(new Response(MESSAGE.NOT_UPDATED, false));
            }
            return Ok(new Response(MESSAGE.SAVED, true));
        }

        /// <summary>
        /// This endpoint is used to update existing attendance records based on the provided input.
        /// </summary>
        /// <param name="vmAttendanceUpdateInput">The input model containing updated attendance details.</param>
        /// <returns>
        /// Returns an ActionResult representing the HTTP response.
        /// - If the attendance is successfully updated, returns 200 OK with a success message.
        /// - If the update fails, returns 404 Not Found with an error message.
        /// </returns>
        /// 
        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpPost("UpdateAttendance")]
        public ActionResult UpdateAttendance([FromBody] VmAttendanceUpdateInput vmAttendanceUpdateInput)
        {
            bool isRequested = _attendanceService.UpdateAttendance(vmAttendanceUpdateInput);
            if (!isRequested)
            {
                return NotFound(new Response(MESSAGE.NOT_UPDATED, false));
            }
            return Ok(new Response(MESSAGE.UPDATED, true));
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("GetRequestedAttendanceById")]
        public async Task<ActionResult<VmGetRequestedAttendanceById>> GetRequestedAttendanceById([FromQuery] VmGetRequestedAttendanceByIdInput vmGetRequestedAttendanceByIdInput)
        {
            var attendacne = await _attendanceService.GetRequestedAttendanceByIdAsync(vmGetRequestedAttendanceByIdInput);
            var mappedAttendance = _mapper.Map<VmGetRequestedAttendanceById>(attendacne);


            if (attendacne == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmGetRequestedAttendanceById>(mappedAttendance, true, MESSAGE.LOADED));
        }

        [Authorize(Roles = "Admin,Manager,Employee")]
        [HttpGet("GetAttendanceByAttenndanceId")]
        public async Task<ActionResult<VmGetAttendanceByAttenndanceId>> GetAttendanceByAttenndanceId([FromQuery] VmGetAttendanceByAttenndanceIdInput vmGetAttendanceByAttenndanceIdInput)
        {
            var attendacne = await _attendanceService.GetAttendanceByAttenndanceId(vmGetAttendanceByAttenndanceIdInput);
            var mappedAttendance = _mapper.Map<VmGetAttendanceByAttenndanceId>(attendacne);


            if (attendacne == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmGetAttendanceByAttenndanceId>(mappedAttendance, true, MESSAGE.LOADED));
        }
    }
}
