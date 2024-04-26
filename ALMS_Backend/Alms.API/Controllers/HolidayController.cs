using Alms.BLL.Interfaces;
using Alms.BLL.Service;
using Alms.DAL.Models;
using Alms.DAL.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Data.DBHelper;
using Alms.BLL.Helpers;

namespace Alms.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HolidayController : BaseApiController<HolidayController>
    {
        public ILogger<HolidayController> Logger { get; }
        public IHolidayService _holidayService { get; }
        public IProcedureManager _procedureManager { get; }

        private readonly IMapper _mapper;
        public HolidayController(ILogger<HolidayController> logger, IHolidayService holidayService, IProcedureManager procedureManager, IMapper mapper) : base(logger)
        {
            Logger = logger;
            _holidayService = holidayService;
            _procedureManager = procedureManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all holidays and returns them as a list of VmHoliday objects.
        /// </summary>
        /// <returns>An HTTP response containing the list of holidays</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VmHoliday>>> GetCategories()
        {
            var holidays = await _holidayService.GetAllAsync();
            var mappedHolidays = _mapper.Map<IEnumerable<VmHoliday>>(holidays);

            if (holidays == null)
            {
                return NotFound(new Response<IEnumerable<VmHoliday>>(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<IEnumerable<VmHoliday>>(mappedHolidays, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Retrieves a holiday by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the holiday to retrieve.</param>
        /// <returns>An HTTP response containing the holiday information.</returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<VmHoliday>> GetHolidayById(int Id)
        {
            var holiday = await _holidayService.GetByIdAsync(Id);
            var mappoedHoliday = _mapper.Map<VmHoliday>(holiday);

            if (holiday == null)
            {
                return NotFound(new Response<IEnumerable<VmHoliday>>(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VmHoliday>(mappoedHoliday, true, MESSAGE.LOADED));
        }

        /// <summary>
        /// Adds a new holiday.
        /// </summary>
        /// <param name="vmAddHoliday">The holiday information to add.</param>
        /// <returns>An HTTP response containing the added holiday information.</returns>
        [HttpPost]
        public async Task<ActionResult<VmHoliday>> AddHoliday([FromBody] VmAddHoliday vmAddHoliday)
        {
            var holidayToBeAdded = new Holiday
            {
                Name = vmAddHoliday.Name,
                StartDate = DateHelper.ConvertStringToDateOnly(vmAddHoliday.StartDate),
                EndDate = DateHelper.ConvertStringToDateOnly(vmAddHoliday.EndDate)
            };
            var holiday = _mapper.Map<Holiday>(holidayToBeAdded);
            var addedHoliday = await _holidayService.AddAsync(holiday);
            var mappedHoliday = _mapper.Map<VmHoliday>(holidayToBeAdded);

            return CreatedAtAction(nameof(GetHolidayById), new { id = addedHoliday.Id }, new Response<VmHoliday>(mappedHoliday, true, MESSAGE.SAVED));
        }

        /// <summary>
        /// Deletes a holiday by its unique identifier.
        /// </summary>
        /// <param name="Id">The unique identifier of the holiday to delete.</param>
        /// <returns>An HTTP response indicating the success of the deletion operation.</returns>
        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var holiday = await _holidayService.GetByIdAsync(Id);
            if (holiday == null)
            {
                return NotFound(new Response<object>(MESSAGE.DATA_NOT_FOUND, false));
            }

            _holidayService?.Remove(holiday);

            var mappedHoliday = _mapper.Map<VmHoliday>(holiday);

            return CreatedAtAction(nameof(GetHolidayById), new { Id = mappedHoliday.Id }, new Response<VmHoliday>(mappedHoliday, true, MESSAGE.DELETED));
        }


    }
}
