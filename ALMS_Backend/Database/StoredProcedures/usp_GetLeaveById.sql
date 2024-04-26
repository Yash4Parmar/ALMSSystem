/****** Object:  StoredProcedure [dbo].[usp_GetLeaveById]    Script Date: 3/20/2024 11:29:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
    Author: Karan Bhalodiya
    Description: This stored procedure retrieves leave information by its ID.
    Parameters:
        @Id: INT - The ID of the leave to retrieve.
    Dependencies:
        Requires tables: Leaves, AspNetUsers, LeaveTypes, Status
*/

CREATE PROCEDURE [dbo].[usp_GetLeaveById]
    @Id INT 
AS
BEGIN

    select le.Id AS LeaveId,le.EmployeeId,(a.FirstName+' '+a.LastName) as Name,lt.LeaveType,lt.Id as LeaveTypeId,le.StartDate as 'From',le.EndDate as 'To',le.NoOfDays as NoOfDays,le.StatusId,status.StatusName
    FROM Leaves le 
    left JOIN AspNetUsers a ON a.EmployeeId = le.EmployeeId
    Left JOIN LeaveTypes lt ON lt.id = le.LeaveTypeId
	left join status on status.Id = le.StatusId
    where 
	(le.IsDeleted is null OR le.IsDeleted =0)
	and le.Id = @Id
End

--exec usp_GetLeaveById @Id = 6 
GO
