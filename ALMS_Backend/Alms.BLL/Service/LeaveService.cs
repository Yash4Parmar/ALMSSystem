using Alms.BLL.Helpers;
using Alms.BLL.Interfaces;
using Alms.BLL.SQlRepository;
using Alms.DAL.Models;
using Alms.DAL.Repository;
using Alms.DAL.TempModels;
using Alms.DAL.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Service.Data.DBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alms.BLL.Service
{
    public class LeaveService : ILeaveService
    {
        private readonly IMapper _mapper;
        public IDBRepository<Leave> _leaveRepo { get; }
        public IProcedureManager _procedureManager { get; }

        public LeaveService(IProcedureManager procedureManager, IMapper mapper, IDBRepository<Leave> leaveRepo)
        {
            _procedureManager = procedureManager;
            _leaveRepo = leaveRepo;
            _mapper = mapper;

        }

        /// <summary>
        ///  Retrieves a paginated list of employees asynchronously using a stored procedure.
        /// </summary>
        /// <param name="getLeavesInputModel">Input parameters for the stored procedure.</param>
        /// <returns>A task representing the asynchronous operation, containing the paginated list of employees.</returns>
        public Task<VmGetLeaves> GetAllLeavesAsync(GetLeavesInputModel getLeavesInputModel)
        {
            var spParameters = new GetLeavesInputModel
            {
                EmployeeIds = getLeavesInputModel.EmployeeIds == null ? "" : getLeavesInputModel.EmployeeIds,
                ManagerIds = getLeavesInputModel.ManagerIds == null ? "" : getLeavesInputModel.ManagerIds,
                LeaveTypeIds = getLeavesInputModel.LeaveTypeIds == null ? "" : getLeavesInputModel.LeaveTypeIds,
                StatusIds = getLeavesInputModel.StatusIds == null ? "" : getLeavesInputModel.StatusIds,
                fromDate = getLeavesInputModel.fromDate == null ? "" : getLeavesInputModel.fromDate,
                toDate = getLeavesInputModel.toDate == null ? "" : getLeavesInputModel.toDate,
                Page = getLeavesInputModel.Page,
                PageSize = getLeavesInputModel.PageSize,
                Field = getLeavesInputModel.Field,
                Sort = getLeavesInputModel.Sort
            };

            var leaves = _procedureManager.ExecStoreProcedureMulResults<StoredProcedureCommonModel, VmSPGetLeaves>(StoredProcedures.GetLeaves, spParameters);

            var countData = leaves.Item1[0].Count;
            var LeavesData = leaves.Item2;

            // below logic when need to map status obj and leave type obj
            // List<VmGetLeavesWithStatus> AllLeaves  =_mapper.Map<List<VmGetLeavesWithStatus>>(LeavesData);

            VmGetLeaves getLeaves = new VmGetLeaves
            {
                Count = (int)countData,
                Leaves = LeavesData

            };
            return Task.FromResult(getLeaves);
        }

        /// <summary>
        /// Retrieves detailed information about an leave asynchronously based on the provided leave ID.
        /// sets properties with VmGetLeaveDetails
        /// </summary>
        /// <param name="Id">The unique identifier of the leave</param>
        /// <returns>A task representing the asynchronous operation, containing detailed information about the leave,
        public Task<VmGetLeaveDetails> GetLeaveByIdAsync(int Id)
        {
            //Create parameter for stored procedure
            var leaveId = new DBSQLParameter("Id", Id);

            List<DBSQLParameter> dBSQLParameters = [leaveId];

            // Execute the stored procedure to get a list of leave details
            var leave = _procedureManager.ExecStoreProcedure<VmGetLeaveDetails>(StoredProcedures.GetLeaveById, dBSQLParameters);


            // Copy properties from the leave to the leaveDetails using an extension method
            VmGetLeaveDetails? leaveDetails = leave.Count == 0 ? null : _mapper.Map<VmGetLeaveDetails>(leave[0]);


            return Task.FromResult(leaveDetails);
        }


        public Task<VmManageLeave> UpdateLeave(VmLeave vmLeave)
        {

            var leave = _procedureManager.ExecStoreProcedure<VmManageLeave>(StoredProcedures.UpdateLeave, vmLeave);

            return Task.FromResult(leave[0]);
        }

        public Task<VmManageLeave> AddLeave(VmAddLeave vmAddLeave)
        {

            var leave = _procedureManager.ExecStoreProcedure<VmManageLeave>(StoredProcedures.AddLeave, vmAddLeave);

            return Task.FromResult(leave[0]);
        }

        public async Task<Leave> GetByIdAsync(int Id)
        {
            return await _leaveRepo.GetByIdAsync(Id);
        }

        public Task<Leave> Remove(Leave leave)
        {
            return _leaveRepo.Remove(leave);
        }

        /// <summary>
        /// Retrieves all leaveTypes and returns them as a list of HelperModel objects.
        /// </summary>
        public Task<List<HelperModel>> GetAllLeaveTypes()
        {
            var tableName = new DBSQLParameter("Type", DAL.ViewModels.Constants.AllLeaveTypes);
            List<DBSQLParameter> dBSQLParameters = [tableName];

            var leaveTypes = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(leaveTypes);
        }

        /// <summary>
        /// Retrieves all Status and returns them as a list of HelperModel objects.
        /// </summary>
        public Task<List<HelperModel>> GetAllStatus()
        {
            var tableName = new DBSQLParameter("Type", DAL.ViewModels.Constants.AllStatus);
            List<DBSQLParameter> dBSQLParameters = [tableName];

            var status = _procedureManager.ExecStoreProcedure<HelperModel>(StoredProcedures.FilteredData, dBSQLParameters);

            return Task.FromResult(status);
        }
    }


}
