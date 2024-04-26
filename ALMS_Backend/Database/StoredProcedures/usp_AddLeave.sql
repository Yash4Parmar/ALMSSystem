/****** Object:  StoredProcedure [dbo].[usp_AddLeave]    Script Date: 3/20/2024 11:29:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  
 /*
  Author: Karan Bhalodiya

  Description: This stored procedure adds a leave record for an employee. It checks if the requested leave days exceed the total allowed days for the leave type and if the leave overlaps with existing leaves for the employee.
  If the leave is valid, it inserts a record into the Leaves table with the provided details.
  StatusId = 3 indicates a rejected leave.

  Parameters:
  @EmployeeId: INT - The ID of the employee requesting the leave.
  @StartDate: DATE - The start date of the leave.
  @EndDate: DATE - The end date of the leave.
  @NoOfDays: INT - The number of days for the leave.
  @LeaveTypeId: INT - The ID of the leave type.
  @Reason: VARCHAR(500) - The reason for the leave.
  @StatusId: INT - The status of the leave request.
  @UID: INT (optional) - The ID of the user performing the leave addition.

  Dependencies:
  Requires tables: Leaves, LeaveTypes
 */

CREATE PROCEDURE [dbo].[usp_AddLeave]
	@EmployeeId Int,
	@StartDate date,
	@EndDate date,
	@NoOfDays int,
	@LeaveTypeId int,
	@Reason varchar(500),
	@StatusId int,
	@UID int = null
AS
BEGIN

	DECLARE @Count INT
	DECLARE @TotalLeave INT
	DECLARE @IsValid BIT
	DECLARE @Massage varchar(50);


	-- Get already taken leave for this leave type and this perticular employee
	-- Below StatusId = 3 means leaves is rejected
	select @Count = Sum(NoOfDays) from Leaves where EmployeeId = @EmployeeId and LeaveTypeId= @LeaveTypeId and StatusId != 3

	--Get total leave days from leavetypes
	select @TotalLeave = days from LeaveTypes where Id = @LeaveTypeId

	
	-- Check the leave taken leaves are not greater than total leaves for this leaveType
	-- Also Check that this user also take leaves from this date
      IF @TotalLeave < @Count + @NoOfDays
         BEGIN
            SET @IsValid = 0;
            SET @Massage = 'Leave Days are not Allowed';
         END
	 -- Below StatusId = 3 means leaves is rejected
     else IF EXISTS (SELECT 1 FROM leaves WHERE EmployeeId = @EmployeeId and StatusId != 3 and ((@StartDate between StartDate and EndDate) OR (@EndDate between StartDate and EndDate) OR (@StartDate < StartDate And @EndDate > EndDate)) )
         begin
		   SET @IsValid = 0;
		    SET @Massage = 'Leave has alredy taken between this range';
         end
      ELSE 
	  begin 
	  SET @IsValid = 1; SET @Massage = 'Leave Added SuccessFully';
	  end

	-- below date for when update leave
	if @IsValid=1
	begin
	declare @InsertedDate date = CAST(GETDATE() as date)

	insert into leaves(EmployeeId,StartDate,EndDate,NoOfDays,LeaveTypeId,StatusId,Reason,InsertedDate,InsertedUID)
	values(@EmployeeId,@StartDate,@EndDate,@NoOfDays,@LeaveTypeId,@StatusId,@Reason, @InsertedDate, @UID)

	end
	
	-- return Ack data is added or not	
	select @IsValid as IsValid,@Massage as Message	
   
End

--exec [usp_AddLeave] @EmployeeId = 1,@StartDate='2024-01-01',@EndDate='2024-01-03',@NoOfDays=2,@LeaveTypeId=1,@StatusId=2,@Reason='Family vacation'
	
GO
