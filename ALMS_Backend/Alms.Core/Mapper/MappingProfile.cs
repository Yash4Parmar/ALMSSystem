using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoMapper;
using Alms.DAL.Models;
using Alms.DAL.ViewModels;

namespace Alms.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AspNetUser, VmGetEmployees>().ReverseMap();

            CreateMap<VmSPGetLeaves, VmGetLeavesWithStatus>()
               // Mapping the Status property
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new VmStatus
               {
                   StatusId = src.StatusId,
                   StatusName = src.StatusName
               }))
                .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => new VmLeaveTypes
                {
                    LeaveTypeId = src.LeaveTypeId,
                    LeaveTypeName = src.LeaveTypeName
                }));

            /*CreateMap<VmSPGetLeaveDetails, VmGetLeaveDetails>()
             // Mapping the Status property
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => new VmStatus
             {
                 StatusId = src.StatusId,
                 StatusName = src.StatusName
             }))

             // Mapping the Employee property directly from VmSPGetLeaveDetails
             .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => new VmLeaveEmployeeObj
             {
                 EmployeeId = src.EmployeeId,
                 Name = src.Name
             }))
            // Mapping the LeaveType property directly from VmSPGetLeaveDetails
            .ForMember(dest => dest.LeaveType, opt => opt.MapFrom(src => new VmLeaveTypeObj
            {
                 LeaveTypeId = src.LeaveTypeId,
                 LeaveType = src.LeaveType
             }));*/


            CreateMap<AspNetUser, VmGetEmployeeDetails>().ReverseMap();
            CreateMap<Holiday, VmHoliday>().ReverseMap();
            CreateMap<Holiday, VmAddHoliday>().ReverseMap();
            CreateMap<VmHoliday, VmAddHoliday>().ReverseMap();

            CreateMap<VmAttendaceDetails, VmAttendaceDetailsResponse>().ReverseMap();

            CreateMap<VmGetRequestedAttendanceById, VmGetRequestedAttendanceByIdResponse>().ReverseMap();

            CreateMap<VmGetAttendanceByAttenndanceId, VmGetAttendanceByAttenndanceIdResponse>().ReverseMap();

        }
    }
}
