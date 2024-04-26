/****** Object:  StoredProcedure [dbo].[usp_GetLeaves]    Script Date: 3/20/2024 11:29:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
    Author: [Your Name]
    Description: This stored procedure retrieves leaves based on various filters such as employee ID, manager ID, leave type ID, leave status, and date range.
    Parameters:
        @Field: NVARCHAR(50) - Specifies the field to sort by.
        @Sort: NVARCHAR(50) - Specifies the sort order ('asc' for ascending, 'desc' for descending).
        @Page: BIGINT - Specifies the page number for pagination.
        @pageSize: BIGINT - Specifies the number of records per page.
        @EmployeeIds: NVARCHAR(MAX) - Comma-separated list of employee IDs to filter leaves.
        @ManagerIds: NVARCHAR(MAX) - Comma-separated list of manager IDs to filter leaves.
        @LeaveTypeIds: NVARCHAR(MAX) - Comma-separated list of leave type IDs to filter leaves.
        @StatusIds: VARCHAR(10) - Comma-separated list of leave status IDs to filter leaves.
        @From: DATE - Specifies the start date range for filtering leaves.
        @To: DATE - Specifies the end date range for filtering leaves.
    Dependencies:
        Requires tables: Leaves, employeeManager, AspNetUsers, LeaveTypes, Status
*/

CREATE PROCEDURE [dbo].[usp_GetLeaves]
    @Field nvarchar(50) = 'Id', --code
    @Sort nvarchar(50) = 'asc', -- asc
    @Page bigint = 1,
    @pageSize bigint = 10,
	@EmployeeIds nvarchar(MAX) = '',
	@ManagerIds nvarchar(MAX) = '',
	@LeaveTypeIds nvarchar(MAX) = '',
	@StatusIds varchar(10) = '',
	@From date = '',
	@To date = ''
AS
BEGIN
   
    DROP TABLE IF EXISTS #tempLeaves

	select le.id
	into #tempLeaves
	from Leaves le
	left join employeeManager em on em.employeeId = le.EmployeeId and (em.isdeleted is null OR em.isDeleted = 0)

	where 
	(le.isdeleted is null OR le.isDeleted = 0)
	and (@EmployeeIds = '' Or le.EmployeeId in (SELECT value FROM STRING_SPLIT(@EmployeeIds, ',')))
	and (@ManagerIds = '' Or em.ManagerId in (SELECT value FROM STRING_SPLIT(@ManagerIds, ',')))
	and (@LeaveTypeIds = '' Or le.LeaveTypeId in (SELECT value FROM STRING_SPLIT(@LeaveTypeIds, ',')))
	and (@StatusIds = '' Or le.StatusId in (SELECT value FROM STRING_SPLIT(@StatusIds, ',')))
	and ((@From = '' and @To = '') OR (le.StartDate < @From and le.EndDate > @To) OR (le.StartDate between @From And @To) )
	group by le.id

   select Count(*) as Count from #tempLeaves

    SELECT 
    ROW_NUMBER() OVER (Order By 
     CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'LeaveId'  then le.Id
    END asc,

	 CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'LeaveId' then le.Id
     END desc,

      CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'Name' then a.FirstName+','+a.LastName
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'Name' then a.FirstName+','+a.LastName
     END desc,

     CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'From' then le.StartDate
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'From'  then le.StartDate
     END desc,

     CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'To' then le.EndDate
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'To'  then le.EndDate
     END desc,

	  CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'NoofDays'  then le.NoOfDays
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'NoofDays'  then le.NoOfDays
     END desc,
	 
     CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'Reason' then le.Reason
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'Reason'  then le.Reason
     END desc,

	 CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'LeaveType' then lt.LeaveType
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'LeaveType'  then lt.LeaveType
     END desc,
	  CASE
       WHEN @Sort <> 'asc' then ''
       WHEN @Field = 'Status' then status.StatusName
    END ASC,

     CASE
       WHEN @Sort <> 'desc' then ''
       WHEN @Field = 'Status'  then status.StatusName
     END desc
     
     )
     as Id,
     le.Id AS LeaveId,(a.FirstName+' '+a.LastName) as Name,lt.LeaveType,le.StartDate as 'From',le.EndDate as 'To',le.NoOfDays as 'NoOfDays',le.Reason as Reason,le.StatusId,status.StatusName
    FROM Leaves le 
	inner join #tempLeaves tl on tl.Id = le.id
    
    left JOIN AspNetUsers a ON a.EmployeeId = le.EmployeeId
    Left JOIN LeaveTypes lt ON lt.id = le.LeaveTypeId
	left join status on status.Id = le.StatusId
    ORDER BY Id
    
    OFFSET (@Page-1) * @pageSize ROWS FETCH NEXT @pagesize ROWS ONLY

END

--exec [usp_GetLeaves] @Page = 1,@pageSize =10, @Field = 'No of Days',@Sort ='asc',@EmployeeIds = '',@ManagerIds='',@LeaveTypeIds='1',@StatusIds='2',@From='2023-01-01',@To='2025-01-01'
GO
