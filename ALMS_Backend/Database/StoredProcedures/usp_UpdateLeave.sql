/****** Object:  StoredProcedure [dbo].[usp_UpdateLeave]    Script Date: 3/20/2024 11:29:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_UpdateLeave]
    @Id INT,
	@StartDate date,
	@EndDate date,
	@NoOfDays int,
	@LeaveTypeId int,
	@Reason varchar(500),
	@StatusId int,
	@UID int = null
AS
BEGIN

	DECLARE @EmployeeId INT
	DECLARE @Count INT
	DECLARE @TotalLeave INT
	DECLARE @IsValid BIT
	DECLARE @Massage varchar(50);

	-- Get EmployeeId From this leaveId 
	select @EmployeeId = EmployeeId from Leaves where Id = @Id

	-- Get already taken leave for this leave type and this perticular employee
	-- Below StatusId = 3 means leaves is rejected
	select @Count = Sum(NoOfDays) from Leaves where EmployeeId = @EmployeeId and LeaveTypeId= @LeaveTypeId and Id != @Id and StatusId != 3

	--Get total leave days from leavetypes
	select @TotalLeave = days from LeaveTypes where Id = @LeaveTypeId

	--Check leave conflict
	

	-- Check the leave taken leaves are not greater than total leaves for this leaveType
	-- Also Check that this user also take leaves from this date
      IF @TotalLeave < @Count + @NoOfDays
         BEGIN
            SET @IsValid = 0;
            SET @Massage = 'Leave Days are not Allowed';
         END
	 -- Below StatusId = 3 means leaves is rejected
     else IF EXISTS (SELECT 1 FROM leaves WHERE EmployeeId = @EmployeeId and id != @Id and StatusId != 3 and ((@StartDate between StartDate and EndDate) OR (@EndDate between StartDate and EndDate) OR (@StartDate < StartDate And @EndDate > EndDate)) )
         begin
		   SET @IsValid = 0;
		    SET @Massage = 'Leave has alredy taken between this range';
         end
      ELSE 
	  begin 
	  SET @IsValid = 1; SET @Massage = 'Leave Updates SuccessFully';
	  end

	-- below date for when update leave
	if @IsValid=1
	begin
	declare @UpdatedDate date = CAST(GETDATE() as date)

	update leaves set 
	StartDate=@StartDate,
	EndDate = @EndDate,
	NoOfDays=@NoOfDays,
	LeaveTypeId = @LeaveTypeId,
	StatusId = @StatusId,
	Reason = @Reason,
	UpdateDate = @UpdatedDate,
	UpdatedUID = @UID
	where Id = @Id

	end
	
	-- return ack data is update or not	
	select @IsValid as IsValid,@Massage as Message	
   
End

--exec [usp_UpdateLeave] @Id = 6,@StartDate='2024-01-01',@EndDate='2024-01-03',@NoOfDays=2,@LeaveTypeId=1,@StatusId=2,@Reason='Family vacation'



 
GO
